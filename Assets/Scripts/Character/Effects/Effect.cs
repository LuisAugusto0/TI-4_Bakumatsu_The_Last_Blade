using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect {
    readonly public EffectReceiver target;

    protected Effect(EffectReceiver target) 
    {
        this.target = target;
    }

    public abstract void Start();
    public abstract void End();
}


public class ImmunityEffect : Effect {
    public ImmunityEffect(EffectReceiver target) : base(target) {}

    public override void Start()
    {
        target.damageable.AddImmunity(this);
    }

    public override void End()
    {
        target.damageable.RemoveImmunity(this);
    }
}







public abstract class UpdatableEffect<T> : Effect
{

    protected T value;

    protected UpdatableEffect(EffectReceiver target, T value) 
    : base(target)
    {
        this.value = value;
        //Start();
    } 

    public abstract void Update(T param);
    // Update can be handled with different parameters
}

public class SpeedBonusEffect : UpdatableEffect<float> {
    public SpeedBonusEffect(EffectReceiver target, float speedBonus) 
    : base(target, speedBonus) {}
    
    public override void Start() 
    {
        Debug.Log("SPEEDBONUX: " + value);
        target.entityMovement.AddToSpeedBonus(value);
    }

    public override void End() 
    {
        target.entityMovement.AddToSpeedBonus(-value);
    }

    public override void Update(float newSpeedBonus) 
    {
        Debug.Log("SPEEDBONUX: " + value);
        float diff = newSpeedBonus - value;
        target.entityMovement.AddToSpeedBonus(diff);
        this.value += diff;
    }
}

public class SpeedMultiplierEffect : UpdatableEffect<float> {
    public SpeedMultiplierEffect(EffectReceiver target, float speedBonus) 
    : base(target, speedBonus) { }
    
    public override void Start() 
    {
        target.entityMovement.AddToSpeedMultiplier(value);
    }

    public override void End() 
    {
        target.entityMovement.AddToSpeedMultiplier(-value);
    }

    public override void Update(float newSpeedMultiplier) 
    {
        float diff = newSpeedMultiplier - value;
        target.entityMovement.AddToSpeedMultiplier(diff);
        this.value += diff;
    }
}

public class DamageBonusEffect : UpdatableEffect<int> {
    public DamageBonusEffect(EffectReceiver target, int damageBonus) 
    : base(target, damageBonus) { }
    
    public override void Start() 
    {
        target.characterDamage.AddDamageBonus(value);
    }

    public override void End() 
    {
        target.characterDamage.AddDamageBonus(-value);
    }

    public override void Update(int newDamageBonus) 
    {
        int diff = newDamageBonus - value;
        target.characterDamage.AddDamageBonus(diff);
        this.value += diff;
    }
}

public class DamageMultiplierEffect : UpdatableEffect<float> {
    public DamageMultiplierEffect(EffectReceiver target, float damageMultiplier) 
    : base(target, damageMultiplier) { }
    
    public override void Start() 
    {
        target.characterDamage.AddDamageMultiplier(value);
    }

    public override void End()
     {
        target.characterDamage.AddDamageMultiplier(-value);
    }

    public override void Update(float newDamageMultiplier) 
    {
        float diff = newDamageMultiplier - value;
        target.characterDamage.AddDamageMultiplier(diff);
        this.value += diff;
    }
}

public class HealthBonusEffect : UpdatableEffect<int> {
    public HealthBonusEffect(EffectReceiver target, int healthBonus) 
    : base(target, healthBonus) {}
    

    public override void Start() 
    {
        target.damageable.IncreaseBaseHealthBonus(value);
    }

    public override void End() 
    {
        target.damageable.IncreaseBaseHealthBonus(-value);
    }

    public override void Update(int newHealthBonus) 
    {
        int diff = newHealthBonus - value;
        target.damageable.IncreaseBaseHealthBonus(diff);
        this.value += diff;
    }
}

// Instantion methods such as healing ring can also be added here


