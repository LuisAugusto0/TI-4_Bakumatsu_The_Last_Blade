using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.WSA;





public class PlayerCreateProjectile : PlayerNonPersistantAction
{
    // Received from factory
    BasePlayerBehaviour player;
    bool isInitialized = false;

    public override void Initialize(BasePlayerBehaviour player) 
    {
        if (isInitialized) throw new InvalidOperationException("Cannot recall initialized");
        this.player = player;
        isInitialized = true;
    }

    public GameObject prefab;
    public float rotationOffset = 0;
    public Transform spawnPos;

    void Awake()
    {
        bool condition = spawnPos == null || prefab == null;
        Debug.Assert(!condition, "Values not assigned");
    }

    public override bool IsInitialized() => isInitialized;
    public override void ForceCancel() {}
    public override bool AttemptCancel() => false;
    public override bool IsActive() => false;
    
    public override void ActionStart()
    {
        Debug.Assert(isInitialized, "Action must be intialized!!");

        float angle = Mathf.Atan2(player.LastPointDirection.y, player.LastPointDirection.x) * Mathf.Rad2Deg;

        angle += rotationOffset;
        
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        UnityEngine.Object.Instantiate(prefab, spawnPos.position, rotation);
    }

}


