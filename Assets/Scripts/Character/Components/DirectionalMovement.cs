using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // From EntityMovement
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
public class DirectionalMovement : EntityMovement
{
    Character character;
    Animator animator;
    AnimatorGetFacingDirection.Direction facingDirection = AnimatorGetFacingDirection.Direction.Forward;
    public AnimatorGetFacingDirection.Direction FacingDirection {get{return facingDirection;}}

    protected override void Awake()
    {
        base.Awake();
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
        AnimatorGetFacingDirection.AssignDelegatesToAnimator(animator, (ctx) => {facingDirection = ctx; Debug.Log(facingDirection);});
    }

    
    public void UpdateFacingDirection()
    {
        UpdateFacingDirection(lastMoveVector);
    }
    
    public void UpdateFacingDirection(Vector2 moveInputVector)
    {
        if (facingDirection == AnimatorGetFacingDirection.Direction.Forward)
        {
            //  Debug.Log(facingDirection);
            if (moveInputVector != Vector2.zero)
            {
                character.FlipX(moveInputVector.x < 0);
            }
        }
        else
        {
           character.FlipX(false);
            
        }
    }
    

    public Vector2 GetFacingDirectionVector2()
    {
        Vector2 dir = Vector2.zero;
  
        switch (facingDirection)
        {   
            case AnimatorGetFacingDirection.Direction.Forward:
                dir = character.mainSpriteRenderer.flipX ? Vector2.left : Vector2.right;
                break;
            case AnimatorGetFacingDirection.Direction.Down:
                dir = new Vector2(0, -1);
                break;
            case AnimatorGetFacingDirection.Direction.Up: 
                dir = new Vector2(0, 1);
                break;
        }
        
        return dir;
    }

}