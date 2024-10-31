using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SceneSingleton<CameraController>
{
    public static float s_CameraLayer = -10f; 

    // current singleton is inside main camera
    public Camera MainCamera { get{ return _mainCamera; } }

    [NonSerialized]
    Camera _mainCamera;

    AbstractPlayerBehaviourHandler player;
    Transform playerTransform;

    public override void Awake()
    {
        base.Awake();
        _mainCamera = GetComponent<Camera>();
        if (_mainCamera == null)
        {
            Debug.LogError("HJEREEE!@");
        }
    }

    void Start()
    {
        player = AbstractPlayerBehaviourHandler.ActivePlayer;
        Debug.Log(player.gameObject ? "transform" : "nopt transofrm");
        Debug.Log(player ? "player exits" : "palyer not exists");
        playerTransform = player.gameObject.transform;
    }

    void Update()
    {
        transform.position = GetNewPosition();
    }

    Vector3 GetNewPosition()
    {
        Vector2 playerPos = new Vector2(playerTransform.position.x, playerTransform.position.y);
        return new Vector3(playerPos.x, playerPos.y, s_CameraLayer);
    }
}


