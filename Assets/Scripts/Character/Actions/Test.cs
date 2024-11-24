using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Test/Prefab With Component")]
public class PrefabWithComponentSO<T> : ScriptableObject where T : MonoBehaviour
{
    public GameObject prefab;

    public T Instantiate(Transform parent)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is null!");
            return null;
        }

        GameObject instance = Object.Instantiate(prefab, parent);
        T component = instance.GetComponent<T>();
        if (component == null)
        {
            Debug.LogError($"Prefab {prefab.name} does not contain the required component of type {typeof(T).Name}");
            Object.Destroy(instance);
            return null;
        }
        return component;
    }
}