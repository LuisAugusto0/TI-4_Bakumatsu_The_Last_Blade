using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class StackableTimedStatusEffect : TimedStatusEffect
{
    public StackableTimedStatusEffect(StatusEffectManager target, float duration) 
        : base (target, duration)
    {}

    public override void Remove()
    {
        target.stackableEffects.Remove(this);
        // Expand End() to add effects
    }
}



// -- Implementations -- 

namespace StackableTimedEffect
{

    public class DoubleSpeed : TimedEffects.AddToSpeedMultiplier
    {
        public DoubleSpeed(StatusEffectManager target, float duration) 
            : base(target, duration, 1) 
        { }

        protected override void RemoveSelfFromStructure()
        {
            target.stackableEffects.Remove(this);
        }
    }

    public class SimpleSpeedBoost : TimedEffects.AddToSpeedBonus
    {
        public static readonly float boost = 1;
        public SimpleSpeedBoost(StatusEffectManager target, float duration) 
            : base(target, duration, boost) 
        { }

        protected override void RemoveSelfFromStructure()
        {
            target.stackableEffects.Remove(this);
        }
    }

    public class DoubleDamage : TimedEffects.AddToDamageMultiplier
    {
        public DoubleDamage(StatusEffectManager target, float duration) 
            : base(target, duration, 1) 
        { }

        protected override void RemoveSelfFromStructure()
        {
            target.stackableEffects.Remove(this);
        }
    }

    public class SimpleDamageBoost : TimedEffects.AddToDamageBonus
    {
        public static readonly int boost = 1;
        public SimpleDamageBoost(StatusEffectManager target, float duration) 
            : base(target, duration, boost) 
        { }

        protected override void RemoveSelfFromStructure()
        {
            target.stackableEffects.Remove(this);
        }
    }

    public class TestDamageOverTime : TimedEffects.TakeDamageOverInterval
    {
        public static readonly int damage = 1;
        public static readonly float interval = 0.5f;
        public TestDamageOverTime(StatusEffectManager target, float duration) 
            : base(target, duration, damage, interval) 
        { }

        protected override void RemoveSelfFromStructure()
        {
            target.stackableEffects.Remove(this);
        }
    }

}
