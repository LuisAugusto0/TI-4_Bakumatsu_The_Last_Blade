
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
    public OnAttackEvent performed;

    [Tooltip("Event triggered when the slash attack starts (frames with collision ON).")]
    public OnAttackEvent slashStart;

    [Tooltip("Event triggered when the action has ended.")]
    public OnAttackEvent actionEnded;

    [Tooltip("Event triggered when the action is cancelled before it ends.")]
    public OnAttackEvent actionCancelled;

    bool onRecoveryFrames = false;

    void Awake()
    {
        character = player.character;
    }

    void Start()
    {
        damagerForward.DisableCollider();
        damagerUp.DisableCollider();
        damagerDown.DisableCollider();
    }


    public override void StartAction(OnActionEnded callback)
    {
        finished = callback;

        // Animation will be responsible for sending animation events
        player.animator.SetTrigger(RoninPlayerBehaviourHandler.baseAttackTriggerHash);
        SelectDamager(player.FacingDirection);

        character.StartActionLock(Cancel, this);
        player.actionAnimationEvent = OnAnimationEventReceived;
        
        performed.Invoke(this);
    }

    void SelectDamager(AnimatorGetFacingDirection.Direction direction)
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
        onRecoveryFrames = true;
    }

    
    protected void End()
    {
        character.EndActionLock(this);
        player.actionAnimationEvent = null;

        onRecoveryFrames = false;
        finished.Invoke();
    }
   
    void Cancel()
    {
        // For now there is no force end to animation
        character.damageable.RemoveImmunity(this);
        currentDamager.DisableCollider();
        End();
        
        actionCancelled.Invoke(this);
    }


    // Force cancel when on recovery frame
    public bool CancelOnRecoveryFrame()
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
