using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerDodgeRoll : PlayerCooldownAction
{
    [Serializable]
    public class OnRollEvent : UnityEvent<PlayerDodgeRoll>
    { }

    public float speed;
    public Vector2 moveVector = Vector2.zero; 
    
    [Tooltip("Event triggered when this action begins.")]
    public OnRollEvent onActionStart;

    [Tooltip("Event triggered when the action has ended.")]
    public OnRollEvent onActionEnd;

    [Tooltip("Event triggered when the action is cancelled before it ends.")]
    public OnRollEvent onActionCancel;

    protected override void Perform(int context = 0)
    {
        playerController.actionAnimationEvent = OnRollAnimationEvent;
        playerController.animator.SetTrigger(PlayerController.rollTriggerHash);
        
        if (character.LastMoveVector == Vector2.zero)
        {
            Vector2 dir = character.spriteRenderer.flipX ? Vector2.left : Vector2.right;
            moveVector = dir * speed;
        }
        else
        {
            moveVector = character.LastMoveVector.normalized * speed;
        }

        character.damageable.AddImmunity(this);
        character.StartActionLock(Cancel, this);
        onActionStart.Invoke(this);
        StartCoroutine(DashRoutine());
    }


    IEnumerator DashRoutine() 
    {
        while (isActive) 
        {
            character.Move(moveVector);
            yield return new WaitForFixedUpdate();
        }
    }
    
    void OnRollAnimationEvent(int context)
    {
        switch(context) {
            case 0:
                CollisionFrameStart();
                break;
            case 1:
                CollisionFrameEnd();
                break;
            case 2:
                End();
                onActionEnd.Invoke(this);
                break;
            default:
                Debug.LogErrorFormat("");
                break;
        }   
    }

    void CollisionFrameStart()
    {
        
    }

    void CollisionFrameEnd()
    {

    }

    // When animation truly finished
    protected override void End()
    {
        base.End();
        character.damageable.RemoveImmunity(this);
        character.EndActionLock(this);
        playerController.actionAnimationEvent = null;
    }

    void Cancel()
    {
        character.damageable.RemoveImmunity(this);
        onActionCancel.Invoke(this);
        End();
    }
}
