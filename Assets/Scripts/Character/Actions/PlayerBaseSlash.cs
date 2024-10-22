using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseSlash : PlayerAction
{
    public TriggerDamager damager;
    protected override void Perform(int context = 0)
    {
        playerController.onSlashEvent = OnSlashAnimationEvent;
        playerController.animator.SetTrigger(PlayerController.slashTriggerHash);
        character.StartActionLock(Cancel, this);
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
        damager.EnableCollider();
    }

    void CollisionFrameEnd()
    {
        character.damageable.RemoveImmunity(this);
        damager.DisableCollider();
    }

    protected override void End()
    {
        base.End();
        character.EndActionLock(this);
    }

    void Cancel()
    {
        character.damageable.RemoveImmunity(this);
        damager.enabled = false;
    }
}
