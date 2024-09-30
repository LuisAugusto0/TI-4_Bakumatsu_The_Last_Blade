using System;
using UnityEngine;

public class BasicDash : PlayerDashBase
{

    public float speed;
    public float duration;

    // Helper variables 
    [NonSerialized]
    private Vector2 moveVector = Vector2.zero; 


    protected override void Perform(int context = 0)
    {
        moveVector = GetMoveVector();
        player.onDash = true;
        player.isImmune = true;
    }

    public bool isActionDone()
    {
        return Time.time > lastActivatedTime + duration;
    }

    public override void FixedExecute() 
    {
        if (!isActionDone()) 
        {
            player.Move(moveVector, speed);

        }
        else
        {
            ActionEnd();
        }

    }

    protected void ActionEnd()
    {
        player.onDash = false;
        player.isImmune = false;
    }

    public override void Cancel()
    {
        ActionEnd();   
    }
}
