using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Unity.VisualScripting;


[RequireComponent(typeof(EffectReceiver))]
public class UniqueStatusEffectManager : MonoBehaviour
{
    // Components used to change attributes / do certain effects
    [NonSerialized] public EffectReceiver effectReceiver;

    [Serializable]
    public class StatusEffectUpdatedEvent : UnityEvent<Type> {}

    // Dictionary of current active effects
  
    public Dictionary<Type, ITimedUniqueEffect> timedEffects = new();
    
  
    public Dictionary<Type, IChargeUniqueEffect> chargeEffects = new();



    public StatusEffectUpdatedEvent OnStatusEffectUpdated; 

    void Awake()
    {
        effectReceiver = GetComponent<EffectReceiver>();
    }

    public void ClearEffects()
    {
        foreach (ITimedUniqueEffect effect in timedEffects.Values)
        {
            effect.Remove();
        }

        foreach (IChargeUniqueEffect effect in chargeEffects.Values)
        {
            effect.Remove();
        }

        OnStatusEffectUpdated.Invoke(null);
    }

 
    public void AddTimedEffect<T>(float duration) where T : ITimedUniqueEffect
    {
        if (duration <= 0)
        {
            Debug.LogError("Cannot refresh to negative duration");
            return;
        }

        var upgradeType = typeof(T);

        if (!timedEffects.ContainsKey(upgradeType))
        {
            var effect = (T)Activator.CreateInstance(upgradeType, this, duration);
            timedEffects[upgradeType] = effect;
        }
        else
        {
            timedEffects[upgradeType].GetEffectReference().UpdateRemaningTime(duration); // How to deal with updates?
        }

        OnStatusEffectUpdated.Invoke(upgradeType);
        PrintTimedUpgrades();
    }

    public void AddChargeEffect<T>(int charges) where T : IChargeUniqueEffect
    {
        if (charges <= 0)
        {
            Debug.LogError("Cannot refresh to negative duration");
            return;
        }

        var effectType = typeof(T);

        if (!chargeEffects.ContainsKey(effectType))
        {
            var effect = (T)Activator.CreateInstance(effectType, this, charges);
            chargeEffects[effectType] = effect;
        }
        else
        {
            chargeEffects[effectType].GetEffectReference().Refresh(charges); // How to deal with updates?
        }

        OnStatusEffectUpdated.Invoke(effectType);
        PrintChargeUpgrades();
    }

    // Force remove instance regardless of quantity
    public void RemoveTimedEffect(Type upgradeType)
    {
        if (timedEffects.ContainsKey(upgradeType))
        {
            timedEffects.Remove(upgradeType);
            OnStatusEffectUpdated.Invoke(upgradeType);
        }
        PrintTimedUpgrades();
    }

    public void RemoveChargeEffect(Type upgradeType)
    {
        if (chargeEffects.ContainsKey(upgradeType))
        {
            chargeEffects.Remove(upgradeType);
            OnStatusEffectUpdated.Invoke(upgradeType);
        }
        PrintChargeUpgrades();
    }




    public void PrintTimedUpgrades()
    {
        foreach (var effectPair in timedEffects)
        {
            Type effectType = effectPair.Key;
            ITimedUniqueEffect effect = effectPair.Value;
            float duration = effect.GetEffectReference().GetRemainingDuration();

            Debug.Log($"Timed Effect Type: {effectType.Name}, Duration: {duration}");
        }
    }


    public void PrintChargeUpgrades()
    {
        foreach (var effectPair in chargeEffects)
        {
            Type effectType = effectPair.Key;
            IChargeUniqueEffect effectRef = effectPair.Value;
            var effect = effectRef.GetEffectReference();
            int charge = effect.GetRemaningCharges();
            float remainingInterval = effect.GetRemainingIntervalDuration();

            Debug.Log($"Charge Effect Type: {effectType.Name}, Charges: {charge}, Interval: {remainingInterval}");
        }
    }

    void Update()
    {
        PrintChargeUpgrades();
    }


}

