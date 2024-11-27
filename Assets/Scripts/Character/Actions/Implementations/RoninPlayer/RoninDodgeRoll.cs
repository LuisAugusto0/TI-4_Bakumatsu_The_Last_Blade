using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[Serializable]
public class RoninDodgeRoll : RoninPlayerPersistantAction
{
    RoninPlayerBehaviour player;
    DirectionalMovement movement;
    Character character;
    OnActionEnded finishedCallback;
    bool isInitialized = false;

    public override void Initialize(RoninPlayerBehaviour player, OnActionEnded finished) 
    {
        if (isInitialized) throw new InvalidOperationException("Cannot recall initialized");
        this.player = player;
        this.movement = player.directionalMovement;
        this.character = player.character;
        this.finishedCallback = finished; 

        isInitialized = true;
    }

    public float speedMultiplier = 1.1f;
    public float perfectDodgeWindow = 0.15f;
    bool isActive = false;
    Vector2 moveVector = Vector2.zero;


    public override bool IsInitialized() => isInitialized;
    public override OnActionEnded GetOnActionEnded() => finishedCallback;
    public override bool IsActive() => isActive;


    public override void ActionStart()
    {
        Debug.Assert(isInitialized, "Action must be intialized!!");
        Debug.Assert(!isActive, "Cannot start action while active!");


        Vector2 dir = player.LastMoveInputVector == Vector2.zero ?
            movement.GetFacingDirectionVector2() : player.LastMoveInputVector.normalized;

        moveVector = dir * (speedMultiplier * movement.CurrentSpeed);

        player.actionAnimationEvent = OnRollAnimationEvent;
        player.animator.SetTrigger(RoninPlayerBehaviour.rollTriggerHash);

        character.damageable.AddImmunity(this);
        character.StartActionLock(ForceCancel, this);
        
        character.damageable.onHit.AddListener(HitOnPerfectDodgeWindow);
        player.StartCoroutine(PerfectDashTime());

        isActive = true;
        player.StartCoroutine(DashRoutine());
    
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
        player.actionAnimationEvent = null;
        finishedCallback.Invoke();
        isActive = false;
    }

    public override void ForceCancel()
    {
        character.damageable.RemoveImmunity(this);
        End();
    }

    public override bool AttemptCancel() => false;


}

