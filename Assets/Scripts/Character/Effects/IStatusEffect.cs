using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Effects that contain a defined end but cannot be instantaneous
 * 
 * Contains a method to return the remaining duration, that can be either:
 *   int: remaning absolute quantifier
 *   float: remaning time in seconds
 *
 * When called while already active, Start() will refresh duration instead
 *
 * Refresh will attempt to refresh the duration to value if it is higher than
 * remaning duration. 
 * Does not change the duration when used Start() 
 */
#nullable enable

public interface ILifeTimedEffect : INonPersistantEffect
{
    public bool IsActive();
    public void ForceStop();
    public abstract Sprite? GetIcon();
}



public interface ITimedStatusEffect : ILifeTimedEffect
{
    public void UpdateRemaningTime(float value);
    public float GetRemainingDuration();
}

public interface IChargeStatusEffect : ILifeTimedEffect 
{
    public void Refresh(int value);
    public float GetRemainingIntervalDuration();
    public int GetRemaningCharges();
}






