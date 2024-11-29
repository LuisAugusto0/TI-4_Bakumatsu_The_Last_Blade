using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

/* Local refreshable timer for when to Start / End effect
 *
 * Only accepts non persistant effects that can be ended manually
 * Generic types are used to allow implementation on UpdatableEffect as well
 */
public abstract class TimedEffect<TEffect> : INonPersistantEffect, ITimedStatusEffect, IEffect
where TEffect : IPersistantEffect {
    public readonly TEffect effect;
    public readonly EffectReceiver target;
    readonly Action? onEffectEnd; // Delegate for effect end callback
    float lastActivatedTime = float.MinValue;
    float endTime = float.MinValue;
    public float duration;
    Coroutine? coroutine = null;

    public TimedEffect(TEffect effect, float duration, Action? onEffectEnd) 
    {
        this.target = effect.GetTarget();
        this.effect = effect;
        this.duration = duration;
        this.onEffectEnd = onEffectEnd;
    }

    public EffectReceiver GetTarget() => target;
    public abstract Sprite? GetIcon();
    public bool IsActive() => coroutine == null;


    public float GetRemainingDuration()
    {
        return endTime - Time.time;
    }


    public void Start() 
    {
        // Inactive to active
        if (coroutine == null)
        {
            target.AddTimedStatusEffect(this);
        }
        else
        {
            // Reset timer
            target.StopCoroutine(coroutine);
        }
        effect.Start();
        lastActivatedTime = Time.time;
        endTime = Time.time + duration;
        coroutine = target.StartCoroutine(Coroutine(this.duration));
        
    }


    /* Update remaning time if time provided is higher than current
     * remaning time
     *
     * Time provided may exceed the base duration, in which case the
     * duration is not changed, and the new running is used regardless
     */
    public void UpdateRemaningTime(float newTime)
    {
        if (coroutine != null)
        {
            float remainingTime = Time.time - lastActivatedTime;

            // Refresh duration to new higher value, without changing base duration
            if (remainingTime < newTime) 
            {
                target.StopCoroutine(coroutine);
                lastActivatedTime = Time.time;
                endTime = Time.time + newTime;
                coroutine = target.StartCoroutine(Coroutine(newTime));
            }
        }
    }

    // Ensures duration is not exceeded
    public void EnforcedUpdateRemaningTime(float newTime)
    {
        UpdateRemaningTime(Mathf.Min(newTime, duration));
    }
    
    
    // Update current duration and refresh
    public void RefreshUpdateDuration(float newDuration)
    {
        UpdateRemaningTime(newDuration);
        this.duration = newDuration;
    }



    IEnumerator Coroutine(float time) 
    {
        yield return new WaitForSeconds(time);
        effect.End();
        target.RemoveTimedStatusEffect(this);
        onEffectEnd?.Invoke();
    }


    public void ForceStop() 
    {
        if (coroutine != null)
        {
            target.StopCoroutine(coroutine);
            effect.End();
            target.RemoveTimedStatusEffect(this);
            onEffectEnd?.Invoke();
        }
    }

    public override string ToString()
    {
        string typeEffect = $"Timed Effect {this.GetType()} for {effect.GetType()} => ";

        if (!IsActive())
        {
            return typeEffect + "Not Active";
        }
     
        return typeEffect + $"Remaning duration: {GetRemainingDuration()}";
    }
}




