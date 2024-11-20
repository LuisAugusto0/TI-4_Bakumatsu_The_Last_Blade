using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effects.Implementations.InstantaneousEffect
{
    public class TakeDamageEffect : BaseEffect, INonPersistantEffect
    {
        int damage;

        public TakeDamageEffect(EffectReceiver target, int dmg) 
        : base(target) 
        { 
            this.damage = dmg;
        }

        public override void Start() 
        {
            target.damageable.TakeDamage(this, 1);
        }

        public void Update(int newDmg) 
        {
            this.damage = newDmg;
        }
    }

}

