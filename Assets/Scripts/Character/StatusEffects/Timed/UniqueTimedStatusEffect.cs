using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UniqueTimedStatusEffect : TimedStatusEffect
{
    public UniqueTimedStatusEffect(StatusEffectManager target, float duration) 
        : base (target, duration)
    {}

    public override void Remove()
    {
        target.uniqueTimedEffects.Remove(this.GetType());
        // Expand End() to add effects
    }

}


// -- Implementations --

namespace UniqueTimed
{
    public class Immunity : TimedStatusEffect
    {
        public Immunity(StatusEffectManager target, float duration) 
            : base(target, duration) 
        {
            target.damageable.AddImmunity(this);
        }

        public override void Remove()
        {
            base.Remove();
            target.damageable.RemoveImmunity(this);
            
        }

        protected override void RemoveSelfFromStructure()
        {
            target.uniqueTimedEffects.Remove(this.GetType());
        }
    }


    public class DoubleDamage : TimedEffects.AddToDamageMultiplier
    {
        public DoubleDamage(StatusEffectManager target, float duration) 
            : base(target, duration, 1) 
        {
            target.damageable.AddImmunity(this);
        }

        protected override void RemoveSelfFromStructure()
        {
            target.uniqueTimedEffects.Remove(this.GetType());
        }
    }

}
