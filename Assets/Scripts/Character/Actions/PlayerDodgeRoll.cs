using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeRoll : PlayerAction
{
    public float speed;
    public Vector2 moveVector = Vector2.zero; 
   

    protected override void Perform(int context = 0)
    {
        playerController.onRollEvent = OnRollAnimationEvent;
        playerController.animator.SetTrigger(PlayerController.rollTriggerHash);
        
        if (character.LastMoveVector == Vector2.zero)
        {
            Vector2 dir = character.spriteRenderer.flipX ? Vector2.left : Vector2.right;
            moveVector = dir * speed;
        }
        else
        {
            moveVector = character.LastMoveVector.normalized * speed;
        }

        character.damageable.AddImmunity(this);
        character.StartActionLock(Cancel, this);
        StartCoroutine(DashRoutine());
    }


    IEnumerator DashRoutine() 
    {
        while (isActive) 
        {
            character.Move(moveVector);
            yield return new WaitForFixedUpdate();
        }
    }
    
    void OnRollAnimationEvent(int context)
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
        
    }

    void CollisionFrameEnd()
    {

    }

    // When animation truly finished
    protected override void End()
    {
        base.End();
        character.damageable.RemoveImmunity(this);
        character.EndActionLock(this);
    }

    void Cancel()
    {
        character.damageable.RemoveImmunity(this);
        End();
    }
}
