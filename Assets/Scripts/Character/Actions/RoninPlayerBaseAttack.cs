
using System;
using UnityEngine;
using UnityEngine.Events;


public class RoninBaseAttack : IAction
{
    [Serializable]
    public class OnAttackEvent : UnityEvent<RoninBaseAttack>
    { }

    public TriggerDamager damagerForward;
    public TriggerDamager damagerUp;
    public TriggerDamager damagerDown;
    TriggerDamager currentDamager = null;
    
    public RoninPlayerBehaviourHandler player;
    private Character character;
    
    [Tooltip("Event triggered when this action begins.")]
    public OnAttackEvent start;

    [Tooltip("Event triggered when the slash attack starts (frames with collision ON).")]
    public OnAttackEvent collisionFrameStart;


    bool onRecoveryFrames = false;
    bool onAction = false; //Stop animation events when cancelled


    void Start()
    {
        character = player.character;
        damagerForward.DisableCollider();
        damagerUp.DisableCollider();
        damagerDown.DisableCollider();
    }


    public override void StartAction(OnActionEnded callback)
    {
        finished = callback;
        onAction = true;

        // Animation will be responsible for sending animation events
        player.animator.SetTrigger(RoninPlayerBehaviourHandler.baseAttackTriggerHash);
        SelectDamager(player.movement.FacingDirection);

        character.StartActionLock(Cancel, this);
        player.actionAnimationEvent = OnAnimationEventReceived;
        
        start.Invoke(this);
    }

    void SelectDamager(AnimatorGetFacingDirection.Direction direction)
    {

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
        

    }



    void OnAnimationEventReceived(int context)
    {
        if (onAction)
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
                    Debug.LogError("Invalid context received on BasreAttack");
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
        onAction = false;

        finished.Invoke();
    }
   



    void Cancel()
    {
        character.damageable.RemoveImmunity(this);
        currentDamager.DisableCollider();
        End();
    }


    // Force cancel when on recovery frame
    public override bool AttemptCancelAction()
    {   
        bool b = false;
        if (onRecoveryFrames)
        {
            b = true;
            Cancel();
        }
        return b;
    }

    
}
