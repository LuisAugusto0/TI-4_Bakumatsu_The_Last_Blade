using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Upgrades.Implementations.EventUpgrade;
using Upgrades.Implementations.PermanentUpgrade;

public class UpgradeGiver : MonoBehaviour
{
    public UpgradesNameTable selectedUpgrade;
    public int upgradeQuantity = 1;

    // Static definitions

    void Awake()
    {
        OnTriggerInteractible interactible = GetComponent<OnTriggerInteractible>();
        if (interactible != null)
        {
            interactible.onInteractEvent.AddListener(GiveSelectedUpgradeFromGameObject);
        }
    }

    public static void GiveUpgrade<T>(UpgradeManager target, int quantity = 1) 
        where T : Upgrade
    {
        if (target != null)
        {
            target.AddUpgrade<T>(quantity);
        }
    }
 
    public static void GiveUpgradeFromEnum(UpgradesNameTable name, UpgradeManager target, 
        int quantity = 1)
    {
        switch(name)
        {            
            case UpgradesNameTable.SpeedBonusUpgrade:
                GiveUpgrade<SpeedBonusUpgrade>(target, quantity);
                break;
            case UpgradesNameTable.DoubleSpeedUpgrade:
                GiveUpgrade<DoubleSpeedUpgrade>(target, quantity);
                break;
            case UpgradesNameTable.DamageBonusStatBoost:
                GiveUpgrade<DamageBonusStatBoost>(target, quantity);
                break;
            case UpgradesNameTable.DamageMultiplierStatBonus:
                GiveUpgrade<DamageMultiplierStatBonus>(target, quantity);
                break;
            case UpgradesNameTable.BaseHealthBonusUpgrade:
                GiveUpgrade<BaseHealthBonusUpgrade>(target, quantity);
                break;
            case UpgradesNameTable.ImmunityAfterHitUpgrade:
                GiveUpgrade<ImmunityAfterHitUpgrade>(target, quantity);
                break;
            case UpgradesNameTable.SpeedBoostAfterHitUpgrade:
                GiveUpgrade<SpeedBoostAfterHitUpgrade>(target, quantity);
                break;
            case UpgradesNameTable.BaseDamageBonusAfterHit:
                GiveUpgrade<BaseDamageBonusAfterHit>(target, quantity);
                break;
            default:
                Debug.LogWarning("Invalid effect give attempt");
                break;
            

        }
    }





    // Instance methods

    public void GiveSelectedUpgrade(UpgradeManager target)
    {
        GiveUpgradeFromEnum(selectedUpgrade, target, upgradeQuantity);
    }

    public void GiveSelectedUpgradeFromGameObject(GameObject target)
    {
        UpgradeManager manager = target.GetComponent<UpgradeManager>();
        
        if (manager != null) 
        {
            Debug.Log("ASSIGNED " + selectedUpgrade.ToString());
            GiveSelectedUpgrade(manager);
        }
        else
        {
            Debug.Log("Attempt givve update on invalid object. It should be properly filtered");
        }
    }
}

public enum UpgradesNameTable
{
    SpeedBonusUpgrade,
    DoubleSpeedUpgrade,
    DamageBonusStatBoost,
    DamageMultiplierStatBonus,
    BaseHealthBonusUpgrade,

    ImmunityAfterHitUpgrade,
    SpeedBoostAfterHitUpgrade,
    BaseDamageBonusAfterHit,  
}


