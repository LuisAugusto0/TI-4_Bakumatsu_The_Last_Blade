using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effects.Implementations.PersistantEffect;
using System.Data;
using Effects.Implementations.TimedEffect;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#nullable enable

namespace Upgrades.Implementations.PermanentUpgrade
{
    public class SpeedBonusUpgrade : BasePermanentEffectUpgrade<FixedSpeedBonusEffect> {
        static Sprite? icon = null;
        public static Sprite? GetStaticIcon() => icon;
        public override Sprite? GetIcon() => icon;
        
        public static void LoadIcon(Sprite sprite) => icon = sprite;
        public static void UnloadIcon() => icon = null;

        static string? iconAdressablePath;
        public static string? StaticGetIconAdressablePath() => iconAdressablePath;
        public override string? GetIconAdressablePath() => iconAdressablePath;
        public static void SetIconAdressablePath(string s) => iconAdressablePath = s;
        public static void AssertIconAdressablePath() => UpgradeIconAdressable.AssertIsPathValid(iconAdressablePath);


        const float bonus = 1f;

        public SpeedBonusUpgrade(UpgradeManager target, int quantity)
        : base(target, quantity) {}

        // Linear growth of 100% by quantity
        protected override void Update()
        {
            float newBonus = quantity * bonus;
            effect.Update(newBonus);
        }

        protected override FixedSpeedBonusEffect GetEffect()
        {
            return new FixedSpeedBonusEffect(target.effectReceiver, quantity * bonus);
        }
    }

    public class DoubleSpeedUpgrade : BasePermanentEffectUpgrade<SpeedMultiplierEffect> {
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

        
        
        const float baseMultiplier = 1f;

        public DoubleSpeedUpgrade(UpgradeManager target, int quantity)
        : base(target, quantity) {}

        // Linear growth of 100% speed per upgrade
        protected override void Update()
        {

            float newMultiplier = quantity + baseMultiplier;
            effect.Update(newMultiplier);
        }

        protected override SpeedMultiplierEffect GetEffect()
        {
            return new SpeedMultiplierEffect(target.effectReceiver, quantity * baseMultiplier);
        }
    }

    public class DamageBonusStatBoost : BasePermanentEffectUpgrade<FixedDamageBonusEffect> {
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



        const int bonus = 1;

        public DamageBonusStatBoost(UpgradeManager target, int quantity)
        : base(target, quantity) {}

        // Linear growth of 100% by quantity
        protected override void Update()
        {
            int newBonus = quantity * bonus;
            effect.Update(newBonus);
        }

        protected override FixedDamageBonusEffect GetEffect()
        {
            return new FixedDamageBonusEffect(target.effectReceiver, quantity * bonus);
        }
    }


    public class DamageMultiplierStatBonus : BasePermanentEffectUpgrade<DamageMultiplierEffect> {
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


        
        const float baseMultiplier = 2;

        public DamageMultiplierStatBonus(UpgradeManager target, int quantity)
        : base(target, quantity) {}

        // Linear growth of 100% by quantity
        protected override void Update()
        {
            float newBonus = quantity * baseMultiplier;
            effect.Update(newBonus);
        }

        protected override DamageMultiplierEffect GetEffect()
        {
            Debug.Log("AAAA" +quantity);
            return new DamageMultiplierEffect(target.effectReceiver, quantity * baseMultiplier);
        }
    }


    public class BaseHealthBonusUpgrade : BasePermanentEffectUpgrade<FixedHealthBonusEffect> 
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



        const int baseBonus = 2;
        public event Action? OnHealthUpdate;   

        public BaseHealthBonusUpgrade(UpgradeManager target, int quantity)
        : base(target, quantity) {}

        // Linear growth of 100% by quantity
        protected override void Update()
        {
            OnHealthUpdate?.Invoke();
            int newBonus = quantity * baseBonus;
            effect.Update(newBonus);
        }

        protected override FixedHealthBonusEffect GetEffect()
        {
            return new FixedHealthBonusEffect(target.effectReceiver, quantity * baseBonus);
        }
    
    }





}
