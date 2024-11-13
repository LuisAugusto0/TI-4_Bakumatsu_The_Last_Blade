using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimedEffect<TEffect> where TEffect : Effect {
    protected readonly TEffect effect;
    protected readonly EffectReceiver target;
    public float duration;
    Coroutine coroutine = null;

    public TimedEffect(TEffect effect, float duration) 
    {
        this.target = effect.target;
        this.effect = effect;
        this.duration = duration;
    }

    public void StartCoroutine() 
    {
        if (coroutine != null) 
        {
            target.StopCoroutine(coroutine);
        }
        coroutine = target.StartCoroutine(Coroutine());
    }

    public void UpdateDuration(float newDuration)
    {
        this.duration = newDuration;
    }

    IEnumerator Coroutine() 
    {
        effect.Start();
        yield return new WaitForSeconds(duration);
        effect.End();
    }

    public void ForceStop() 
    {
        if (coroutine != null)
        {
            target.StopCoroutine(coroutine);
        }
    }
}


public class UpdatableTimedEffect<TEffect, TModifier> : TimedEffect<TEffect> 
where TEffect : UpdatableEffect<TModifier> 
{
    public UpdatableTimedEffect(TEffect effect, float duration) 
    : base (effect, duration) { }

    public void UpdateEffect(TModifier modifier) 
    {
        effect.Update(modifier);
    }
    
}