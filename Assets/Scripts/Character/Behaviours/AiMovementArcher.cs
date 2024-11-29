using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Character))]
[RequireComponent(typeof(PathFindingAlgorithmArcher))]
[RequireComponent(typeof(DirectionalMovement))]
public class AiMovementArcher : MonoBehaviour
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
    PathFindingAlgorithmArcher pathFinding;
    public PathFindingAlgorithmArcher GetTargetSearchAlgorithm {get {return pathFinding;}}

    public Coroutine updatePathCoroutine = null;
    public Coroutine followPathCoroutine = null;
    GameObject target;
    public GameObject Target {get {return target;}}
    DirectionalMovement movement;

    void Awake()
    {
        movement = GetComponent<DirectionalMovement>();
        pathFinding = GetComponent<PathFindingAlgorithmArcher>();
        pathFinding.SetMoveTowardsDelegate(Move);
        keepAwayRadius = 1;
        target = GameObject.FindGameObjectWithTag("Player");

    }

    void Start()
    {
        pathFinding.StartSeekingTarget(target.gameObject.transform);
        //SwitchState(currentState);
    }

    void Update()
    {
        currentDistance = Vector2.Distance(target.transform.position, gameObject.transform.position);
    }

    // Called by delegate of pathfinding
    void Move(Vector2 worldPoint)
    {
        if (!character.IsActionLocked)
        {
            movement.MoveTowardsPoint(worldPoint);
        }
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


    // Managing State value updates
    void StopState()
    {
        pathFinding.StopSeeking();
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
        pathFinding.StartSeekingTarget(target.gameObject.transform);
    }

    void Avoid()
    {

    }
    


 

}
