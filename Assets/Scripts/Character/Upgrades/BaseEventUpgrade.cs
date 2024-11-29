using Effects.Implementations.PersistantEffect;
using Effects.Implementations.TimedEffect;
using UnityEngine;



// Scales with increase in duration
public abstract class BaseUpgradeAfterEventEffect<TEffect> : BaseUpgrade
    where TEffect : ILifeTimedEffect
{ 
    protected TEffect effect;
    protected Coroutine activeCoroutine = null;

    protected BaseUpgradeAfterEventEffect(UpgradeManager target, int quantity)
        : base(target, quantity)
    {
        this.effect = GetEffect();
        SubscribeToEvent();
    }

    protected abstract TEffect GetEffect();
    protected abstract void Update();
    protected abstract void SubscribeToEvent();
    protected abstract void UnsubscribeToEvent();

    protected override void UpdateQuantity(int quantity)
    {
        base.UpdateQuantity(quantity);
        Update();
    }

    public override void Remove()
    {
        base.Remove();
        effect.ForceStop();
        UnsubscribeToEvent();
    }
}


public abstract class BaseUpgradeAfterEvent : BaseUpgrade
{ 
    protected Coroutine activeCoroutine = null;

    protected BaseUpgradeAfterEvent(UpgradeManager target, int quantity)
        : base(target, quantity)
    {
        SubscribeToEvent();
    }

    protected abstract void Update();
    protected abstract void SubscribeToEvent();
    protected abstract void UnsubscribeToEvent();

    protected override void UpdateQuantity(int quantity)
    {
        base.UpdateQuantity(quantity);
        Update();
    }

    public override void Remove()
    {
        base.Remove();
        UnsubscribeToEvent();
    }
}
