using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#nullable enable

using Effects.Implementations.InstantaneousEffect;


namespace Effects.Implementations.ChargeRepeatingEffects
{
    public abstract class BaseChargeRepeatingDamageEffect : BaseChargeRepeatingEffect<TakeDamageEffect>
    {
        public BaseChargeRepeatingDamageEffect(
            TakeDamageEffect effect, 
            int totalCharges, 
            int chargeConsumeRate, 
            float interval, 
            Action? onEffectEnd
        )
        : base(effect, totalCharges, chargeConsumeRate, interval, onEffectEnd) {}
    }


    public class ChargeRepeatingBurningEffect : BaseChargeRepeatingDamageEffect
    {
        const int damage = 1;
        const float interval = 1f;
        const int chargeConsumeRate = 1;

        public ChargeRepeatingBurningEffect(EffectReceiver target, int totalCharges, Action? onEffectEnd)
        : base(GetEffect(target), totalCharges, chargeConsumeRate, interval, onEffectEnd) {}

        public static TakeDamageEffect GetEffect(EffectReceiver target)
        {
            return new TakeDamageEffect(target, damage);
        }

        public override Sprite GetIcon()
        {
            throw new NotImplementedException();
        }
    }
}