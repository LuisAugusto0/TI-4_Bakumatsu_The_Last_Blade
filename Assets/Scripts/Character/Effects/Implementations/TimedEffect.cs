using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

using Effects.Implementations.PersistantEffect;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Effects.Implementations.TimedEffect
{
    public class TimedImmunityEffect : TimedEffect<ImmunityEffect>
    {
        public TimedImmunityEffect(ImmunityEffect effect, float duration, Action? onEffectEnd)
        : base(effect, duration, onEffectEnd) 
        {}
        
        static Sprite? icon;    
        public static Sprite? GetStaticIcon() => icon;
        public override Sprite? GetIcon() => icon;

        public static void LoadIcon(Sprite sprite) => icon = sprite;
        public static void UnloadIcon() => icon = null;



        public void Update(float duration)
        {
            this.duration = duration;
        }

        public void RefreshUpdate(float duration) => Update(duration);
        
    }

    [Obsolete("Icon not implemented")]
    public class TimedPositiveFixedSpeedBonusEffect : TimedEffect<FixedSpeedBonusEffect>
    {
        public TimedPositiveFixedSpeedBonusEffect(FixedSpeedBonusEffect effect, float duration, Action? onEffectEnd) 
        : base(effect, duration, onEffectEnd) 
        {}

        static Sprite? icon;    
        public static Sprite? GetStaticIcon() => icon;
        public override Sprite? GetIcon() => icon;

        public static void LoadIcon(Sprite sprite) => icon = sprite;
        public static void UnloadIcon() => icon = null;


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

    
    public class TimedPositiveFixedDamageBonusEffect : TimedEffect<FixedDamageBonusEffect>
    {
        public TimedPositiveFixedDamageBonusEffect(FixedDamageBonusEffect effect, float duration, Action? onEffectEnd) 
        : base(effect, duration, onEffectEnd) 
        {}

        static Sprite? icon;    
        public static Sprite? GetStaticIcon() => icon;
        public override Sprite? GetIcon() => icon;

        public static void LoadIcon(Sprite sprite) => icon = sprite;
        public static void UnloadIcon() => icon = null;


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

    [Obsolete("Icon not implemented")]
    public class TimedFixedHealthBonusEffect : TimedEffect<FixedHealthBonusEffect>
    {
        public TimedFixedHealthBonusEffect(FixedHealthBonusEffect effect, float duration, Action? onEffectEnd) 
        : base(effect, duration, onEffectEnd) 
        {}

        static Sprite? icon;          
        public static Sprite? GetStaticIcon() => icon;
        public override Sprite? GetIcon() => icon;

        public static void LoadIcon(Sprite sprite) => icon = sprite;
        public static void UnloadIcon() => icon = null;
        



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

        static Sprite? icon;    
        public static Sprite? GetStaticIcon() => icon;
        public override Sprite? GetIcon() => icon;

        public static void LoadIcon(Sprite sprite) => icon = sprite;
        public static void UnloadIcon() => icon = null;


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