using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeGiver : MonoBehaviour
{
    public UpgradesName selectedUpgrade;
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
 
    public static void GiveUpgradeFromEnum(UpgradesName name, UpgradeManager target, 
        int quantity = 1)
    {
        switch(name)
        {            
            case UpgradesName.SpeedBonus:
                GiveUpgrade<SpeedBonusUpgrade>(target, quantity);
                break;
            case UpgradesName.SpeedBonusAfterHit:
                GiveUpgrade<SpeedBonusAfterHitUpgrade>(target, quantity);
                break;
            case UpgradesName.DoubleSpeed:
                GiveUpgrade<DoubleSpeedUpgrade>(target, quantity);
                break;
            case UpgradesName.ImmunityAfterHit:
                GiveUpgrade<ImmunityAfterHitUpgrade>(target, quantity);
                break;
            case UpgradesName.DamageBoostAfterHit:
                GiveUpgrade<DamageBonusUpgrade>(target, quantity);
                break;
            case UpgradesName.DoubleDamage:
                GiveUpgrade<DoubleDamageUpgrade>(target, quantity);
                break;
            case UpgradesName.SingleHealthBoost:
                GiveUpgrade<SingleHealthBonusUpgrade>(target, quantity);
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

public enum UpgradesName
{
    SpeedBonus,
    SpeedBonusAfterHit,
    DoubleSpeed,
    ImmunityAfterHit,
    DamageBoostAfterHit,
    DoubleDamage,
    SingleHealthBoost,
    None,   
}