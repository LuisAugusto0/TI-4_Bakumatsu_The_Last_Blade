using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetFacingDirection : AnimatorGetFacingDirection
{

    // Hash codes for states
    private static readonly int IdleUpHash = Animator.StringToHash("BaseLayer.Up.Idle");
    private static readonly int IdleForwardHash = Animator.StringToHash("BaseLayer.Forward.Idle");
    private static readonly int IdleDownHash = Animator.StringToHash("BaseLayer.Down.Idle");
    private static readonly int MoveUpHash = Animator.StringToHash("BaseLayer.Up.Walk");
    private static readonly int MoveForwardHash = Animator.StringToHash("BaseLayer.Forward.Walk");
    private static readonly int MoveDownHash = Animator.StringToHash("BaseLayer.Down.Walk");


    protected override void SetDirection(Animator animator, AnimatorStateInfo stateInfo)
    {
        // Check which state is currently active and set the direction
        if (stateInfo.fullPathHash == IdleUpHash || stateInfo.fullPathHash == MoveUpHash)
        {
            _currentDirection = Direction.Up;
          
        }

        else if (stateInfo.fullPathHash == IdleForwardHash || stateInfo.fullPathHash == MoveForwardHash)
        {
            _currentDirection = Direction.Forward;
        
        }

        else if (stateInfo.fullPathHash == IdleDownHash || stateInfo.fullPathHash == MoveDownHash)
        {
            _currentDirection = Direction.Down;
            
        }

        else
        {
            Debug.LogWarning("None of the assigned values match");
        }


        Debug.Log(_currentDirection);

    }


}