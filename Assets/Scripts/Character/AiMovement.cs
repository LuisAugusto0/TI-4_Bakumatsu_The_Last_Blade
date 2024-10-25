using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Character))]
[RequireComponent(typeof(TargetSearchAlgorithm))]
public class AiMovement : MonoBehaviour
{
    // Ai will not approach closer than this at all. 
    public float minimumApproachableDistance = 0.5f;

    // When not attacking, enemy will try to atleast get a 
    // distance further than this radius 
    public float keepAwayRadius = 1f;

    // Enemy will always try to keep this distance
    public float minimumInteractionRadius = 2f;

    public enum MovementState
    {
        // Complete stop
        Stop, 

        // Moving randomly until finds target
        Roaming, 

        // Maintain Surround distance:
        //  if further than minimum interaction distance; move closer from target
        //  if closer than keep away distance; move further from target
        //  if between both, move on a circle / parabole
        //
        // If the enemy is a ranged type with a big attack radius, 
        // you can make the surround distance the same; in this way the ai will
        // always look to be on a shooting position
        Surround, 

        // Heads straight towards target until minimum approachable distance
        Attack,

        // Heads the opposite direction to target
        Escape,
    }

    [SerializeField]
    MovementState currentState = MovementState.Attack;
    public float CurrentDistance { get {return currentDistance;} }
    float currentDistance = 0;

    public Character character;
    public Animator animator;
    TargetSearchAlgorithm targetSearchAlgorithm;
    public TargetSearchAlgorithm GetTargetSearchAlgorithm {get {return targetSearchAlgorithm;}}

    public Coroutine updatePathCoroutine = null;
    public Coroutine followPathCoroutine = null;
    GameObject player;

    void Awake()
    {
        targetSearchAlgorithm = GetComponent<TargetSearchAlgorithm>();
        keepAwayRadius = 1;
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Start()
    {
        targetSearchAlgorithm.SetTargetTransform(player.gameObject.transform);
        EnablePathFindingCoroutines();
        //SwitchState(currentState);
    }

    void Update()
    {
        currentDistance = Vector2.Distance(player.transform.position, gameObject.transform.position);
    }


    // void FixedUpdate()
    // {
    //     if (!character.IsActionLocked)
    //     {
    //         switch (currentState) {
    //             case MovementState.Stop:
    //                 StopState();
    //                 break;

    //             case MovementState.Roaming:
    //                 Roaming();
    //                 break;

    //             case MovementState.Surround:
    //                 Surround();
    //                 break;

    //             case MovementState.Attack:
    //                 AttackTarget();
    //                 break;

    //             case MovementState.Escape:
    //                 Avoid();
    //                 break;
    //         }
    //     }
    // }

    public void SwitchState(MovementState state)
    {
        switch (state) {
            case MovementState.Stop:
                StopState();
                break;

            case MovementState.Roaming:
                Roaming();
                break;

            case MovementState.Surround:
                Surround();
                break;

            case MovementState.Attack:
                AttackTarget();
                break;

            case MovementState.Escape:
                Avoid();
                break;
        }
    }

    // Handling Coroutines
    void DisablePathFindingCoroutines()
    {
        if (followPathCoroutine != null)
        {
            StopCoroutine(followPathCoroutine);
            StopCoroutine(updatePathCoroutine);
            followPathCoroutine = null;
            updatePathCoroutine = null;
        }
    }

    void EnablePathFindingCoroutines()
    {
        Debug.Log("Enabled");
        if (followPathCoroutine == null)
        {
            followPathCoroutine = StartCoroutine(targetSearchAlgorithm.UpdatePathForMovingTarget());
            updatePathCoroutine = StartCoroutine(targetSearchAlgorithm.MoveAlongPath());
        }
    }

    // Managing State value updates
    void StopState()
    {
        DisablePathFindingCoroutines();
        currentState = MovementState.Stop;
    }

    void Roaming()
    {

    }

    void Surround()
    {

    }

    void AttackTarget()
    {
        targetSearchAlgorithm.SetTargetTransform(player.gameObject.transform);
        EnablePathFindingCoroutines();
    }

    void Avoid()
    {

    }
    


 

}
