using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;





public class DirectionalDash : PlayerPersistantAction
{
    [Serializable]
    public class OnDashEvent : UnityEvent<DirectionalDash>
    { }

    BasePlayerBehaviour player;
    DirectionalMovement movement;
    Character character;
    OnActionEnded finished;
    bool isInitialized = false;

    public override void Initialize(BasePlayerBehaviour player, OnActionEnded finished) 
    {
        if (isInitialized) throw new InvalidOperationException("Cannot recall initialized");
        this.player = player;
        this.movement = player.directionalMovement;
        this.character = player.character;
        this.finished = finished; 

        isInitialized = true;
    }


    public float speedMultiplier;
    public float duration;
    
    Vector2 moveVector = Vector2.zero; 
    Coroutine dashCoroutine;
    Coroutine timerCoroutine;
    bool isActive = false;



    public override bool IsInitialized() => isInitialized;
    public override bool IsActive() => isActive;
    public override OnActionEnded GetOnActionEnded() => finished;

    public override void ActionStart()
    {
        Debug.Assert(isInitialized, "Action must be intialized!!");
        Debug.Assert(!isActive, "Cannot start action while active!");

        Vector2 dir = movement.LastMoveVector == Vector2.zero ? 
            movement.GetFacingDirectionVector2() : movement.LastMoveVector;

        moveVector = dir * (speedMultiplier * movement.CurrentSpeed);

        character.StartActionLock(ForceCancel, this);
        character.damageable.AddImmunity(this);

        dashCoroutine = movement.StartCoroutine(DashRoutine());
        timerCoroutine = movement.StartCoroutine(TimerRoutine());
        isActive = true;
    }


    IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(duration);
        timerCoroutine = null;
        End();
    }

    IEnumerator DashRoutine() 
    {
        while (true) 
        {
            movement.MoveTowardsMoveVector(moveVector);
            yield return new WaitForFixedUpdate();
        }
    }

    void End()
    {
        movement.StopCoroutine(dashCoroutine);
        dashCoroutine = null;

        character.EndActionLock(this);
        character.damageable.RemoveImmunity(this);
        finished.Invoke();
        isActive = true;
    }

    public override void ForceCancel()
    {
        if (timerCoroutine != null)
        {
            movement.StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        End();
    }

    // No implementations
    public override bool AttemptCancel()
    { 
        ForceCancel();
        return true;
    }
}
