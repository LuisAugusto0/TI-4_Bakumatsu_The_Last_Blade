using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Effects.Implementations.CompositePersistantEffect
{
    public class Escape : BaseEffect, IPersistantEffect {
        public Escape(EffectReceiver target) : base(target) {}
        bool inEffect = false;

        public bool IsActive() => inEffect;

        public override void Start()
        {
            // if immunity was already added, 
            // damageable HashSet will block the consecutive adds
            target.damageable.AddImmunity(this);
            inEffect = true;
        }


        public void End()
        {
            target.damageable.RemoveImmunity(this);
            inEffect = false;
        }
    }

}
