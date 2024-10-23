
using System;
using UnityEngine;
using UnityEngine.Events;


public class PlayerBaseAttack : PlayerCooldownAction
{
    [Serializable]
    public class OnAttackEvent : UnityEvent<PlayerBaseAttack>
    { }

    public TriggerDamager damagerForward;
    public TriggerDamager damagerUp;
    public TriggerDamager damagerDown;
    TriggerDamager currentDamager = null;
    
    
    [Tooltip("Event triggered when this action begins.")]
    public OnAttackEvent performed;

    [Tooltip("Event triggered when the slash attack starts (frames with collision ON).")]
    public OnAttackEvent slashStart;

    [Tooltip("Event triggered when the action has ended.")]
    public OnAttackEvent actionEnded;

    [Tooltip("Event triggered when the action is cancelled before it ends.")]
    public OnAttackEvent actionCancelled;


    void Awake()
    {
        character = playerController.character;

    }

    void Start()
    {
        damagerForward.DisableCollider();
        damagerUp.DisableCollider();
        damagerDown.DisableCollider();
    }


    protected override void Perform(int context = 0)
    {
        // Animation will be responsible for sending animation events
        playerController.animator.SetTrigger(PlayerController.baseAttackTriggerHash);
        EnableCollider(AnimatorGetFacingDirection.CurrentDirection);

        character.StartActionLock(Cancel, this);
        playerController.actionAnimationEvent = OnAnimationEventReceived;
        
        performed.Invoke(this);
    }

    void EnableCollider(AnimatorGetFacingDirection.Direction direction)
    {
        //Debug.Log(direction);
        switch (direction)
        {
            case AnimatorGetFacingDirection.Direction.Up:
                currentDamager = damagerUp;
                break;
            case AnimatorGetFacingDirection.Direction.Down:
                currentDamager = damagerDown;
                break;
            case AnimatorGetFacingDirection.Direction.Forward:
                currentDamager = damagerForward;
                break;
        }
        Debug.Log(direction);
    }



    void OnAnimationEventReceived(int context)
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

                // If animation is not cancelled, actionEnded is still invoked after cancel
                actionEnded.Invoke(this);
                break;
            default:
                Debug.LogError("Invalid context received on BasreAttack");
                break;
        }
    }


    // Methods called from Animation Events
    void CollisionFrameStart()
    {
        slashStart.Invoke(this);
        currentDamager.EnableCollider();
    }

    void CollisionFrameEnd()
    {
        currentDamager.DisableCollider();
    }

    
    protected override void End()
    {
        base.End();
        character.EndActionLock(this);
        playerController.actionAnimationEvent = null;
    }


    void Cancel()
    {
        // For now there is no force end to animation
        character.damageable.RemoveImmunity(this);
        currentDamager.DisableCollider();
        End();
        
        actionCancelled.Invoke(this);
    }
}
