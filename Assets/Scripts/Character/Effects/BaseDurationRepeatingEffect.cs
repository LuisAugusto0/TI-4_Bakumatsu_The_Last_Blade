using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

/* Defines General Effect that refreshes inner Effect every interval
 *
 * Inner effect may be instantaneous or not. Since only one instance is created,

 */
public abstract class BaseTimedRepeatingEffect<TEffect> : ITimedStatusEffect
where TEffect : INonPersistantEffect
{
    readonly EffectReceiver target;
    readonly TEffect effect;
    readonly float interval;
    float totalDuration;
    readonly Action? onEffectEnd;

    float lastActivatedTime = float.MinValue;
    float endTime = float.MinValue;

    Coroutine? durationCoroutine;
    Coroutine? effectCoroutine;

    public EffectReceiver GetTarget() => target;

    public BaseTimedRepeatingEffect(
        TEffect effect, float totalDuration, float interval, Action? onEffectEnd)
    {
        if (totalDuration < interval)
        {
            Debug.LogError("Interval cannot be higher than totalDuration");
        }

        this.target = effect.GetTarget();
        this.effect = effect;
        this.totalDuration = totalDuration;
        this.interval = interval;
        this.onEffectEnd = onEffectEnd;
    }

    public abstract Sprite GetIcon();
    public bool IsActive() => durationCoroutine == null;

    public float GetRemainingDuration()
    {
        return (endTime) - Time.time;
    }



    public void Start() 
    {
        this.lastActivatedTime = Time.time;

        // Inactive to Active
        if (durationCoroutine == null)
        {
            effectCoroutine = target.StartCoroutine(EffectCoroutine());
            target.AddTimedStatusEffect(this);
        }
        else
        {
            // Reset only duration coroutine (keep effect clock intact)
            target.StopCoroutine(durationCoroutine);
        }

        durationCoroutine = target.StartCoroutine(DurationCoroutine(totalDuration));
        endTime = Time.time + totalDuration;
    }

    IEnumerator DurationCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        target.StopCoroutine(effectCoroutine);
        target.RemoveTimedStatusEffect(this);
        onEffectEnd?.Invoke();
        effectCoroutine = null; 
        durationCoroutine = null;
    }


    public void UpdateRemaningTime(float newTime)
    {
        float remainingTime = Time.time - lastActivatedTime;

        if (remainingTime < newTime) 
        {
            lastActivatedTime = Time.time;
            target.StopCoroutine(durationCoroutine);
            durationCoroutine = target.StartCoroutine(DurationCoroutine(newTime));
            endTime = Time.time + newTime;
        }
    }

    public void ChangeDuration(float newDuration)
    {
        UpdateRemaningTime(newDuration);
        this.totalDuration = newDuration;
    }

 

    IEnumerator EffectCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            effect.Start();

        }
    }


    public void ForceStop()
    {
        if (durationCoroutine != null)
        {
            target.StopCoroutine(durationCoroutine);
        }

        if (effectCoroutine != null)
        {
            target.StopCoroutine(effectCoroutine);
            effectCoroutine = null;
        }

        target.RemoveTimedStatusEffect(this);
        onEffectEnd?.Invoke();
    }


    public override string ToString()
    {
        string typeString = $"Repeating Duration Effect {this.GetType()} for {effect.GetType()} => ";

        if (!IsActive())
        {
            return typeString + "Not Active";
        }
        
        return $"Remaning duration: {GetRemainingDuration()}";
    }


}

