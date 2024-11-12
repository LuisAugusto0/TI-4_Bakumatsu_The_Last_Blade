using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Unity.VisualScripting;




[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(EntityMovement))]
[RequireComponent(typeof(CharacterDamage))]
public class StatusEffectManager : MonoBehaviour
{
    // Components used to change attributes / do certain effects
    [NonSerialized] public Character character;
    [NonSerialized] public EntityMovement entityMovement;
    [NonSerialized] public Damageable damageable;
    [NonSerialized] public CharacterDamage characterDamage;


    // Dictionary of current active effects
    [NonSerialized] public StackableEffectList stackableEffects;
    [NonSerialized] public UniqueTimedEffectDictionary uniqueTimedEffects;
    // [NonSerialized] public UniqueChargeEffectDictionary uniqueChargeEffects;
    
    public StackableEffectList.UpdatedEvent onStackableEffectUpdated;
    public UniqueTimedEffectDictionary.UpdatedEvent onUniqueTimedEffectUpdated;
    // public UniqueChargeEffectDictionary.UpdatedEvent onUniqueChargeEffectUpdated;

    void Awake()
    {
        character = GetComponent<Character>();
        entityMovement = GetComponent<EntityMovement>();
        damageable = GetComponent<Damageable>();
        characterDamage = GetComponent<CharacterDamage>();

        stackableEffects = new(onStackableEffectUpdated);
        uniqueTimedEffects = new(onUniqueTimedEffectUpdated);
        // uniqueChargeEffects = new(onUniqueChargeEffectUpdated);
    }

    public void ClearAllEffects()
    {
        stackableEffects.Clear();
        uniqueTimedEffects.Clear();
        // uniqueChargeEffects.Clear();
        
    }



    #region Data Structures

    /* ----------------------------------------
    * List containing all stackable effects
    *
    * Allows any number of instances of same type
    * Specific per StatusEffect implementation, as different data can be 
    * extracted from each
    * ----------------------------------------
    */

    public class StackableEffectList
    {
        [Serializable]
        public class UpdatedEvent : UnityEvent<StatusEffect> {}

        public UpdatedEvent onUpdated;
        public List<StatusEffect> stackedTimedEffects;

        public StackableEffectList(UpdatedEvent update)
        {
            onUpdated = update;
            stackedTimedEffects = new();
        }

        public void Clear()
        {
            foreach (StatusEffect effect in stackedTimedEffects)
            {
                effect.Remove();
            }

            Debug.Assert(stackedTimedEffects.Count == 0);
            onUpdated.Invoke(null);
        }
        
        public void Add(StatusEffect instance)
        {
            stackedTimedEffects.Add(instance);
            onUpdated.Invoke(instance);    
        }

        public void Remove(StatusEffect instance)
        {
            stackedTimedEffects.Remove(instance);
            onUpdated.Invoke(instance);    
        }
        
    }




    /* ----------------------------------------
    * Dictionaries for Unique Timed Effects
    *
    * Type specific dictionary - only allow one instance
    * Specific per StatusEffect implementation, as different methods may be called
    *
    * Adding multiple dictionaries per StatusEffectType might however be very costly
    * ----------------------------------------
    */

    public class UniqueTimedEffectDictionary
    {
        [Serializable]
        public class UpdatedEvent : UnityEvent<UniqueTimedStatusEffect> {}
        
        public Dictionary<Type, UniqueTimedStatusEffect> uniqueTimedEffects;
        UpdatedEvent onUpdated;

        public UniqueTimedEffectDictionary(UpdatedEvent updated)
        {
            onUpdated = updated;
            uniqueTimedEffects = new();
        }

        public void Clear()
        {
            foreach (TimedStatusEffect effect in uniqueTimedEffects.Values)
            {
                effect.Remove();
            }
            Debug.Assert(uniqueTimedEffects.Count == 0);
            onUpdated.Invoke(null);
        }


