using Effects.Implementations.CompositePersistantEffect;
using Effects.Implementations.PersistantEffect;
using Effects.Implementations.TimedEffect;
using UniqueStatusEffects.Implementations.UniqueChargeEffect;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
#nullable enable

namespace Upgrades.Implementations.EventUpgrade
{
    /* Description:
    *   Makes target invincible for (2 * quantity) seconds after hit
    *
    * Growth type: Linear
    * Rarity: ? 
    */
    
    public class EscapeUpgrade : BaseUpgradeAfterEventEffect<TimedEscapeEffect>
    {
        static Sprite? icon;
        public static Sprite? GetStaticIcon() => icon;
        public override Sprite? GetIcon() => icon;

        public static void LoadIcon(Sprite sprite) => icon = sprite;
        public static void UnloadIcon() => icon = null;


        static string? iconAdressablePath;
        public static string? StaticGetIconAdressablePath() => iconAdressablePath;
        public override string? GetIconAdressablePath() => iconAdressablePath;
        public static void SetIconAdressablePath(string s) => iconAdressablePath = s;
        public static void AssertIconAdressablePath() => UpgradeIconAdressable.AssertIsPathValid(iconAdressablePath);


        const float BaseDuration = 1.5f;
        const float BaseMultiplier = 1.5f;

        public EscapeUpgrade(UpgradeManager target, int quantity)
        : base(target, quantity){}

        protected PairEffect<ImmunityEffect, SpeedMultiplierEffect> GetImmunityEffect()
        {
            return new PairEffect<ImmunityEffect, SpeedMultiplierEffect>(
                new ImmunityEffect(target.effectReceiver), 
                new SpeedMultiplierEffect(target.effectReceiver, BaseMultiplier)
            );
        }
        protected override TimedEscapeEffect GetEffect()
        {
            return new TimedEscapeEffect(
                GetImmunityEffect(), CalculateDuration(), null
            );
        }

        

        // Duration = 2.5 * 1.5 * quantity
        public float CalculateDuration()
        {
            return BaseDuration + quantity * 1.5f;
        }


