using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Effects.Implementations;
using Effects.Implementations.TimedEffect;
using Effects.Implementations.ChargeRepeatingEffects;
using Effects.Implementations.PersistantEffect;

namespace UniqueStatusEffects.Implementations.UniqueTimedEffects {
    public class BasicSpeedBonusUniqueEffect : BaseTimedUniqueEffect<TimedPositiveSpeedBonusEffect>
    {
        const float speedBonus = 1f;
        
        public BasicSpeedBonusUniqueEffect(UniqueStatusEffectManager target, float totalTime) 
        : base(target, totalTime) {}


        public override TimedPositiveSpeedBonusEffect GetEffect(EffectReceiver target, float duration)
        {
            return new TimedPositiveSpeedBonusEffect(
                new SpeedBonusEffect(target, speedBonus),
                duration,
                Remove
            );
        }
    }
}

