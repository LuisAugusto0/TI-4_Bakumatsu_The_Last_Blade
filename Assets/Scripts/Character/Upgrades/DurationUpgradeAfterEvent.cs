using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;


// Scales with increase in duration
public abstract class BaseDurationUpgradeAfterEvent<TEffect> : Upgrade
    where TEffect : Effect
{ 
    public static float TimeMultiplier = 1f;

    protected TimedEffect<TEffect> timedEffect;
    protected Coroutine activeCoroutine = null;

    protected BaseDurationUpgradeAfterEvent(UpgradeManager target, int quantity)
        : base(target, quantity)
    {
        this.timedEffect = CreateTimedEffect();
        UpdateDuration();
    }

    // Create TimedEffect from subclass
    protected abstract TimedEffect<TEffect> CreateTimedEffect();

    // Get base duration from subclass
    protected abstract float GetBaseDuration();


    protected override void UpdateQuantity(int quantity)
    {
        base.UpdateQuantity(quantity);
        UpdateDuration();
    }

    protected void UpdateDuration()
    {
        float additionalMultiplier = (quantity - 1) * TimeMultiplier;
        float newDuration = GetBaseDuration() * (1 + additionalMultiplier);
        timedEffect.UpdateDuration(newDuration);
    }

    public override void Remove()
    {
        base.Remove();
        timedEffect.ForceStop();
    }
}





public class ImmunityAfterHitUpgrade : BaseDurationUpgradeAfterEvent<ImmunityEffect>
{
    public static readonly float BaseDuration = 2f;

    public ImmunityAfterHitUpgrade(UpgradeManager target, int quantity)
        : base(target, quantity)
    {
        SubscribeToOnHitEvent();
    }

    protected override float GetBaseDuration() => BaseDuration;
    
    protected override TimedEffect<ImmunityEffect> CreateTimedEffect()
    {
        return new TimedEffect<ImmunityEffect>(
            new ImmunityEffect(target.effectReceiver), BaseDuration
        );
    }

    private void SubscribeToOnHitEvent()
    {
        target.effectReceiver.damageable.onHit.AddListener(
            (object o, Damageable d) => timedEffect.StartCoroutine()
        );
    }
}







