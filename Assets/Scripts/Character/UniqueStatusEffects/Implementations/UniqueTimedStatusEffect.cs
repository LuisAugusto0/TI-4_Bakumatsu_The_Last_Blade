using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Effects.Implementations;
using Effects.Implementations.TimedEffect;
using Effects.Implementations.ChargeRepeatingEffects;
using Effects.Implementations.PersistantEffect;

namespace UniqueStatusEffects.Implementations.UniqueTimedEffects {
    public class BasicSpeedBonusUniqueEffect : BaseTimedUniqueEffect<TimedPositiveFixedSpeedBonusEffect>
    {
        const float speedBonus = 1f;
        
        public BasicSpeedBonusUniqueEffect(UniqueStatusEffectManager target, float totalTime) 
        : base(target, totalTime) {}


        public override TimedPositiveFixedSpeedBonusEffect GetEffect(EffectReceiver target, float duration)
        {
            return new TimedPositiveFixedSpeedBonusEffect(
                new FixedSpeedBonusEffect(target, speedBonus),
                duration,
                Remove
            );
        }
    }
}

