using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#nullable enable

public class FacingTeleportData 
: IActionPrefabFactory<RoninPlayerBehaviourHandler>
{
    public FacingTeleport.OnFiredEvent fired = new();
    public float distance;  

    public int charges = 1;
    public float cooldown = 0f;
    

    public override IManagedAction GetManagedAction(RoninPlayerBehaviourHandler target)
    {
        return new ManagedNonPersistantCooldownAction<ChargeCooldown<Character>>(
            target.character, GetAction(target), GetCooldown(target.character)
        );
    }

    public FacingTeleport GetAction(RoninPlayerBehaviourHandler target)
    {
        return new FacingTeleport(
            target.movement, 
            distance, 
            fired
        );
    }

    ChargeCooldown<Character> GetCooldown(Character target)
    {
        return new ChargeCooldown<Character>(target, charges, cooldown);
    }
}




public class FacingTeleport : INonPersistantAction
{
   [Serializable]
    public class OnFiredEvent : UnityEvent<FacingTeleport>
    { }

    // For now its required even as empty
    public void ForceCancel() {}
    public bool AttemptCancel() => false;
    public bool IsActive() => false;

    public readonly DirectionalMovement movement;
    public readonly float distance;
    public readonly OnFiredEvent fired;

    public FacingTeleport(DirectionalMovement movement, float distance, OnFiredEvent fired)
    {
        this.movement = movement;
        this.distance = distance;
        this.fired = fired;
    }

    public void ActionStart()
    {
        Vector2 teleportVector = movement.LastMoveVector == Vector2.zero ? 
            movement.GetFacingDirectionVector2() : movement.LastMoveVector;
        
        movement.TeleportTowards(teleportVector);

        fired.Invoke(this);
        
    }
}