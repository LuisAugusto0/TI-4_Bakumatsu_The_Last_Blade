using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public abstract class BasePermanentUpdatableUpgrade<TEffect, TModifier> : Upgrade
    where TEffect : UpdatableEffect<TModifier>
{ 
    protected TEffect effect;

    public BasePermanentUpdatableUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity) 
    { }

    // Ensure its called after subclasses constructor
    protected void Initialize()
    {
        this.effect = GetEffect();
        effect.Start();
    }

    protected abstract TEffect GetEffect();
    
    public override void Remove() {
        effect.End();
    }
}



// Speed bonus
public abstract class BaseSpeedBonusStatBoost : BasePermanentUpdatableUpgrade<SpeedBonusEffect, float> {
    float baseBonus;
    public BaseSpeedBonusStatBoost(UpgradeManager target, int quantity, float bonus)
    : base(target, quantity) 
    {
        this.baseBonus = bonus;
        Initialize();
    }

    protected override SpeedBonusEffect GetEffect()
    {
        Debug.Log("HERE! quantity: " + quantity);
        return new SpeedBonusEffect(target.effectReceiver, quantity * baseBonus);
    }

    protected override void UpdateQuantity(int quantity)
    {
        base.UpdateQuantity(quantity);
        float newBonus = quantity * baseBonus;
        effect.Update(newBonus);
        
    }


}


public class SpeedBonusUpgrade : BaseSpeedBonusStatBoost {
    public static readonly float bonus = 1f;

    public SpeedBonusUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity, bonus) {}
}

public class HighSpeedBonusUpgrade : BaseSpeedBonusStatBoost {
    public static readonly float bonus = 2f;

    public HighSpeedBonusUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity, bonus) {}
}




// Speed multiplier
public abstract class BaseSpeedMultiplierStatBoost : BasePermanentUpdatableUpgrade<SpeedMultiplierEffect, float> {
    float baseMultiplier;
    public BaseSpeedMultiplierStatBoost(UpgradeManager target, int quantity, float multiplier)
    : base(target, quantity) 
    {
        this.baseMultiplier = multiplier;
        Initialize();
    }

    protected override SpeedMultiplierEffect GetEffect()
    {
        return new SpeedMultiplierEffect(target.effectReceiver, quantity * baseMultiplier);
    }

    protected override void UpdateQuantity(int quantity)
    {
        base.UpdateQuantity(quantity);
        float newMultiplier = quantity * baseMultiplier;
        effect.Update(newMultiplier);
    }
}


public class SpeedMultiplierUpgrade : BaseSpeedMultiplierStatBoost {
    public static readonly float multiplier = 0.5f;

    public SpeedMultiplierUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity, multiplier) {}
}

public class DoubleSpeedUpgrade : BaseSpeedMultiplierStatBoost {
    public static readonly float multiplier = 1f;

    public DoubleSpeedUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity, multiplier) {}
}




// Damage Bonus
public abstract class BaseDamageBonusStatBoost : BasePermanentUpdatableUpgrade<DamageBonusEffect, int> {
    int baseBonus;
    public BaseDamageBonusStatBoost(UpgradeManager target, int quantity, int bonus)
    : base(target, quantity) 
    {
        this.baseBonus = bonus;
        Initialize();
    }

    protected override DamageBonusEffect GetEffect()
    {
        return new DamageBonusEffect(target.effectReceiver, quantity * baseBonus);
    }

    protected override void UpdateQuantity(int quantity)
    {
        base.UpdateQuantity(quantity);
        int newBonus = quantity * baseBonus;
        effect.Update(newBonus);
    }
}


public class DamageBonusUpgrade : BaseDamageBonusStatBoost {
    public static readonly int bonus = 1;

    public DamageBonusUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity, bonus) {}
}

public class HighDamageBonusUpgrade : BaseDamageBonusStatBoost {
    public static readonly int bonus = 2;

    public HighDamageBonusUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity, bonus) {}
}



//Damage multiplier
public abstract class BaseDamageMultiplierStatBonus : BasePermanentUpdatableUpgrade<DamageMultiplierEffect, float> {
    float baseMultiplier;
    
    public BaseDamageMultiplierStatBonus(UpgradeManager target, int quantity, float multiplier)
    : base(target, quantity) 
    {
        this.baseMultiplier = multiplier;
        Initialize();
    }
        
    protected override DamageMultiplierEffect GetEffect()
    {
        return new DamageMultiplierEffect(target.effectReceiver, quantity * baseMultiplier);
    }

    protected override void UpdateQuantity(int quantity)
    {
        
        base.UpdateQuantity(quantity);
        float newMultiplier = quantity * baseMultiplier;
        effect.Update(newMultiplier);

    }
}


public class DoubleDamageUpgrade : BaseDamageMultiplierStatBonus {
    public static readonly float bonus = 1f;

    public DoubleDamageUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity, bonus) {}
}


//Health bonus
public abstract class BaseHealthBonusUpgrade : BasePermanentUpdatableUpgrade<HealthBonusEffect, int> 
{
    int baseBonus;
    
    public BaseHealthBonusUpgrade(UpgradeManager target, int quantity, int bonus)
    : base(target, quantity) 
    {
        this.baseBonus = bonus;
        Initialize();
    }

    protected override HealthBonusEffect GetEffect()
    {
        return new HealthBonusEffect(target.effectReceiver, quantity * baseBonus);
    }

    protected override void UpdateQuantity(int quantity)
    {
        
        base.UpdateQuantity(quantity);
        int newBonus = quantity * baseBonus;
        effect.Update(newBonus);

    }
}

public class SingleHealthBonusUpgrade : BaseHealthBonusUpgrade 
{
    public static readonly int bonus = 1;
    
    public SingleHealthBonusUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity, bonus) {}


}