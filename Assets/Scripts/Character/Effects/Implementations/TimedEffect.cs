using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

using Effects.Implementations.PersistantEffect;

namespace Effects.Implementations.TimedEffect
{
    public class TimedImmunityEffect : TimedEffect<ImmunityEffect>
    {
        public TimedImmunityEffect(ImmunityEffect effect, float duration, Action? onEffectEnd)
        : base(effect, duration, onEffectEnd) 
        {}

        public override Sprite GetIcon()
        {
            throw new NotImplementedException();
        }
    }




    public class TimedPositiveSpeedBonusEffect : TimedEffect<SpeedBonusEffect>
    {
        public TimedPositiveSpeedBonusEffect(SpeedBonusEffect effect, float duration, Action? onEffectEnd) 
        : base(effect, duration, onEffectEnd) 
        {}

        public override Sprite GetIcon()
        {
            throw new NotImplementedException();
        }
    }  

    public class TimedPositiveDamageBonusEffect : TimedEffect<DamageBonusEffect>
    {
        public TimedPositiveDamageBonusEffect(DamageBonusEffect effect, float duration, Action? onEffectEnd) 
        : base(effect, duration, onEffectEnd) 
        {}

        public override Sprite GetIcon()
        {
            throw new NotImplementedException();
        }
    }  

    public class TimedHealthBonusEffect : TimedEffect<HealthBonusEffect>
    {
        public TimedHealthBonusEffect(HealthBonusEffect effect, float duration, Action? onEffectEnd) 
        : base(effect, duration, onEffectEnd) 
        {}

        public override Sprite GetIcon()
        {
            throw new NotImplementedException();
        }
    }  





    public class TimedDoubleSpeedEffect : TimedEffect<SpeedMultiplierEffect>
    {
        public TimedDoubleSpeedEffect(EffectReceiver target, float duration, Action? onEffectEnd) 
        : base(GetEffect(target), duration, onEffectEnd) 
        {}

        public static SpeedMultiplierEffect GetEffect(EffectReceiver target)
        {
            return new SpeedMultiplierEffect(target, 1f);
        }

        public override Sprite GetIcon()
        {
            throw new NotImplementedException();
        }
    }  

}