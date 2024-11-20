using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EffectReceiver))]

// Hold references held in EffectReceive for less redirections
[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(EntityMovement))]
[RequireComponent(typeof(CharacterDamage))]

public class UpgradeManager : MonoBehaviour
{
    [Serializable]
    public class UpgradeUpdatedEvent : UnityEvent<Upgrade> {}


    [NonSerialized] public EffectReceiver effectReceiver;
    [NonSerialized] public Character character;
    [NonSerialized] public EntityMovement entityMovement;
    [NonSerialized] public Damageable damageable;
    [NonSerialized] public CharacterDamage characterDamage;

    // Dictionary containing each active upgrade
    // Ensures only one instance of each type of upgrade exists
    public Dictionary<Type, Upgrade> upgrades;

    // Invokers
    public UpgradeUpdatedEvent onUpgradeUpdated; 

    void Awake()
    {
        character = GetComponent<Character>();
        entityMovement = GetComponent<EntityMovement>();
        damageable = GetComponent<Damageable>();
        characterDamage = GetComponent<CharacterDamage>();

        effectReceiver = GetComponent<EffectReceiver>();
        upgrades = new();
    }

    public void ClearUpgrades()
    {
        foreach (Upgrade upgrade in upgrades.Values)
        {
            upgrade.Remove();
        }
        Debug.Assert(upgrades.Count == 0);
        onUpgradeUpdated.Invoke(null);
    }

    // Create upgrade if not exists ; add quantity if exists
    // Does not accept negative values as its not responsible for removal of upgrades 
    public void AddUpgrade<T>(int quantity) where T : Upgrade
    {
        if (quantity <= 0)
        {
            Debug.LogError("Cannot add upgrade with negative quantity");
            return;
        }

        var upgradeType = typeof(T);

        if (!upgrades.ContainsKey(upgradeType))
        {
            var upgrade = (T)Activator.CreateInstance(upgradeType, this, quantity);
            upgrades[upgradeType] = upgrade;
        }
        else
        {
            upgrades[upgradeType].AddToQuantity(quantity);
        }

        onUpgradeUpdated.Invoke(upgrades[upgradeType]);
        PrintUpgrades();
    }

    // Remove n quantity on the upgrade type given. If it reaches 0, remove from upgrade list
    // Does not accept negative values as its only responsible for subtractions
    public bool RemoveUpgrade<T>(int quantity) where T : Upgrade
    {
        if (quantity <= 0)
        {
            Debug.LogError("Cannot remove with negative quantity of upgrades");
            return false;
        }
        var upgradeType = typeof(T);

        if (upgrades.ContainsKey(upgradeType))
        {
            Upgrade instance = upgrades[upgradeType];
            instance.AddToQuantity(-quantity);
            onUpgradeUpdated.Invoke(instance);
            return true;
        }
        return false;
    }

    // Force remove instance regardless of quantity
    public void RemoveUpgradeInstance(Type upgradeType)
    {
        if (upgrades.ContainsKey(upgradeType))
        {
            upgrades.Remove(upgradeType);
            onUpgradeUpdated.Invoke(upgrades[upgradeType]);
        }
    }


    public void PrintUpgrades()
    {
        foreach (var upgradePair in upgrades)
        {
            Type upgradeType = upgradePair.Key;
            Upgrade upgrade = upgradePair.Value;
            int quantity = upgrade.Quantity;

            Debug.Log($"Upgrade Type: {upgradeType.Name}, Quantity: {quantity}");
        }
    }
}





