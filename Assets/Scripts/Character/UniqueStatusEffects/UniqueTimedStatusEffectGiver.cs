using System.Collections;
using System.Collections.Generic;
using UniqueStatusEffects.Implementations.UniqueTimedEffects;
using UniqueStatusEffects.Implementations.UniqueChargeEffect;
using UnityEngine;
using Effects.Implementations.TimedEffect;


public class UniqueTimedStatusEffectGiver : MonoBehaviour
{
    public UniqueTimedStatusEffectTable selectedUpgrade;
    public int upgradeQuantity = 1;

    // Static definitions

    void Awake()
    {
        OnTriggerInteractible interactible = GetComponent<OnTriggerInteractible>();
        if (interactible != null)
        {
            interactible.onInteractEvent.AddListener(GiveSelectedEffectFromGameObject);
        }
    }

    public static void GiveEffect<T>(UniqueStatusEffectManager target, int quantity = 1) 
        where T : ITimedUniqueEffect
    {
        if (target != null)
        {
            target.AddTimedEffect<T>(quantity);
        }
    }
 
    public static void GiveEffectFromEnum(UniqueTimedStatusEffectTable name, UniqueStatusEffectManager target, 
        int quantity = 3)
    {
        switch(name)
        {            
            case UniqueTimedStatusEffectTable.BasicSpeedBonusUniqueEffect:
                GiveEffect<BasicSpeedBonusUniqueEffect>(target, quantity);
                break;
        }
    }

    public void GiveSelectedEffect(UniqueStatusEffectManager target)
    {
        GiveEffectFromEnum(selectedUpgrade, target, upgradeQuantity);
    }


    public void GiveSelectedEffectFromGameObject(GameObject target)
    {
        var manager = target.GetComponent<UniqueStatusEffectManager>();
        
        if (manager != null) 
        {
            Debug.Log("ASSIGNED " + selectedUpgrade.ToString());
            GiveSelectedEffect(manager);
        }
        else
        {
            Debug.Log("Attempt givve update on invalid object. It should be properly filtered");
        }
    }
}

public enum UniqueTimedStatusEffectTable
{
    BasicSpeedBonusUniqueEffect,

}