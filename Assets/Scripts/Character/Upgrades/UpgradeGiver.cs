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

    public static void GiveUpgrade<T>(BaseUpgradeManager target, int quantity = 1) 
        where T : Upgrade
    {
        if (target != null)
        {
            target.AddUpgrade<T>(quantity);
        }
    }
 
    public static void GiveUpgradeFromEnum(UpgradesName name, BaseUpgradeManager target, 
        int quantity = 1)
    {
        switch(name)
        {            
            case UpgradesName.SpeedBoost:
                GiveUpgrade<SpeedBoost>(target, quantity);
                break;
            case UpgradesName.SpeedBoostAfterHit:
                GiveUpgrade<SpeedBoostAfterHit>(target, quantity);
                break;
            case UpgradesName.DoubleSpeed:
                GiveUpgrade<DoubleSpeed>(target, quantity);
                break;
            case UpgradesName.DamageBoost:
                GiveUpgrade<DamageBoost>(target, quantity);
                break;
            case UpgradesName.DamageBoostAfterHit:
                GiveUpgrade<DamageBoostAfterHit>(target, quantity);
                break;
            case UpgradesName.DoubleDamage:
                GiveUpgrade<DoubleDamage>(target, quantity);
                break;
            case UpgradesName.HealthBoost:
                GiveUpgrade<HealthBoost>(target, quantity);
                break;
            default:
                Debug.LogWarning("Invalid effect give attempt");
                break;
            

        }
    }


    public static void GiveDamageBoost(BaseUpgradeManager target, int quantity = 1)
    {
        GiveUpgrade<DamageBoost>(target, quantity);
    }  

    public static void Give(BaseUpgradeManager target, int quantity = 1)
    {
        GiveUpgrade<DamageBoost>(target, quantity);
    }  



    // Instance methods

    public void GiveSelectedUpgrade(BaseUpgradeManager target)
    {
        GiveUpgradeFromEnum(selectedUpgrade, target, upgradeQuantity);
    }

    public void GiveSelectedUpgradeFromGameObject(GameObject target)
    {
        BaseUpgradeManager manager = target.GetComponent<BaseUpgradeManager>();
        
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
    SpeedBoost,
    SpeedBoostAfterHit,
    DoubleSpeed,
    DamageBoost,
    DamageBoostAfterHit,
    DoubleDamage,
    HealthBoost,
    None,   
}