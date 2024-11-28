using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable


namespace Effects.Implementations.PersistantEffect
{
    public class ImmunityEffect : BaseEffect, IPersistantEffect {
        public ImmunityEffect(EffectReceiver target) : base(target) {}
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



    public class FixedSpeedBonusEffect : BaseEffect, IPersistantEffect {
        float speedBonus;

        public FixedSpeedBonusEffect(EffectReceiver target, float speedBonus) 
        : base(target) 
        {
            this.speedBonus = speedBonus;
        }

        bool inEffect = false;

        public bool IsActive() => inEffect;

        public override void Start() 
        {
            if (inEffect == false)
            {
                Debug.Log("HERE!!");
                target.entityMovement.AddToSpeedBonus(speedBonus);
                inEffect = true;
            }
        }

        public void End() 
        {
            target.entityMovement.AddToSpeedBonus(-speedBonus);
            inEffect = false;
        }

        public void Update(float newSpeedBonus) 
        {
            float diff = newSpeedBonus - speedBonus;
            target.entityMovement.AddToSpeedBonus(diff);
            this.speedBonus += diff;
        }
    }


    public class SpeedMultiplierEffect : BaseEffect, IPersistantEffect {
        float speedMultiplier;
        public SpeedMultiplierEffect(EffectReceiver target, float speedMultiplier) 
        : base(target) 
        { 
            this.speedMultiplier = speedMultiplier;
        }

        bool inEffect = false;

        public bool IsActive() => inEffect;

        public override void Start() 
        {
            if (inEffect == false)
            {
                target.entityMovement.AddSpeedMultiplier(speedMultiplier);
                inEffect = true;
            }
        }

        public void End() 
        {
            target.entityMovement.RemoveSpeedMultiplier(speedMultiplier);
            inEffect = false;
        }

        public void Update(float newSpeedMultiplier) 
        {
            target.entityMovement.RemoveSpeedMultiplierAndNotUpdate(speedMultiplier);
            this.speedMultiplier = newSpeedMultiplier;
            target.entityMovement.AddSpeedMultiplier(speedMultiplier);
        }
    }

    public class FixedDamageBonusEffect : BaseEffect, IPersistantEffect {
        int damageBonus;
        public FixedDamageBonusEffect(EffectReceiver target, int damageBonus) 
        : base(target) 
        { 
            this.damageBonus = damageBonus;
        }

        bool inEffect = false;

        public bool IsActive() => inEffect;

        public override void Start() 
        {
            if (inEffect == false)
            {
                target.characterDamage.AddDamageBonus(damageBonus);
                inEffect = true;
            }
        }

        public void End() 
        {
            target.characterDamage.AddDamageBonus(-damageBonus);
            inEffect = false;
        }

        public void Update(int newDamageBonus) 
        {
            int diff = newDamageBonus - damageBonus;
            target.characterDamage.AddDamageBonus(diff);
            this.damageBonus += diff;
        }
    }

    public class DamageMultiplierEffect : BaseEffect, IPersistantEffect {
        float damageMultiplier;
        public DamageMultiplierEffect(EffectReceiver target, float damageMultiplier) 
        : base(target) 
        {
            this.damageMultiplier = damageMultiplier;
        }

        bool inEffect = false;

        public bool IsActive() => inEffect;

        public override void Start() 
        {
            if (inEffect == false)
            {
                target.characterDamage.AddDamageMultiplier(damageMultiplier);
                inEffect = true;
            }
            
        }

        public void End()
        {
            target.characterDamage.RemoveDamageMultiplier(damageMultiplier);
            inEffect = false;
        }

        public void Update(float newDamageMultiplier) 
        {
            target.characterDamage.RemoveDamageMultiplierAndNotUpdate(damageMultiplier);
            this.damageMultiplier = newDamageMultiplier;
            target.characterDamage.AddDamageMultiplier(damageMultiplier);
        }
    } 

    public class FixedHealthBonusEffect : BaseEffect, IPersistantEffect {
        int healthBonus;
        public FixedHealthBonusEffect(EffectReceiver target, int healthBonus) 
        : base(target) 
        {
            this.healthBonus = healthBonus;
        }

        bool inEffect = false;

        public bool IsActive() => inEffect;

        public override void Start() 
        {
            if (inEffect == false)
            {
                target.damageable.IncreaseBaseHealthBonus(healthBonus);
                inEffect = true;
            }

        }

        public void End() 
        {
            target.damageable.IncreaseBaseHealthBonus(-healthBonus);
            inEffect = false;
        }

        public void Update(int newHealthBonus) 
        {
            int diff = newHealthBonus - healthBonus;
            target.damageable.IncreaseBaseHealthBonus(diff);
            this.healthBonus += diff;
        }
    }
}