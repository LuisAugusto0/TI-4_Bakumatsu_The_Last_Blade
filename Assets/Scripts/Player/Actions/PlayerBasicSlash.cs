using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicSlash : PlayerSlashBase
{
    private float lastSlashEndTime = 0f;

    protected override void Perform(int context = 0)
    {
        player.onSlash = true;
        player.animator.SetTrigger(PlayerController.slashTriggerHash);
    }


    public override void OnAnimationEvent(int context = 0)
    {
        switch(context) {
            case 0:
                CollisionFrameStart();
                break;
            case 1:
                CollisionFrameEnd();
                break;
            case 2:
                AnimationEnd();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(context), 
                    "Invalid context received: " + context
                );
        }   
    }
    // Scripts called by Animation Events
    public void CollisionFrameStart()
    {
        slashCollider.enabled = true;
    }

    public void CollisionFrameEnd()
    {
        slashCollider.enabled = false;
    }

    // When animation truly finished
    public void AnimationEnd()
    {
        lastSlashEndTime = Time.time;
        player.onSlash = false;
    }
}
