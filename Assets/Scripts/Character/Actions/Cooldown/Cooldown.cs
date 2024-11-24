using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#nullable enable

public interface ICooldown
{
    public bool IsReadyForUse();

    // Returns true if activated. This means that the cooldown was "consumed" or reset
    public bool AttemptActivation();
}



[Serializable]
public class Cooldown : ICooldown
{
    [SerializeField] 
    float cooldown; 

    [SerializeField]
    float lastTimeUsed; 

    public bool IsOnCooldown => Time.time < lastTimeUsed + cooldown;

    public Cooldown(float cooldown)
    {
        this.cooldown = cooldown;
        this.lastTimeUsed = Time.time; //start timer
    }

    public float GetCooldown() => cooldown;
    public void SetCooldown(float value)
    {
        if (value <= 0)
        {
            Debug.LogError($"Interval time must be <= 0: {value}");
        }
        cooldown = value;
    }

    public bool IsReadyForUse() => ((lastTimeUsed + cooldown) - Time.time) > 0;
    public float GetRemaningTime() => (lastTimeUsed + cooldown) - Time.time;

    public void RefreshCooldown()
    {
        lastTimeUsed = Time.time;
    }

    bool ICooldown.AttemptActivation() 
    {
        bool used = IsReadyForUse();
        if (used)
        {
            RefreshCooldown();
        }
        return used;
    }
    
}



