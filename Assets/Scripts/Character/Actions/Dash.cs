using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;


public class Dash : CharacterCooldownAction
{
    [Serializable]
    public class OnDashEvent : UnityEvent<PlayerDodgeRoll>
    { }

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

    protected override void Perform(int context = 0)
    {
        _lastStartTime = Time.time;
        if (character.LastMoveVector == Vector2.zero)
        {
            Vector2 dir = character.spriteRenderer.flipX ? Vector2.left : Vector2.left;
            moveVector = dir * speed;
        }
        else
        {
            moveVector = character.LastMoveVector.normalized * speed;
        }

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
            character.Move(moveVector);
            yield return new WaitForFixedUpdate();
        }
     
        End();
    }

    protected override void End()
    {
        base.End();
        character.EndActionLock(this);
        character.damageable.RemoveImmunity(this);
    }

    void Cancel()
    {
        End();
    }
}
