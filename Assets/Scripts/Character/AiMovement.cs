using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Character))]
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

    public MovementState currentState = MovementState.Attack;
    public float CurrentDistance { get {return currentDistance;} }
    float currentDistance = 0;

    public Character character;
    public Animator animator;
    private GameObject _target;


    void Awake()
    {
        keepAwayRadius = 1;
        _target = GameObject.FindGameObjectWithTag("Player");
    }


    void FixedUpdate()
    {
        currentDistance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.y), 
            new Vector2(_target.transform.position.x, _target.transform.position.y)
        );

   
        if (!character.IsActionLocked)
        {
            Vector2 direction = GetFacingDirection();

            switch (currentState) {
                case MovementState.Stop:
                    break;

                case MovementState.Roaming:
                    // Not implemented
                    break;

                case MovementState.Surround:
                    //Not implemented
                    break;

                case MovementState.Attack:
                    AttackTarget(direction);
                    break;

                case MovementState.Escape:
                    Avoid(direction);
                    break;
            }
        }
    }

    public Vector2 GetFacingDirection()
    {
        return (_target.transform.position - transform.position).normalized;
    }


    void MoveTowards(Vector2 direction)
    {
        character.Move(direction * character.moveSpeed);
    }

    void Avoid(Vector2 direction)
    {
        character.Move(-direction * character.moveSpeed);
    }
    

    void AttackTarget(Vector2 direction)
    {

        if (currentDistance > minimumApproachableDistance)
        {
            MoveTowards(direction);
        } 
        else if (currentDistance < minimumInteractionRadius)
        {
            Avoid(direction);
        }
     
    }

 

}
