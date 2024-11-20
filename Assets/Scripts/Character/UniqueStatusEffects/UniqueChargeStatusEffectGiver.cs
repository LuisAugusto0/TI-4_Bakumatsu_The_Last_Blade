using System.Collections;
using System.Collections.Generic;
using UniqueStatusEffects.Implementations.UniqueTimedEffects;
using UniqueStatusEffects.Implementations.UniqueChargeEffect;
using UnityEngine;


public class UniqueChargeStatusEffectGiver : MonoBehaviour
{
    public UniqueChargeStatusEffectTable selectedUpgrade;
    public int charges = 5;

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
        where T : IChargeUniqueEffect
    {
        if (target != null)
        {
            target.AddChargeEffect<T>(quantity);
        }
    }
 
    public static void GiveEffectFromEnum(UniqueChargeStatusEffectTable name, UniqueStatusEffectManager target, 
        int quantity = 3)
    {
        switch(name)
        {            
            case UniqueChargeStatusEffectTable.BurningUniqueEffect:
                GiveEffect<BurningUniqueEffect>(target, quantity);
                break;
        }
    }

    public void GiveSelectedEffect(UniqueStatusEffectManager target)
    {
        GiveEffectFromEnum(selectedUpgrade, target, charges);
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

public enum UniqueChargeStatusEffectTable
{
    BurningUniqueEffect,

}