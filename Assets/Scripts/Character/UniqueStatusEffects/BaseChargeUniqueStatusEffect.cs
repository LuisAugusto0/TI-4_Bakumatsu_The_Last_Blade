using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
#nullable enable

public interface IChargeUniqueEffect : IUniqueStatusEffect
{
    public IChargeStatusEffect GetEffectReference();
}

public abstract class BaseChargeUniqueEffect<TEffect> : IChargeUniqueEffect 
where TEffect : IChargeStatusEffect
{
    UniqueStatusEffectManager manager;
    TEffect effect;

    public BaseChargeUniqueEffect(UniqueStatusEffectManager manager, int baseCharges)
    {
        this.manager = manager;
        this.effect = GetEffect(manager.effectReceiver, baseCharges);
        this.effect.Start();
    }

    public UniqueStatusEffectManager GetUniqueStatusEffectManager() => manager;
    public abstract TEffect GetEffect(EffectReceiver target, int charges);
    public IChargeStatusEffect GetEffectReference() => effect;

    public void Remove()
    {
        manager.RemoveChargeEffect(this.GetType());
    }
}


