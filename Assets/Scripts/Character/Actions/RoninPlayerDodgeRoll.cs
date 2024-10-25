using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class RoninDodgeRoll : IAction
{
    [Serializable]
    public class OnRollEvent : UnityEvent<RoninDodgeRoll>
    { }

    public float speed;
    public float perfectDodgeTime = 0.1f; 
    
    public Vector2 moveVector = Vector2.zero; 
    public RoninPlayerBehaviourHandler player;
    public Character character;
    public DirectionalMovement movement;

    bool isActive = false;



    [Tooltip("Event triggered when this action begins.")]
    public OnRollEvent onActionStart;

    [Tooltip("Event triggered when the action has ended.")]
    public OnRollEvent onActionEnd;

    [Tooltip("Event triggered when the action is cancelled before it ends.")]
    public OnRollEvent onActionCancel;


    public override void StartAction(OnActionEnded callback)
    {
        finished = callback;

        Vector2 dir = player.LastMoveInputVector == Vector2.zero ?
            movement.GetFacingDirection() : player.LastMoveInputVector.normalized;

  
        moveVector = dir *  speed;
        player.actionAnimationEvent = OnRollAnimationEvent;
        player.animator.SetTrigger(RoninPlayerBehaviourHandler.rollTriggerHash);
        
        character.damageable.onHit.AddListener(HitOnPerfectDodgeWindow);


        character.damageable.AddImmunity(this);
        character.StartActionLock(Cancel, this);
        onActionStart.Invoke(this);

        isActive = true;
        StartCoroutine(DashRoutine());
        StartCoroutine(PerfectDashTime());
        
    }


    void HitOnPerfectDodgeWindow(GameObject source, Damageable damageable)
    {
        Debug.Log("PERFECT DODGE!!");
    }


    IEnumerator PerfectDashTime()
    {
        yield return new WaitForSeconds(perfectDodgeTime);
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
                onActionEnd.Invoke(this);
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
        finished.Invoke();
        isActive = false;
    }

    void Cancel()
    {
        character.damageable.RemoveImmunity(this);
        onActionCancel.Invoke(this);
        End();
    }

    
}
