using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AnimationDash : PlayerDashBase
{
    public float speed = 5f;
    private Vector2 moveVector = Vector2.zero; 
    private bool canMove = false;

    protected override void Perform(int context = 0)
    {
        Debug.Log("Roll!");
        player.animator.SetBool(PlayerController.rollTriggerHash, true);
        moveVector = GetMoveVector();    
        player.onDash = true;
    }

    public override void FixedExecute() 
    {
        if (canMove)
        {
            player.Move(moveVector, speed);
        }

    }

    protected void ActionEnd()
    {
        player.onDash = false;
        player.isImmune = false;
    }

    
    public override void OnAnimationEvent(int context)
    {
        switch (context) {
            case 0:
                ImmunityFramesStart();
                break;
            case 1:
                ImmunityFramesEnd();
                break;
            case 2:
                AnimationEnd();
                break;

            default: throw new Exception("Unexpected context received");
        }
    }

    public void ImmunityFramesStart()
    {
        canMove = true;
        player.isImmune = true;
    }

    public void ImmunityFramesEnd()
    {
        canMove = false;
        player.isImmune = false;
        
    }

    public void AnimationEnd()
    {
        ActionEnd();

    }
}