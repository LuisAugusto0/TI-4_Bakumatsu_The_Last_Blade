using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

public interface IEffect 
{
    public EffectReceiver GetTarget();
}


public abstract class BaseEffect : IEffect
{
    readonly public EffectReceiver target;
    protected bool isActive = false;

    protected BaseEffect(EffectReceiver target) 
    {
        this.target = target;
    }

    public EffectReceiver GetTarget() => target;
    public abstract void Start();

}



// Any effect type that can be updated (even if not directly UpdatableEffect)
public interface IUpdatableEffect<T> : IEffect
{
    public void Update(T param);
}
