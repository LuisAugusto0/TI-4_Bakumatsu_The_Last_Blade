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

        public void Update(float duration)
        {
            this.duration = duration;
        }

        public void RefreshUpdate(float duration) => Update(duration);
        
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

        public void Update(float duration, int bonus)
        {
            this.duration = duration;
            this.effect.Update(bonus);
        }

        public void RefreshUpdate(float duration, int bonus)
        {
            Update(duration, bonus);
            UpdateRemaningTime(duration);
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

        public void Update(float duration, int bonus)
        {
            this.duration = duration;
            this.effect.Update(bonus);
        }

        public void RefreshUpdate(float duration, int bonus)
        {
            Update(duration, bonus);
            UpdateRemaningTime(duration);
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

        public void Update(float duration, int bonus)
        {
            this.duration = duration;
            this.effect.Update(bonus);
        }

        public void RefreshUpdate(float duration, int bonus)
        {
            Update(duration, bonus);
            UpdateRemaningTime(duration);
        }
    }  





    public class TimedPositiveSpeedMultiplier : TimedEffect<SpeedMultiplierEffect>
    {
        public TimedPositiveSpeedMultiplier(SpeedMultiplierEffect effect, float duration, Action? onEffectEnd) 
        : base(effect, duration, onEffectEnd) 
        {}

        public override Sprite GetIcon()
        {
            throw new NotImplementedException();
        }

        public void Update(float duration, float multiplier)
        {
            this.duration = duration;
            this.effect.Update(multiplier);
        }

        public void RefreshUpdate(float duration, float multiplier)
        {
            Update(duration, multiplier);
            UpdateRemaningTime(duration);
        }
    }  

}