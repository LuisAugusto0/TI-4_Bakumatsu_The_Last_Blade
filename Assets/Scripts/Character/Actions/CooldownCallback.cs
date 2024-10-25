using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Il2Cpp;
using UnityEngine;


// Separate object that contains 

public abstract class IAction : MonoBehaviour
{
    public abstract void StartAction();
}

public interface CooldownBehaviour
{
    public bool Attempt();
}


[Serializable]
public class CooldownCallback : CooldownBehaviour
{
    public delegate void FunctionCallback();    
    public FunctionCallback callback;

    public float cooldown = 0f;
    public float lastExecutedTime = 0f;

    public CooldownCallback(FunctionCallback callback)
    {
        this.callback = callback;
    }

    public CooldownCallback(IAction action)
    {
        this.callback = action.StartAction;
    }

    public bool Attempt()
    {
        bool success = false;
        if (Time.time >= lastExecutedTime + cooldown)
        {
            success = true;
            callback.Invoke();
        }
        return success;
    }
}



[Serializable]
public class IterableCooldownCallback : CooldownBehaviour
{
    public delegate void FunctionCallback();    
    public FunctionCallback callback;

    public int maxCharges = 1;
    
    [SerializeField]
    private int storedCharges;
    
    
    public float cooldown = 0f;
    public float lastExecutedTime = 0f;

    public IterableCooldownCallback(FunctionCallback callback)
    {
        this.callback = callback;
    }

    public IterableCooldownCallback(IAction action)
    {
        this.callback = action.StartAction;
    }

    public bool Attempt()
    {
        bool success = false;
        if (Time.time >= lastExecutedTime + cooldown)
        {
            success = true;
            callback.Invoke();
        }
        return success;
    }
}
