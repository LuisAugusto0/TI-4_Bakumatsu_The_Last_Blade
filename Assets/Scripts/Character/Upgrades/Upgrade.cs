using System.Collections;
using UnityEditor;
using UnityEngine;

public abstract class Upgrade
{
    public int Quantity { get {return quantity;}}
    protected int quantity = 1;
    protected BaseUpgradeManager entity;

    public Upgrade(BaseUpgradeManager entity, int quantity)
    {
        this.entity = entity;
        this.quantity = quantity;
        // Expand constructor to add permanent changes that does not 
        // change on each update (example: adding listeners)
    }

    protected virtual void UpdateEffect(int quantity)
    {
        this.quantity = quantity;
        // Expand to change effect after quantity is changed
    }

    public void AddToEffect(int value)
    {
        int newQuantity = this.quantity + value;
        if (newQuantity <= 0)
        {
            Remove();
        }
        else
        {
            UpdateEffect(this.quantity + value);
        }
    }

    // Call when the effect is cleared
    public virtual void Remove()
    {
        entity.RemoveUpgradeInstance(this.GetType());
        // Expand End() to reset additional changes
    }
}



public class SpeedBoost : Upgrade
{
    public readonly float speedBonus = 1f;
    public float appliedSpeed = 0;

    public SpeedBoost(BaseUpgradeManager entity, int quantity) 
        : base(entity, quantity) 
    {
        Apply();   
    }

    public override void Remove()
    {
        base.Remove();
        ResetBonus();
    }


    void Apply()
    {
        ResetBonus();
        appliedSpeed = speedBonus * quantity;
        entity.entityMovement.AddToSpeedBonus(appliedSpeed);
    }

    protected override void UpdateEffect(int quantity)
    {
        base.UpdateEffect(quantity);
        Apply();
    }

    void ResetBonus()
    {
        entity.entityMovement.AddToSpeedBonus(-(appliedSpeed));
        appliedSpeed = 0;
    }
}

public class SpeedBoostAfterHit : Upgrade
{
    public readonly float speedBonus = 5f;
    public readonly float duration = 2f;
    Coroutine activeCoroutine = null;
    float currentBonus = 0f;

    public SpeedBoostAfterHit(BaseUpgradeManager entity, int quantity) 
        : base(entity, quantity)
    {
        entity.damageable.onHit.AddListener(OnHit);
    }

    public override void Remove()
    {
        base.Remove();
        StopCurrentEffect();
    }



    void OnHit(object source, Damageable self)
    {
        StopCurrentEffect();
        activeCoroutine = entity.StartCoroutine(Routine());
    }

    IEnumerator Routine()
    {
        currentBonus = speedBonus * quantity;
        entity.entityMovement.AddToSpeedBonus(currentBonus);
        yield return new WaitForSeconds(duration);
        SetBonusBack();

    }
    
    void StopCurrentEffect()
    {
        if (activeCoroutine != null)
        {
            entity.StopCoroutine(activeCoroutine);
        }
        SetBonusBack();
    }

    void SetBonusBack()
    {
        entity.entityMovement.AddToSpeedBonus(-currentBonus);
        currentBonus = 0;
    }


}

public class DamageBoostAfterHit : Upgrade
{
    public readonly int damageBonus = 1;
    public readonly float duration = 5f;
    Coroutine activeCoroutine = null;
    int currentBonus = 0;

    public DamageBoostAfterHit(BaseUpgradeManager entity, int quantity) 
        : base(entity, quantity)
    {
        entity.damageable.onHit.AddListener(OnHit);
    }

    public override void Remove()
    {
        base.Remove();
        StopCurrentEffect();
    }



    void OnHit(object source, Damageable self)
    {
        StopCurrentEffect();
        activeCoroutine = entity.StartCoroutine(Routine());
    }

    IEnumerator Routine()
    {
        currentBonus = damageBonus * quantity;
        entity.characterDamage.AddDamageBonus(currentBonus);
        yield return new WaitForSeconds(duration);
        SetBonusBack();

    }
    

    void StopCurrentEffect()
    {
        if (activeCoroutine != null)
        {
            entity.StopCoroutine(activeCoroutine);
        }
        SetBonusBack();
    }

    void SetBonusBack()
    {
        entity.characterDamage.AddDamageBonus(-currentBonus);
        currentBonus = 0;
    }


}

public class HealthBoost : Upgrade
{
    public readonly int hpBonus = 1;
    int appliedBonus = 0;

    public HealthBoost(BaseUpgradeManager entity, int quantity) 
        : base(entity, quantity) 
    {
        Apply();   
    }


    public override void Remove()
    {
        base.Remove();
        ResetBonus();
    }



    void Apply()
    {
        ResetBonus();
        appliedBonus = hpBonus * quantity;
        entity.damageable.IncreaseBaseHealthBonus(appliedBonus);
    }

    protected override void UpdateEffect(int quantity)
    {
        base.UpdateEffect(quantity);
        Apply();
    }

    void ResetBonus()
    {
        entity.damageable.IncreaseBaseHealthBonus(-(appliedBonus));
        appliedBonus = 0;
    }
}

// -- OP --
public class DamageBoost : Upgrade
{
    public readonly int attackBonus = 1;
    public int appliedBonus = 0;

    public DamageBoost(BaseUpgradeManager entity, int quantity) 
        : base(entity, quantity) 
    {
        Apply();   
    }

    public override void Remove()
    {
        base.Remove();
        ResetBonus();
    }


    void Apply()
    {
        ResetBonus();
        appliedBonus = attackBonus * quantity;
        entity.characterDamage.AddDamageBonus(appliedBonus);
    }

    protected override void UpdateEffect(int quantity)
    {
        base.UpdateEffect(quantity);
        Apply();
    }

    void ResetBonus()
    {
        entity.characterDamage.AddDamageBonus(-(appliedBonus));
        appliedBonus = 0;
    }
}


// -- EXTRA OP --
// Maybe add levels of upgrade later?
public class DoubleSpeed : Upgrade
{
    public readonly float speedMultiplier = 1f; //double
    public float appliedMultiplier = 0;

    public DoubleSpeed(BaseUpgradeManager entity, int quantity) 
        : base(entity, quantity) 
    {
        Apply();   
    }


    public override void Remove()
    {
        base.Remove();
        ResetMultiplier();
    }


    void Apply()
    {
        ResetMultiplier();
        appliedMultiplier = speedMultiplier * quantity;
        entity.entityMovement.AddToSpeedMultiplier(appliedMultiplier);
    }

    protected override void UpdateEffect(int quantity)
    {
        base.UpdateEffect(quantity);
        Apply();
    }

    void ResetMultiplier()
    {
        entity.entityMovement.AddToSpeedMultiplier(-(appliedMultiplier));
        appliedMultiplier = 0;
    }
}

public class DoubleDamage : Upgrade
{
    public readonly float damageMultiplier = 1f; //double
    public float appliedMultiplier = 0;

    public DoubleDamage(BaseUpgradeManager entity, int quantity) 
        : base(entity, quantity) 
    {
        Apply();   
    }


    public override void Remove()
    {
        base.Remove();
        ResetMultiplier();
    }


    void Apply()
    {
        ResetMultiplier();
        appliedMultiplier = damageMultiplier * quantity;
        entity.characterDamage.AddDamageMultiplier(appliedMultiplier);
    }

    protected override void UpdateEffect(int quantity)
    {
        base.UpdateEffect(quantity);
        Apply();
    }

    void ResetMultiplier()
    {
        entity.characterDamage.AddDamageMultiplier(-(appliedMultiplier));
        appliedMultiplier = 0;
    }
}

