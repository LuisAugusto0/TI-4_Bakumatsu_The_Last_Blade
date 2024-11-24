using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
#nullable enable



public class DirectionDashData 
: ICancellableActionPrefabFactory<RoninPlayerBehaviourHandler>
{
    public float speedMultiplier = 1.2f;
    public float perfectDodgeWindow = 1f;
    public float duration = 2f;

    public int charges = 1;
    public float cooldown = 0f;


    public override IManagedAction GetManagedAction(
        RoninPlayerBehaviourHandler target, OnActionEnded finishedCallback
    )
    {
        var res = new ManagedPersistantCooldownAction<ChargeCooldown<Character>>(
            target.character, GetAction(target, finishedCallback), GetCooldown(target.character)
        );

        return res;

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




public class DirectionalDash : IPersistantAction
{
    [Serializable]
    public class OnDashEvent : UnityEvent<DirectionalDash>
    { }

    public readonly AbstractPlayerBehaviourHandler player;
    public readonly DirectionalMovement movement;
    public readonly Character character;
    
    public readonly float speed;
    public readonly float duration;

    readonly OnActionEnded finished;

    Vector2 moveVector = Vector2.zero; 
    Coroutine? dashCoroutine;
    Coroutine? timerCoroutine;
    bool isActive = false;

    public DirectionalDash(
        AbstractPlayerBehaviourHandler player, 
        float speed, 
        float duration,
        OnActionEnded finished
    )
    {
        this.player = player;
        character = player.character;
        movement = player.directionalMovement;

        this.speed = speed;
        this.duration = duration;
        this.finished = finished;
    }

    public bool IsActive() => isActive;
    public OnActionEnded GetOnActionEnded() => finished;

    public void ActionStart()
    {
        Debug.Assert(
            !isActive, 
            "ActionStart() is not supposed to be called from managed actions while persistant action is active"
        );

        moveVector = movement.LastMoveVector == Vector2.zero ? 
            movement.GetFacingDirectionVector2() : movement.LastMoveVector;

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

    public void ForceCancel()
    {
        if (timerCoroutine != null)
        {
            movement.StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        End();
    }

    // No implementations
    public bool AttemptCancel()
    { 
        ForceCancel();
        return true;
    }
}
