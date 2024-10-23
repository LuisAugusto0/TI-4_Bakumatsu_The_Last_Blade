using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CooldownAction<T> : MonoBehaviour
{
    public bool isActive = false;
    public float cooldown = 1f;
    
    protected float _lastEndTime = 0f;

    public void ResetCooldown()
    {
        _lastEndTime = 0f;
    }

    public float GetRemainingTime() 
    {
        return _lastEndTime + cooldown - Time.time;
    }

    public bool CanPerform()
    {
        return (Time.time > _lastEndTime + cooldown) && !isActive;
    }

    
    public bool Attempt(T val)
    {
        bool canPerform = (Time.time > _lastEndTime + cooldown) && !isActive;
        
        if (canPerform)
        {
            isActive = true;
            Perform(val);
        }
        return canPerform;
    }

    protected virtual void End()
    {
        _lastEndTime = Time.time;
        isActive = false;
    }

    protected abstract void Perform(T obj);
}

public abstract class CharacterCooldownAction : CooldownAction<int>
{
    public Character character;
    
    // Default empty value
    public bool Attempt()
    {
        return Attempt(0);  
    }
}

public abstract class PlayerCooldownAction : CharacterCooldownAction
{
    public PlayerController playerController;
}
