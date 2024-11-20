using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;
#nullable enable


public abstract class BasePermanentUpgrade<TEffect> : Upgrade
    where TEffect : BaseEffect, IPersistantEffect
{ 
    protected TEffect effect;

    public BasePermanentUpgrade(UpgradeManager target, int quantity)
    : base(target, quantity) 
    { 
        this.effect = GetEffect();
        effect.Start();
    }
    
    protected abstract TEffect GetEffect();
    protected abstract void Update();

    protected override void UpdateQuantity(int quantity)
    {
        base.UpdateQuantity(quantity);
        Update();
    }

    
    public override void Remove() {
        effect.End();
    }
}


