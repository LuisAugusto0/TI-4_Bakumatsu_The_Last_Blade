using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effects.Implementations.PersistantEffect;
using System.Data;
using Effects.Implementations.TimedEffect;
using System;
#nullable enable

namespace Upgrades.Implementations.PermanentUpgrade
{


    public class SpeedBonusUpgrade : BasePermanentUpgrade<SpeedBonusEffect> {
        const float bonus = 1f;


        public SpeedBonusUpgrade(UpgradeManager target, int quantity)
        : base(target, quantity) {}

        // Linear growth of 100% by quantity
        protected override void Update()
        {
            float newBonus = quantity * bonus;
            effect.Update(newBonus);
        }

        protected override SpeedBonusEffect GetEffect()
        {
            return new SpeedBonusEffect(target.effectReceiver, quantity * bonus);
        }
    }

    public class DoubleSpeedUpgrade : BasePermanentUpgrade<SpeedMultiplierEffect> {
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

    public class DamageBonusStatBoost : BasePermanentUpgrade<DamageBonusEffect> {
        const int bonus = 1;


        public DamageBonusStatBoost(UpgradeManager target, int quantity)
        : base(target, quantity) {}

        // Linear growth of 100% by quantity
        protected override void Update()
        {
            int newBonus = quantity * bonus;
            effect.Update(newBonus);
        }

        protected override DamageBonusEffect GetEffect()
        {
            return new DamageBonusEffect(target.effectReceiver, quantity * bonus);
        }
    }


    public class DamageMultiplierStatBonus : BasePermanentUpgrade<DamageMultiplierEffect> {
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


    public class BaseHealthBonusUpgrade : BasePermanentUpgrade<HealthBonusEffect> 
    {
        const int baseBonus = 1;        

        public BaseHealthBonusUpgrade(UpgradeManager target, int quantity)
        : base(target, quantity) {}

        // Linear growth of 100% by quantity
        protected override void Update()
        {
            int newBonus = quantity * baseBonus;

            // Atualiza o bônus de vida no jogador
            var damageable = target.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.IncreaseBaseHealthBonus(newBonus); // Atualiza o baseHp e o currentHp

                // Atualiza a UI para refletir o novo máximo de vida
                var gameplayUI = GameObject.FindObjectOfType<GameplayUI>();
                if (gameplayUI != null)
                {
                    gameplayUI.UpdateHeartsUI(); // Atualiza os corações dinamicamente
                }
                else
                {
                    Debug.LogWarning("GameplayUI não encontrado. Certifique-se de que está ativo na cena.");
                }
            }
            else
            {
                Debug.LogError("Componente Damageable não encontrado no jogador!");
            }
        }



        protected override HealthBonusEffect GetEffect()
        {
            return new HealthBonusEffect(target.effectReceiver, quantity * baseBonus);
        }
    
    }


}
