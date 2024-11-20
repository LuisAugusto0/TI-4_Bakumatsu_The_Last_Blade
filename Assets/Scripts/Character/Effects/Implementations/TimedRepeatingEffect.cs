using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
#nullable enable

using Effects.Implementations.TimedEffect;
using Effects.Implementations.PersistantEffect;



namespace Effects.Implementations.TimedRepeatingEffect 
{
    abstract class TimedRepeatingSpeedBonusEffect : BaseTimedRepeatingEffect<TimedEffect<SpeedBonusEffect>> 
    {
        public TimedRepeatingSpeedBonusEffect(
            TimedEffect<SpeedBonusEffect> effect, float totalDuration, 
            float interval, Action? onEffectEnd
        )
        : base(effect, totalDuration, interval, onEffectEnd) {}
    }

    class SlidingShoes : TimedRepeatingSpeedBonusEffect
    {
        const float speedBonus = 2f;
        const float duration = 0.5f;
        
        public SlidingShoes(
            EffectReceiver target, float totalDuration, 
            float interval, Action? onEffectEnd
        )
        : base(GetEffect(target), totalDuration, interval, onEffectEnd) {}

        public static TimedPositiveSpeedBonusEffect GetEffect(EffectReceiver target)
        {
            return new TimedPositiveSpeedBonusEffect(
                new SpeedBonusEffect(target, speedBonus), duration, null
            );
        }

        public override Sprite GetIcon()
        {
            throw new NotImplementedException();
        }
    }
}