using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
#nullable enable

using Effects.Implementations.TimedEffect;
using Effects.Implementations.PersistantEffect;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;



namespace Effects.Implementations.TimedRepeatingEffect 
{
    abstract class TimedRepeatingSpeedMultiplierEffect : BaseTimedRepeatingEffect<TimedEffect<SpeedMultiplierEffect>> 
    {
        public TimedRepeatingSpeedMultiplierEffect(
            TimedEffect<SpeedMultiplierEffect> effect, float totalDuration, 
            float interval, Action? onEffectEnd
        )
        : base(effect, totalDuration, interval, onEffectEnd) {}
    }

    
    [Obsolete("Icon not implemented")]
    class SlidingShoes : TimedRepeatingSpeedMultiplierEffect
    {
        const float speedMultiplier = 1.5f;
        const float duration = 0.5f;
        
        static Sprite? icon;
        public static Sprite? GetStaticIcon() => icon;
        public override Sprite? GetIcon() => icon;

        public static void LoadIcon(Sprite sprite) => icon = sprite;
        public static void UnloadIcon() => icon = null;
        

        public SlidingShoes(
            EffectReceiver target, float totalDuration, 
            float interval, Action? onEffectEnd
        )
        : base(GetEffect(target), totalDuration, interval, onEffectEnd) {}

        public static TimedPositiveSpeedMultiplier GetEffect(EffectReceiver target)
        {
            return new TimedPositiveSpeedMultiplier(
                new SpeedMultiplierEffect(target, speedMultiplier), duration, null
            );
        }
    }
}