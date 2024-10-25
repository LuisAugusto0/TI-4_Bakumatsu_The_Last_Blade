using UnityEngine;
using UnityEngine.Events;
using System;
using System.Runtime.CompilerServices;


public class FacingTeleport : IAction
{
    DirectionalMovement movement;
    
    [Serializable]
    public class OnFiredEvent : UnityEvent<FacingTeleport>
    { }

    [Tooltip("Event triggered when the skill is fired.")]
    public OnFiredEvent fired;

    public float distance;
    public override void StartAction()
    {
        Vector2 teleportVector = movement.LastMoveVector == Vector2.zero ? 
            movement.GetFacingDirection() : movement.LastMoveVector;
        
        movement.Teleport(teleportVector);

        fired.Invoke(this);
    }
}
