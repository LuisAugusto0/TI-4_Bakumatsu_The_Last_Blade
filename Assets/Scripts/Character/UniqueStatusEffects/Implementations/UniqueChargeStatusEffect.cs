using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Effects.Implementations.TimedEffect;
using Effects.Implementations.ChargeRepeatingEffects;

namespace UniqueStatusEffects.Implementations.UniqueChargeEffect {
    public class BurningUniqueEffect : BaseChargeUniqueEffect<ChargeRepeatingBurningEffect>
    {
        public BurningUniqueEffect(UniqueStatusEffectManager target, int baseCharges) 
        : base(target, baseCharges) {}

        public override ChargeRepeatingBurningEffect GetEffect(EffectReceiver target, int charges)
        {
            return new ChargeRepeatingBurningEffect(
                target,
                charges,
                Remove
            );
        }
    }
}

