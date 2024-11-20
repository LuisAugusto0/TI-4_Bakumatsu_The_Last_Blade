using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable


public abstract class BaseChargeRepeatingEffect<TEffect> : IChargeStatusEffect
where TEffect : INonPersistantEffect
{
    EffectReceiver target;
    TEffect effect;
    int totalCharges; 
    int chargeConsumeRate;
    float interval;
    readonly Action? onEffectEnd;

    int charges = 0;
    float lastIntervalActivatedTime = float.MinValue;

    Coroutine? effectCoroutine;

    
    public EffectReceiver GetTarget() => target;

    public BaseChargeRepeatingEffect(
        TEffect effect, 
        int totalCharges, 
        int chargeConsumeRate, 
        float interval, 
        Action? onEffectEnd
    )
    {
        this.target = effect.GetTarget();
        this.effect = effect;
        this.totalCharges = totalCharges;
        this.chargeConsumeRate = chargeConsumeRate;
        this.onEffectEnd = onEffectEnd;
        this.interval = interval;


        effectCoroutine = target.StartCoroutine(Coroutine());
    }

    public abstract Sprite GetIcon();
    public bool IsActive() => effectCoroutine == null; 
    public int GetRemaningCharges() => charges;

    public float GetRemainingIntervalDuration() 
    {
        return (lastIntervalActivatedTime + interval) - Time.time;
    }


    public void Start()
    {
        // Always refresh charges
        charges = totalCharges;

        // Inactive to active
        if (effectCoroutine == null)
        {
            target.AddChargeStatusEffect(this);
            effectCoroutine = target.StartCoroutine(Coroutine());
        }
    }



    public void Refresh(int charges)
    {
        this.charges = Mathf.Max(this.charges, charges);
    }

    public void AddToCharges(int charges)
    {
        this.charges += charges;
        if (charges <= 0)
        {
            ForceStop();
        }
    }

    IEnumerator Coroutine()
    {
        while (charges > 0)
        {
            lastIntervalActivatedTime = Time.time;
            yield return new WaitForSeconds(interval);
            effect.Start();
            charges -= chargeConsumeRate;
        }
        onEffectEnd?.Invoke();
        effectCoroutine = null;
        target.RemoveChargeStatusEffect(this);

    }


    public void ForceStop()
    {
        if (effectCoroutine != null)
        {
            target.StopCoroutine(effectCoroutine);
            effectCoroutine = null;
            target.RemoveChargeStatusEffect(this);
            onEffectEnd?.Invoke();
        }
    }


    public override string ToString()
    {
        string typeString = $"Repeating Charge Effect {this.GetType()} for {effect.GetType()} => ";

        if (!IsActive())
        {
            return typeString + "Not Active";
        }
    
        string s1 = $"Remaning charges: {GetRemaningCharges()}, Current interval: ";
        string s2 = $"{GetRemainingIntervalDuration()}";
        return typeString + s1 + s2;
        
    }

}

