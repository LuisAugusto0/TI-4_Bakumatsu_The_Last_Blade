using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public abstract class StackableChargeStatusEffect : ChargeStatusEffect
{
    public StackableChargeStatusEffect(StatusEffectManager target, int charges)
        : base(target, charges)
    {}
    public override void Remove()
    {
        throw new System.NotImplementedException();
    }
}
