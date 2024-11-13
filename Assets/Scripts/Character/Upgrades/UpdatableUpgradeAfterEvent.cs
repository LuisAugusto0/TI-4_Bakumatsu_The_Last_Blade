using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Scales by updating effect
public abstract class BaseUpdatableUpgradeAfterEvent<TEffect, TModifier> : Upgrade
    where TEffect : UpdatableEffect<TModifier>
{ 
    // How much each subsequent quantity after 1 affects the effect duration
    public static float timeMultiplier = 1f;

    protected UpdatableTimedEffect<TEffect, TModifier> timedEffect;
    protected Coroutine activeCoroutine = null;

    public BaseUpdatableUpgradeAfterEvent(UpgradeManager entity, int quantity) 
    : base(entity, quantity) 
    {
        this.timedEffect = CreateTimedEffect();

    }

    protected abstract UpdatableTimedEffect<TEffect, TModifier> CreateTimedEffect();
    protected abstract TModifier GetNewModifier();  
    protected abstract float GetBaseDuration();

    protected override void UpdateQuantity(int quantity)
    {
        base.UpdateQuantity(quantity);
        timedEffect.UpdateEffect(GetNewModifier());
    }


    public override void Remove() {
        base.Remove();
        timedEffect.ForceStop();
    }
}


public abstract class BaseSpeedBonusAfterHitUpgrade : BaseUpdatableUpgradeAfterEvent<SpeedBonusEffect, float> {

    public BaseSpeedBonusAfterHitUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity) 
    {
        SubscribeToOnHitEvent();
    }

    public abstract float GetSpeedBonus();


    private void SubscribeToOnHitEvent()
    {
        target.effectReceiver.damageable.onHit.AddListener(
            (object o, Damageable d) => timedEffect.StartCoroutine()
        );
    }

    protected override UpdatableTimedEffect<SpeedBonusEffect, float> CreateTimedEffect()
    {
        return new UpdatableTimedEffect<SpeedBonusEffect, float>(
            new SpeedBonusEffect(target.effectReceiver, GetSpeedBonus()), GetBaseDuration()
        );
    }

    protected override float GetNewModifier()
    {
        return quantity * GetSpeedBonus();
    }
}

public class SpeedBonusAfterHitUpgrade : BaseSpeedBonusAfterHitUpgrade 
{
    public static readonly float BaseDuration = 2f;
    public static readonly float BaseBonus = 2f;

    public SpeedBonusAfterHitUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity) {}

    public override float GetSpeedBonus() => BaseBonus;
    protected override float GetBaseDuration() => BaseDuration;
}

public class HighSpeedBonusAfterHitUpgrade : BaseSpeedBonusAfterHitUpgrade 
{
    public static readonly float BaseDuration = 3f;
    public static readonly float BaseBonus = 4f;

    public HighSpeedBonusAfterHitUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity) {}

    public override float GetSpeedBonus() => BaseBonus;
    protected override float GetBaseDuration() => BaseDuration;
}





public abstract class BaseDamageBonusAfterHit : BaseUpdatableUpgradeAfterEvent<DamageBonusEffect, int> {


    public BaseDamageBonusAfterHit(UpgradeManager target, int quantity)
    : base(target, quantity) 
    {
        SubscribeToOnHitEvent();
    }

    public abstract int GetDamageBonus();


    private void SubscribeToOnHitEvent()
    {
        target.effectReceiver.damageable.onHit.AddListener(
            (object o, Damageable d) => timedEffect.StartCoroutine()
        );
    }

    protected override UpdatableTimedEffect<DamageBonusEffect, int> CreateTimedEffect()
    {
        return new UpdatableTimedEffect<DamageBonusEffect, int>(
            new DamageBonusEffect(target.effectReceiver, GetDamageBonus()), GetBaseDuration()
        );
    }

    protected override int GetNewModifier()
    {
        return quantity * GetDamageBonus();
    }
}


public class DamageBoostAfterHitUpgrade : BaseDamageBonusAfterHit 
{
    public static readonly int BaseBonus = 2;
    public static readonly float BaseDuration = 2f;
 
    public DamageBoostAfterHitUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity) {}

    public override int GetDamageBonus() => BaseBonus;
    protected override float GetBaseDuration() => BaseDuration;
}

