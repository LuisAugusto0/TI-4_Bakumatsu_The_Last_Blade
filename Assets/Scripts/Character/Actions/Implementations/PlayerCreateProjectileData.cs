using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#nullable enable


public class PlayerCreateProjectileData 
: IActionPrefabFactory<AbstractPlayerBehaviourHandler>
{
    public GameObject? prefab;
    public Transform? spawnPos; //Use children to offset spawn pos from center
    public float rotationOffset;

    public int charges = 1;
    public float cooldown = 0f;
    
    public override IManagedAction GetManagedAction(AbstractPlayerBehaviourHandler target)
    {
        return new ManagedNonPersistantCooldownAction<ChargeCooldown<Character>>(
            target.character, GetAction(target), GetCooldown(target.character)
        );
    }
    

    public PlayerCreateProjectile GetAction(AbstractPlayerBehaviourHandler target)
    {
        if (prefab == null || spawnPos == null)
        {
            throw new NullReferenceException("Expected serialized references are null");
        }

        return new PlayerCreateProjectile(
            target, 
            prefab, 
            rotationOffset,
            target.transform
        );
    }

    ChargeCooldown<Character> GetCooldown(Character target)
    {
        return new ChargeCooldown<Character>(target, charges, cooldown);
    }
}


public class PlayerCreateProjectile : INonPersistantAction
{

    public readonly AbstractPlayerBehaviourHandler player;
    public readonly GameObject prefab;
    public readonly float rotationOffset;
    public readonly Transform spawnPos;

    public PlayerCreateProjectile(
        AbstractPlayerBehaviourHandler player,
        GameObject prefab, 
        float rotationOffset,
        Transform spawnPos
    )
    {
        this.player = player;
        this.prefab = prefab;
        this.rotationOffset = rotationOffset;
        this.spawnPos = spawnPos;
    }

    public void ForceCancel() {}
    public bool AttemptCancel() => false;
    public bool IsActive() => false;
    
    public void ActionStart()
    {
        Debug.Log("MAHOYY");
        float angle = Mathf.Atan2(player.LastPointDirection.y, player.LastPointDirection.x) * Mathf.Rad2Deg;

        angle += rotationOffset;
        
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        UnityEngine.Object.Instantiate(prefab, spawnPos.position, rotation);
    }

}


