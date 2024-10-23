using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatorGetFacingDirection : StateMachineBehaviour 
{
    // Define enums for direction and state
    public enum Direction
    {
        Up = 1,
        Forward = 0,
        Down = -1
    }

    protected static Direction _currentDirection = Direction.Forward; //default direction
    public static Direction CurrentDirection { get{return _currentDirection; }}

    public delegate int GetDirectionDelegate();
    protected GetDirectionDelegate getFacingDirection;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetDirection(animator, stateInfo);
        getFacingDirection?.Invoke();
    }

    public int GetCurrentDirection()
    {
        return (int)_currentDirection; 
    }

    protected abstract void SetDirection(Animator animator, AnimatorStateInfo stateInfo);
}


