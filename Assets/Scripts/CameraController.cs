using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static float s_CameraLayer = -10f; 
    // Static management for unique CameraController
    public static CameraController Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            Create ();

            return s_Instance;
        }
    }
    protected static CameraController s_Instance;


    public static void Create ()
    {
        CameraController controllerPrefab = Resources.Load<CameraController> ("MainCamera");
        s_Instance = Instantiate (controllerPrefab);
    }



    PlayerController player;
    Transform playerTransform;

    void Awake()
    {
        s_Instance = this;
    }

    void Start()
    {
        player = PlayerController.ActivePlayer;
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