        // Create new effect if it does not exist ; Refresh duration if already exists
        // Does not accept negatives
        public void Add<T>(float duration) where T : UniqueTimedStatusEffect
        {
            if (duration <= 0)
            {
                Debug.LogError("Negative value not expected on add duration effect");
                return;
            }

            var effectType = typeof(T);

            if (!uniqueTimedEffects.ContainsKey(effectType))
            {
                var effect = (T)Activator.CreateInstance(effectType, this, duration);
                uniqueTimedEffects[effectType] = effect;
            }
            else
            {
                uniqueTimedEffects[effectType].RefreshDuration(duration);
            }

            onUpdated.Invoke(uniqueTimedEffects[effectType]);
            PrintDurationEffects();
        }

        
        // Force removal of a type of duration if its active
        public bool Remove(Type effectType) 
        {
            if (uniqueTimedEffects.ContainsKey(effectType))
            {
                UniqueTimedStatusEffect instance = uniqueTimedEffects[effectType];
                instance.Remove();
                onUpdated.Invoke(instance);
                return true;
            }

            return false;
        }


        public bool AddToDuration<T>(float newDuration) 
            where T : UniqueTimedStatusEffect
        {
            var effectType = typeof(T);

            if (uniqueTimedEffects.ContainsKey(effectType))
            {
                UniqueTimedStatusEffect instance = uniqueTimedEffects[effectType];
                instance.AddToDuration(newDuration);
                onUpdated.Invoke(instance);
                return true;
            }

            return false;
        }


        public void PrintDurationEffects()
        {
            foreach (var pair in uniqueTimedEffects)
            {
                Type upgradeType = pair.Key;
                TimedStatusEffect upgrade = pair.Value;
                float duration = upgrade.Duration;
                float time = upgrade.GetRemainingTime();

                Debug.Log($"Upgrade Type: {upgradeType.Name}, RemainingTime: {time}, Duration: {duration}");
            }
        }
    }


    // -- Abandoned for now. Implementation might not be needed --
    [Obsolete]
    public class UniqueChargeEffectDictionary
    {
        [Serializable]
        public class UpdatedEvent : UnityEvent<ChargeStatusEffect> {}

        public UpdatedEvent onUpdate;
        public Dictionary<Type, ChargeStatusEffect> chargeEffects;


        public UniqueChargeEffectDictionary(UpdatedEvent updated)
        {
            onUpdate = updated;
            chargeEffects = new();
        }

        
        public void Add<T>(int charges) where T : ChargeStatusEffect
        {
            if (charges <= 0)
            {
                Debug.LogError("Negative value not expected on add charge effect");
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
                chargeEffects[effectType].AddToCharges(charges);
            }

            onUpdate.Invoke(chargeEffects[effectType]);
        }

        // Force removal of a type of duration if its active
        public bool Remove(Type effectType) 
        {
            if (chargeEffects.ContainsKey(effectType))
            {
                ChargeStatusEffect instance = chargeEffects[effectType];
                instance.Remove();
                onUpdate.Invoke(instance);
                return true;
            }

            return false;
        }

        public bool AddToCharge<T>(int quantity) 
        {
            Type effectType = typeof(T);
            if (chargeEffects.ContainsKey(effectType))
            {
                ChargeStatusEffect instance = chargeEffects[effectType];
                instance.AddToCharges(quantity);
                onUpdate.Invoke(instance);
                return true;
            }
            return false;
        }



        public void Print()
        {
            foreach (var pair in chargeEffects)
            {
                Type upgradeType = pair.Key;
                ChargeStatusEffect upgrade = pair.Value;
                float charges = upgrade.Charges;

                Debug.Log($"Upgrade Type: {upgradeType.Name}, Charges: {charges}");
            }
        }

        public void Clear()
        {
            foreach (ChargeStatusEffect effect in chargeEffects.Values)
            {
                effect.Remove();
            }
            Debug.Assert(chargeEffects.Count == 0);
            onUpdate.Invoke(null);
        }

    }

    #endregion

}

