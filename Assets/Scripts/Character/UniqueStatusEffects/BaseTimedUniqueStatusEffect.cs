using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

public interface ITimedUniqueEffect : IUniqueStatusEffect
{
    public ITimedStatusEffect GetEffectReference();
}

public abstract class BaseTimedUniqueEffect<TEffect> : ITimedUniqueEffect 
where TEffect : ITimedStatusEffect
{
    UniqueStatusEffectManager manager;
    TEffect effect;

    public BaseTimedUniqueEffect(UniqueStatusEffectManager manager, float duration)
    {
        this.manager = manager;
        this.effect = GetEffect(manager.effectReceiver, duration);
        this.effect.Start();
    }

    public UniqueStatusEffectManager GetUniqueStatusEffectManager() => manager;
    public abstract TEffect GetEffect(EffectReceiver target, float duration);
    public ITimedStatusEffect GetEffectReference() => effect;

    public void Remove()
    {
        manager.RemoveTimedEffect(this.GetType());
    }

}