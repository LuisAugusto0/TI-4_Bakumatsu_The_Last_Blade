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

    BasePlayerBehaviour player;
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
        player = BasePlayerBehaviour.ActivePlayer;
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


