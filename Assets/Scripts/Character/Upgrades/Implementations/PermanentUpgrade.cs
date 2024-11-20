using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effects.Implementations.PersistantEffect;
using System.Data;
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
            effect.Update(newBonus);
        }

        protected override HealthBonusEffect GetEffect()
        {
            return new HealthBonusEffect(target.effectReceiver, quantity * baseBonus);
        }
    
    }


}
