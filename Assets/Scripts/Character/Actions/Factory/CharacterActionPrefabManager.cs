using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPrefabManager : MonoBehaviour
{
    // Contains same name as asset file:
    // Instead of matching enum with string, c# allows you to turn
    // enum into its exact string match on ToString() method
    public enum PrefabName
    {
        Player,
        Enemy,
        PowerUp
    }

    private static readonly Dictionary<PrefabName, GameObject> _prefabs = new();

    static ActionPrefabManager()
    {
        foreach (PrefabName prefabEnum in Enum.GetValues(typeof(PrefabName)))
        {
            string name = prefabEnum.ToString();
            var prefab = Resources.Load<GameObject>(name.ToString());
            if (prefab != null)
            {
                _prefabs[prefabEnum] = prefab;
            }
            else
            {
                Debug.LogError($"Prefab '{name}' could not be loaded. Check if it exists in the Resources folder.");
            }
        }

        Debug.Log("PrefabManager initialized.");
    }

    /// <summary>
    /// Gets the prefab by its name.
    /// </summary>
    /// <param name="name">The name of the prefab.</param>
    /// <returns>The prefab GameObject, or null if not found.</returns>
    public static GameObject GetPrefab(PrefabName name)
    {
        if (_prefabs.TryGetValue(name, out var prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"Prefab '{name}' not found.");
        return null;
    }

    /// <summary>
    /// Gets a list of all available prefab names.
    /// </summary>
    /// <returns>A list of prefab names.</returns>
    public static List<PrefabName> GetPrefabNames()
    {
        return new List<PrefabName>(_prefabs.Keys);
    }
    
}
