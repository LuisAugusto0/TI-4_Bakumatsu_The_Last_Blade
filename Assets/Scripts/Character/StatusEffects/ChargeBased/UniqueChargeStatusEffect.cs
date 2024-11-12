using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public abstract class UniqueChargeStatusEffect : ChargeStatusEffect
{
    public UniqueChargeStatusEffect(StatusEffectManager target, int charges)
    : base(target, charges)
    {}

    protected override void RemoveSelfFromStructure()
    {
        throw new NotImplementedException();
    }
}

namespace UniqueCharge
{
    [Obsolete]
    public class Test : UniqueChargeStatusEffect
    {
        Coroutine timeCoroutine;
        public static readonly float interval = 1.5f;

        public Test(StatusEffectManager target, int charges)
        : base(target, charges)
        {
            timeCoroutine = target.StartCoroutine(Timer());
        }


        void Execute()
        {
            if (charges <= 0)
            {
                Remove();
                return;
            }
            
            charges--;
            Debug.Log(charges);
            timeCoroutine = target.StartCoroutine(Timer());
        }

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(interval);
            Execute();
        }
    }
}
