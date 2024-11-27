using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;







public class FacingTeleport : PlayerNonPersistantAction
{
   [Serializable]
    public class OnFiredEvent : UnityEvent<FacingTeleport>
    { }

    DirectionalMovement movement; //Updated when fetched
    bool isInitialized = false;

    public override void Initialize(BasePlayerBehaviour player) 
    {
        if (isInitialized) throw new InvalidOperationException("Cannot recall initialized");
        this.movement = player.directionalMovement;
        isInitialized = true;
    }

    public float distance = 2f;
    public OnFiredEvent fired = null;


    // For now its required even as empty
    public override bool IsInitialized() => isInitialized;
    public override void ForceCancel() {}
    public override bool AttemptCancel() => false;
    public override bool IsActive() => false;


    public override void ActionStart()
    {
        Debug.Assert(isInitialized, "Action must be intialized!!");

        Vector2 teleportVector = movement.LastMoveVector == Vector2.zero ? 
            movement.GetFacingDirectionVector2() : movement.LastMoveVector;
        
        movement.TeleportTowards(teleportVector);

        fired.Invoke(this);
        
    }
}