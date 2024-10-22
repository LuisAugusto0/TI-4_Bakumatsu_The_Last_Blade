using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Definition of a component in which its instance must be unique, but
// differently from a normal Singleton, does not persist between scenes.
//
// Because of this behaviour it cannot:
//  - contain reference to instances of a scene
// 
// It still needs to be serialized as a instance, either to get prefab data or
// access to other components inside its game object
//
// When to use:
//   - For example, the PauseMenu needs to be open when Player presses the designated input,
//   however since player is a prefab it cant contain a reference to a instance in the scene,
//   and instead must access the PauseMenu in a different manner.
//   - Differently from normal Singleton, scoping the Singleton lifetime to the scene ensures more
//   flexiblity on the scene. Otherwise the object the Singleton is attached to is forced to persist
//   and always represent the Singleton
//
// Warning:
//   - If no instances where created on the scene, and they are expected by another instance,
//   there will be a NullException crash on the game

public abstract class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // NullException if never changed and all received
    private static T _instance = null;


    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    public virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    
    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            // Make sure only one instance exists
            Destroy(gameObject);
        }
    }

}