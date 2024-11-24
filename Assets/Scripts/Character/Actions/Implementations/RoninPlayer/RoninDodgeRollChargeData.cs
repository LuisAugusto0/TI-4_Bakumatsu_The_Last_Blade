using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable


public class RoninDodgeRollChargeData 
: ICancellableActionPrefabFactory<RoninPlayerBehaviourHandler>
{
    public float speedMultiplier = 1.2f;
    public float perfectDodgeWindow = 1f;
    public float cooldown = 1f;
    public int charges = 1;

    public override IManagedAction GetManagedAction(
        RoninPlayerBehaviourHandler target, OnActionEnded finishedCallback)
    {
        return new ManagedPersistantCooldownAction<ChargeCooldown<Character>>(
            target.character, GetAction(target, finishedCallback), GetCooldown(target.character)
        );
    }


    ChargeCooldown<Character> GetCooldown(Character target)
    {
        return new ChargeCooldown<Character>(target, charges, cooldown);
    }

    RoninDodgeRoll GetAction(RoninPlayerBehaviourHandler target, OnActionEnded finishedCallback)
    {
        return new RoninDodgeRoll(target, finishedCallback, speedMultiplier, perfectDodgeWindow);
    }
}




[Serializable]
public class RoninDodgeRoll : IPersistantAction
{
    public readonly RoninPlayerBehaviourHandler target;
    public readonly DirectionalMovement movement;
    public readonly OnActionEnded finishedCallback;
    public readonly Character character;
    
    public readonly float speedMultiplier;
    public readonly float perfectDodgeWindow;
    
    bool isActive;
    Vector2 moveVector = Vector2.zero;

    public RoninDodgeRoll(
        RoninPlayerBehaviourHandler target, 
        OnActionEnded callback, 
        float speedMultiplier, 
        float perfectDodgeWindow
    )
    {
        this.target = target;
        this.movement = target.movement;
        this.character = target.character;
        this.finishedCallback = callback;
        this.perfectDodgeWindow = perfectDodgeWindow;
        this.speedMultiplier = speedMultiplier;
        
    }

    
    public OnActionEnded GetOnActionEnded()
    {
        return finishedCallback;
    }

    public bool IsActive() => isActive;


    public void ActionStart()
    {
        Debug.Assert(
            !isActive, 
            "ActionStart() is not supposed to be called from managed actions while persistant action is active"
        );


        Vector2 dir = target.LastMoveInputVector == Vector2.zero ?
            movement.GetFacingDirectionVector2() : target.LastMoveInputVector.normalized;

        moveVector = dir * (speedMultiplier * movement.CurrentSpeed);

        target.actionAnimationEvent = OnRollAnimationEvent;
        target.animator.SetTrigger(RoninPlayerBehaviourHandler.rollTriggerHash);

        character.damageable.AddImmunity(this);
        character.StartActionLock(ForceCancel, this);
        
        character.damageable.onHit.AddListener(HitOnPerfectDodgeWindow);
        target.StartCoroutine(PerfectDashTime());

        isActive = true;
        target.StartCoroutine(DashRoutine());
    
    }

    void HitOnPerfectDodgeWindow(object source, Damageable damageable)
    {
        Debug.Log("PERFECT DODGE!!");
    }


    IEnumerator PerfectDashTime()
    {
        yield return new WaitForSeconds(perfectDodgeWindow);
        character.damageable.onHit.RemoveListener(HitOnPerfectDodgeWindow);
    }


    IEnumerator DashRoutine() 
    {
        while (isActive) 
        {
            movement.MoveTowardsMoveVector(moveVector);
            yield return new WaitForFixedUpdate();
        }
    }


    void OnRollAnimationEvent(int context)
    {
        switch(context) {
            case 0:
                //Starting to roll
                break;
            case 1:
                // Ended Roll
                break;
            case 2:
                // Ended Animation
                End();
                break;
            default:
                Debug.LogErrorFormat("");
                break;
        }   
    }



    // When animation truly finished
    void End()
    {
        character.damageable.RemoveImmunity(this);
        character.EndActionLock(this);
        target.actionAnimationEvent = null;
        finishedCallback.Invoke();
        isActive = false;
    }

    public void ForceCancel()
    {
        character.damageable.RemoveImmunity(this);
        End();
    }

    public bool AttemptCancel() => false;

}

