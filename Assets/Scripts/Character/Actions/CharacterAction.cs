using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAction : MonoBehaviour
{
    public Character character;
    public bool isActive;
    public float cooldown = 1f;
    
    protected float _lastEndTime = 0f;

    public void ResetCooldown()
    {
        _lastEndTime = 0f;
    }

    public bool Attempt()
    {
        bool canPerform = (Time.time > _lastEndTime + cooldown) && !isActive;
        
        if (canPerform)
        {
            isActive = true;
            Perform();
        }
        return canPerform;
    }

    protected virtual void End()
    {
        _lastEndTime = Time.time;
        isActive = false;
    }

    protected abstract void Perform(int context = 0);
}