        protected override void Update()
        {
            effect.RefreshUpdateDuration(CalculateDuration());
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
    *   Instantenous boost of 100% speed for (1 * quantity) seconds after kill
    *
    * Growth type: Linear
    * Rarity: ? 
    */
    public class SpeedBoostAfterHitUpgrade : BaseUpgradeAfterEventEffect<TimedPositiveSpeedMultiplier>  
    {
        static Sprite? icon;
        public static Sprite? GetStaticIcon() => icon;
        public override Sprite? GetIcon() => icon;

        public static void LoadIcon(Sprite sprite) => icon = sprite;
        public static void UnloadIcon() => icon = null;


        static string? iconAdressablePath;
        public static string? StaticGetIconAdressablePath() => iconAdressablePath;
        public override string? GetIconAdressablePath() => iconAdressablePath;
        public static void SetIconAdressablePath(string s) => iconAdressablePath = s;
        public static void AssertIconAdressablePath() => UpgradeIconAdressable.AssertIsPathValid(iconAdressablePath);


        const float BaseDuration = 1f;
        const float BaseMultiplier = 2f;

        public SpeedBoostAfterHitUpgrade(UpgradeManager target, int quantity)
        : base(target, quantity) {}

        public float CalculateDuration() => BaseDuration * quantity;


        protected override TimedPositiveSpeedMultiplier GetEffect()
        {
            return new TimedPositiveSpeedMultiplier(
                new SpeedMultiplierEffect(target.effectReceiver, BaseMultiplier), 
                CalculateDuration(),
                null
            );
        }

    
        protected override void Update()
        {
            float newDuration = CalculateDuration();
            effect.RefreshUpdateDuration(newDuration);
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
    public class BeserkUpgrade : BaseUpgradeAfterEventEffect<TimedPositiveDamageMultiplierEffect>  
    {
        static Sprite? icon;
        public static Sprite? GetStaticIcon() => icon;
        public override Sprite? GetIcon() => icon;

        public static void LoadIcon(Sprite sprite) => icon = sprite;
        public static void UnloadIcon() => icon = null;


        static string? iconAdressablePath;
        public static string? StaticGetIconAdressablePath() => iconAdressablePath;
        public override string? GetIconAdressablePath() => iconAdressablePath;
        public static void SetIconAdressablePath(string s) => iconAdressablePath = s;
        public static void AssertIconAdressablePath() => UpgradeIconAdressable.AssertIsPathValid(iconAdressablePath);


        const float BaseDuration = 0.25f;
        const float BaseMultiplier = 2f;

        public BeserkUpgrade(UpgradeManager target, int quantity)
        : base(target, quantity) {}


        protected override TimedPositiveDamageMultiplierEffect GetEffect()
        {
            return new TimedPositiveDamageMultiplierEffect(
                new DamageMultiplierEffect(target.effectReceiver, BaseMultiplier), 
                quantity * BaseDuration, 
                null
            );
        }

        // Duration scaling by 100% on duration
        protected override void Update()
        {
            float newDuration = quantity * BaseDuration;
            effect.duration = newDuration; // do not refresh
        }


        UnityAction<Damageable, TriggerDamager>? onKillListener;
        protected override void SubscribeToEvent()
        {
            onKillListener = (Damageable d, TriggerDamager t) => {effect.Start(); Debug.Log("Invoked beserk");};
            target.characterDamage.onKill.AddListener(onKillListener);
        }

        protected override void UnsubscribeToEvent()
        {
            if (onKillListener != null)
            {
                target.characterDamage.onKill.AddListener(onKillListener);
            }
        }
    }


    public class FireOnHitUpgrade : BaseUpgradeAfterEvent 
    {
        static Sprite? icon;
        public static Sprite? GetStaticIcon() => icon;
        public override Sprite? GetIcon() => icon;

        public static void LoadIcon(Sprite sprite) => icon = sprite;
        public static void UnloadIcon() => icon = null;


        static string? iconAdressablePath;
        public static string? StaticGetIconAdressablePath() => iconAdressablePath;
        public override string? GetIconAdressablePath() => iconAdressablePath;
        public static void SetIconAdressablePath(string s) => iconAdressablePath = s;
        public static void AssertIconAdressablePath() => UpgradeIconAdressable.AssertIsPathValid(iconAdressablePath);


        const int BaseCharges = 1;
        const float BaseChance = 1f;
        int currentCharges = BaseCharges;
        float currenceChance = BaseChance;

        public FireOnHitUpgrade(UpgradeManager target, int quantity)
        : base(target, quantity) 
        {
            Update();
            // Not best way to do this but for now...
            var ronin = target.GetComponent<RoninPlayerBehaviour>();
            if (ronin != null)
            {
                Material m = ronin.FlameSwordMaterial;
                ronin.character.mainSpriteRenderer.material = m;
            }
        }


        // Duration scaling by 100% on duration
        protected override void Update()
        {
            currenceChance = BaseChance * quantity;
            currentCharges = BaseCharges + quantity;
        }


        UnityAction<Damageable, TriggerDamager>? onHitListener;
        protected override void SubscribeToEvent()
        {
            onHitListener = (Damageable d, TriggerDamager t) => ApplyEffect(d);
            target.characterDamage.onAttackHit.AddListener(onHitListener);
        }

        void ApplyEffect(Damageable target)
        {
            var manager = target.uniqueStatusEffectManager;

            Debug.Log("Invoked");
            if (manager != null)
            {
                Debug.Log("HREEE~!");
                if (Random.Range(0.0f, 1.0f) > (1 - BaseChance))
                {
                    manager.AddChargeEffect<BurningUniqueEffect>(currentCharges);
                    Debug.Log("Efeito de fogo dado");
                }
            }
        }

        protected override void UnsubscribeToEvent()
        {
            if (onHitListener != null)
            {
                target.characterDamage.onKill.AddListener(onHitListener);
            }
        }

        public override void Remove()
        {
            base.Remove();
            var ronin = target.GetComponent<RoninPlayerBehaviour>();
            if (ronin != null)
            {
                Material m = ronin.BaseMaterial;
                ronin.character.mainSpriteRenderer.material = m;
            }
        }
    }
}
