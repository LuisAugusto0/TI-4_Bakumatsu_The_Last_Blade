using System.Collections;
using System.Collections.Generic;
using Effects.Implementations.PersistantEffect;
using UnityEngine;


namespace Effects.Implementations.CompositePersistantEffect
{
    public class PairEffect<TEffect1, TEffect2> : IPersistantEffect
    where TEffect1 : BaseEffect, IPersistantEffect
    where TEffect2 : BaseEffect, IPersistantEffect
    {
        bool inEffect = false;
        public EffectReceiver target;
        public TEffect1 first;
        public TEffect2 second;
        

        public PairEffect(TEffect1 first, TEffect2 second)
        {
            this.first = first;
            this.second = second;
            
            if (first.GetTarget() != second.GetTarget())
            {
                Debug.LogError("Mismatch between effect parents");
            }
            this.target = first.GetTarget();
        }
    
        public EffectReceiver GetTarget() => target;
        public bool IsActive() => inEffect;
        
        public void Start()
        {
            inEffect = true;
            first.Start();
            second.Start();
        }

        public void End()
        {
            inEffect = false;
            first.End();
            second.End();
        }
    }

}
