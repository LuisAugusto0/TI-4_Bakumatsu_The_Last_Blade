using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseSlash : PlayerAction
{
    public Damager damager;
    protected override void Perform(int context = 0)
    {
        playerController.onSlashEvent = OnSlashAnimationEvent;
        playerController.animator.SetTrigger(PlayerController.slashTriggerHash);
        character.StartCancellableActionLock(Cancel);
        character.isActionLocked = true;
    }

    void OnSlashAnimationEvent(int context)
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
                Debug.LogErrorFormat("");
                break;
        }   
    }
   
    void CollisionFrameStart()
    {
        character.damageable.AddImmunity(this);
        damager.enabled = true;
    }

    void CollisionFrameEnd()
    {
        character.damageable.RemoveImmunity(this);
        damager.enabled = false;
    }

    protected override void End()
    {
        base.End();
        character.EndCancellableActionLock();
    }

    void Cancel()
    {
        character.damageable.RemoveImmunity(this);
        damager.enabled = false;
        End();
    }
}
