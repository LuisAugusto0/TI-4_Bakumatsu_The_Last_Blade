using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[Serializable]
public class RoninBaseAttack : RoninPlayerPersistantAction
{
    [Serializable]
    public class OnAttackEvent : UnityEvent<RoninBaseAttack>
    { }

    RoninPlayerBehaviour player;
    Character character;
    OnActionEnded finishedCallback;
    bool isInitialized = false;

    public override void Initialize(RoninPlayerBehaviour player, OnActionEnded finished) 
    {
        if (isInitialized) throw new InvalidOperationException("Cannot recall initialized");
        this.player = player;
        this.character = player.character;
        this.finishedCallback = finished; 


        isInitialized = true;
    }


    public CharacterTriggerDamager damagerForward;
    public CharacterTriggerDamager damagerUp;
    public CharacterTriggerDamager damagerDown;
    CharacterTriggerDamager currentDamager = null;

    public OnAttackEvent start;
    public OnAttackEvent collisionFrameStart;
      
    bool onRecoveryFrames = false;
    bool isActive = false; 


    void Start()
    {
        damagerForward.DisableCollider();
        damagerUp.DisableCollider();
        damagerDown.DisableCollider();

        damagerForward.Initialize(character, character.damage);
        damagerUp.Initialize(character, character.damage);
        damagerDown.Initialize(character, character.damage);
    }


    public override bool IsInitialized() => isInitialized;
    public override OnActionEnded GetOnActionEnded() => finishedCallback;
    public override bool IsActive() => isActive;


    public override void ActionStart()
    {
        Debug.Assert(isInitialized, "Action must be intialized!!");
        Debug.Assert(!isActive, "Cannot start action while active!");
       
        currentDamager = SelectDamager(player.movement.FacingDirection);
        if (currentDamager == null)
        {
            Debug.LogError("Could not get proper direction from facing direction enum. Cancelling attack");
            return;
        }

        // Animation will be responsible for sending animation events
        player.animator.SetTrigger(RoninPlayerBehaviour.baseAttackTriggerHash);
        
        // Receive animation event values from player
        player.actionAnimationEvent = OnAnimationEventReceived;
        
        character.StartActionLock(ForceCancel, this);
        start.Invoke(this);

        isActive = true;
        
    }

    CharacterTriggerDamager SelectDamager(AnimatorGetFacingDirection.Direction direction)
    {
        CharacterTriggerDamager res;
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
        currentDamager.EnableCollider();
        collisionFrameStart.Invoke(this);
    }

    void CollisionFrameEnd()
    {
        currentDamager.DisableCollider();
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
   



    public override void ForceCancel()
    {
        character.damageable.RemoveImmunity(this);
        if (currentDamager != null)
        {
            currentDamager.DisableCollider();
        }
        End();
    }

    public override bool AttemptCancel() => CancelOnRecoveryFrame();
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
