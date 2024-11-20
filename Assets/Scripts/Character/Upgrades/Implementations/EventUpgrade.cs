using Effects.Implementations.PersistantEffect;
using Effects.Implementations.TimedEffect;
using UnityEngine;
using UnityEngine.Events;
#nullable enable

namespace Upgrades.Implementations.EventUpgrade
{
    /* Description:
    *   Makes target invincible for (2 * quantity) seconds after hit
    *
    * Growth type: Linear
    * Rarity: ? 
    */
    public class ImmunityAfterHitUpgrade : BaseUpgradeAfterEvent<TimedImmunityEffect>
    {
        const float baseDuration = 2f;

        public ImmunityAfterHitUpgrade(UpgradeManager target, int quantity)
        : base(target, quantity){}

        protected override TimedImmunityEffect GetEffect()
        {
            return new TimedImmunityEffect(
                new ImmunityEffect(target.effectReceiver), baseDuration, null
            );
        }


        protected override void Update()
        {
            float newDuration = quantity * baseDuration;
            effect.UpdateDuration(newDuration);
        }



        UnityAction<object, Damageable>? onHitListener;
        protected override void SubscribeToEvent()
        {
            onHitListener = (object o, Damageable d) => effect.Start();
            target.damageable.onHit.AddListener(onHitListener);
        }

        protected override void UnsubscribeToEvent()
        {
            if (onHitListener != null)
            {
                target.damageable.onHit.RemoveListener(onHitListener);
            }
        }
    }



    /* Description:
    *   Instantenous boost of 40% speed for (0.15 * quantity) seconds after kill
    *
    * Growth type: Linear
    * Rarity: ? 
    */
    public class SpeedBoostAfterHitUpgrade : BaseUpgradeAfterEvent<TimedPositiveSpeedBonusEffect>  
    {
        const float BaseDuration = 2f;
        const float BaseBonus = 2f;
        const float SpeedUpgradeCoeficient = 0.5f;

        public SpeedBoostAfterHitUpgrade(UpgradeManager target, int quantity)
        : base(target, quantity) {}

        protected override TimedPositiveSpeedBonusEffect GetEffect()
        {
            return new TimedPositiveSpeedBonusEffect(
                new SpeedBonusEffect(target.effectReceiver, BaseBonus), 
                BaseDuration * quantity,
                null
            );
        }

        protected override void Update()
        {
            float newDuration = quantity * BaseDuration;
            effect.UpdateDuration(newDuration);

            float newSpeedBonus = quantity * SpeedUpgradeCoeficient * BaseBonus;
            effect.effect.Update(newSpeedBonus);
        }


        UnityAction<object, Damageable>? onHitListener;
        protected override void SubscribeToEvent()
        {
            onHitListener = (object o, Damageable d) => {Debug.Log("HERE!!"); effect.Start(); };
            target.damageable.onHit.AddListener(onHitListener);
        }

        protected override void UnsubscribeToEvent()
        {
            if (onHitListener != null)
            {
                target.damageable.onHit.RemoveListener(onHitListener);
            }
        }


    }



    /* Description:
    *   Gives 100% damage boost of for (1 * quantity) seconds after getting hit
    *
    * Growth type: Linear
    * Rarity: ? 
    */
    public class BaseDamageBonusAfterHit : BaseUpgradeAfterEvent<TimedPositiveDamageBonusEffect>  
    {
        const float BaseDuration = 1f;
        const int BaseBonus = 1;

        public BaseDamageBonusAfterHit(UpgradeManager target, int quantity)
        : base(target, quantity) {}


        protected override TimedPositiveDamageBonusEffect GetEffect()
        {
            return new TimedPositiveDamageBonusEffect(
                new DamageBonusEffect(target.effectReceiver, BaseBonus), 
                BaseDuration, 
                null
            );
        }

        // Duration scaling by 100% on duration
        protected override void Update()
        {
            float newDuration = quantity * BaseDuration;
            effect.duration = newDuration; // do not refresh
        }


        UnityAction<object, Damageable>? onHitListener;
        protected override void SubscribeToEvent()
        {
            onHitListener = (object o, Damageable d) => effect.Start();
            target.damageable.onHit.AddListener(onHitListener);
        }

        protected override void UnsubscribeToEvent()
        {
            if (onHitListener != null)
            {
                target.damageable.onHit.RemoveListener(onHitListener);
            }
        }
    }


    
}