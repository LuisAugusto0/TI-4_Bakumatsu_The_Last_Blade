using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;


public class DirectionalDash : IAction
{
    [Serializable]
    public class OnDashEvent : UnityEvent<DirectionalDash>
    { }

    public AbstractPlayerBehaviourHandler player;
    public DirectionalMovement movement;
    public Character character;
    
    public float speed;
    public float duration;

    private float _lastStartTime;
    private Vector2 moveVector = Vector2.zero; 

    
    
    [Tooltip("Event triggered when this action begins.")]
    public OnDashEvent onActionStart;

    [Tooltip("Event triggered when the action has ended.")]
    public OnDashEvent onActionEnd;

    [Tooltip("Event triggered when the action is cancelled before it ends.")]
    public OnDashEvent onActionCancel;

    public override void StartAction(OnActionEnded callback)
    {
        finished = callback;
        
        _lastStartTime = Time.time;
        moveVector = movement.LastMoveVector == Vector2.zero ? 
            movement.GetFacingDirectionVector2() : movement.LastMoveVector;

        character.StartActionLock(Cancel, this);
        character.damageable.AddImmunity(this);

        StartCoroutine(DashRoutine());
    }

    public bool IsActionDone()
    {
        return Time.time > _lastStartTime + duration;
    }

    IEnumerator DashRoutine() 
    {
        while (!IsActionDone()) 
        {
            movement.MoveTowardsMoveVector(moveVector);
            yield return new WaitForFixedUpdate();
        }
     
        End();
    }

    void End()
    {
        character.EndActionLock(this);
        character.damageable.RemoveImmunity(this);
        finished.Invoke();
    }

    void Cancel()
    {
        End();
    }
}
