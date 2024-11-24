using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#nullable enable


public class RoninBaseAttackData 
: ICancellableActionPrefabFactory<RoninPlayerBehaviourHandler>
{

    //Serialized data
    public CharacterTriggerDamager? damagerForward;
    public CharacterTriggerDamager? damagerUp;
    public CharacterTriggerDamager? damagerDown;

    public RoninBaseAttack.OnAttackEvent start = new();
    public RoninBaseAttack.OnAttackEvent collisionFrameStart = new();

    public override IManagedAction GetManagedAction(
        RoninPlayerBehaviourHandler target, OnActionEnded finishedCallback)
    {
  
        return new ManagedPersistantAction(
            target.character, GetAction(target, finishedCallback)
        );
    }


    public RoninBaseAttack GetAction(RoninPlayerBehaviourHandler target, OnActionEnded finishedCallback)
    {
        if (damagerForward == null || damagerUp == null || damagerDown == null)
        {
            throw new NullReferenceException("Expected serialized references are null");
        }

        CharacterDamage damage = target.character.damage;
        damagerForward.UpdateCharacterDamage(damage);
        damagerUp.UpdateCharacterDamage(damage);
        damagerDown.UpdateCharacterDamage(damage);

        return new RoninBaseAttack(
            target, 
            finishedCallback, 
            damagerForward, 
            damagerUp,
            damagerDown,
            start,
            collisionFrameStart
        );
    }
}


[Serializable]
public class RoninBaseAttack : IPersistantAction
{
    [Serializable]
    public class OnAttackEvent : UnityEvent<RoninBaseAttack>
    { }

    readonly RoninPlayerBehaviourHandler player;
    readonly Character character;

    public readonly TriggerDamager damagerForward;
    public readonly TriggerDamager damagerUp;
    public readonly TriggerDamager damagerDown;
    TriggerDamager? currentDamager = null;

    // Events passed from the class builder
    public readonly OnActionEnded finishedCallback;
    public readonly OnAttackEvent start;
    public readonly OnAttackEvent collisionFrameStart;
  
    // Control variables
    bool onRecoveryFrames = false;
    bool isActive = false; 


    public RoninBaseAttack(
        RoninPlayerBehaviourHandler player, 
        OnActionEnded callback, 
        TriggerDamager damagerForward, 
        TriggerDamager damagerUp,
        TriggerDamager damagerDown,
        OnAttackEvent start,
        OnAttackEvent collisionFrameStart
    )
    {
        this.player = player;
        this.character = player.character;

        this.finishedCallback = callback;
        this.start = start;
        this.collisionFrameStart = collisionFrameStart;


        this.damagerForward = damagerForward;
        this.damagerUp = damagerUp;
        this.damagerDown = damagerDown;

        // Ensure colliders are disabled
        damagerForward.DisableCollider();
        damagerUp.DisableCollider();
        damagerDown.DisableCollider();
    }
    
    public OnActionEnded GetOnActionEnded() => finishedCallback;
    public bool IsActive() => isActive;


    public void ActionStart()
    {
        Debug.Assert(
            !isActive, 
            "ActionStart() is not supposed to be called from managed actions while persistant action is active"
        );
       
        currentDamager = SelectDamager(player.movement.FacingDirection);
        if (currentDamager == null)
        {
            Debug.LogError("Could not get proper direction from facing direction enum. Cancelling attack");
            return;
        }

        // Animation will be responsible for sending animation events
        player.animator.SetTrigger(RoninPlayerBehaviourHandler.baseAttackTriggerHash);
        
        // Receive animation event values from player
        player.actionAnimationEvent = OnAnimationEventReceived;
        
        character.StartActionLock(ForceCancel, this);
        start.Invoke(this);

        isActive = true;
        
    }

    TriggerDamager? SelectDamager(AnimatorGetFacingDirection.Direction direction)
    {
        TriggerDamager? res;
        switch (direction)
        {
            case AnimatorGetFacingDirection.Direction.Up:
                res = damagerUp;
                break;
            case AnimatorGetFacingDirection.Direction.Down:
                res = damagerDown;
                break;
            case AnimatorGetFacingDirection.Direction.Forward:
                res = damagerForward;
                break;
            default:
                res = null;
                break;     
        }
        return res;
    }





    void OnAnimationEventReceived(int context)
    {
        if (isActive)
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
                    break;
                default:
                    Debug.LogError("Invalid context received on BaseAttack");
                    break;
            }
        }
    }


    // Methods called from Animation Events
    void CollisionFrameStart()
    {
        currentDamager!.EnableCollider();
        collisionFrameStart.Invoke(this);
    }

    void CollisionFrameEnd()
    {
        currentDamager!.DisableCollider();
        onRecoveryFrames = true;
    }

    

    void End()
    {
        character.EndActionLock(this);
        player.actionAnimationEvent = null;

        onRecoveryFrames = false;
        isActive = false;

        finishedCallback.Invoke();
    }
   



    public void ForceCancel()
    {
        character.damageable.RemoveImmunity(this);
        if (currentDamager != null)
        {
            currentDamager.DisableCollider();
        }
        End();
    }

    public bool AttemptCancel() => CancelOnRecoveryFrame();
    public bool CancelOnRecoveryFrame()
    {   
        bool b = false;
        if (onRecoveryFrames)
        {
            b = true;
            ForceCancel();
        }
        return b;
    }


}
