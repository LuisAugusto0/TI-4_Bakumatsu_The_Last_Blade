using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(Character))] // Primarily for action locks
[RequireComponent(typeof(Animator))] // For changing animation states
[RequireComponent(typeof(DirectionalMovement))]
public class RoninPlayerBehaviourHandler : AbstractPlayerBehaviourHandler
{
    [Serializable]
    public enum States
    {
        Default,
        Dead
    }

    // Unity events
    [Serializable]
    public class OnDeathEnd : UnityEvent<AbstractPlayerBehaviourHandler>
    { }

    // Delegates to transmit values received from animation events to the actions
    public delegate void ActionAnimationEvent(int context);    
    

    [NonSerialized]
    public ActionAnimationEvent actionAnimationEvent;


    // Animator clips
    static readonly public int moveHash = Animator.StringToHash("Moving");
    static readonly public int deadTriggerHash = Animator.StringToHash("Dead");
    static readonly public int verticalAxisHash = Animator.StringToHash("VerticalAxis");
    static readonly public int horizontalAxisHash = Animator.StringToHash("HorizontalAxis");

    // Actions
    static readonly public int baseAttackTriggerHash = Animator.StringToHash("BaseAttack");
    static readonly public int rollTriggerHash = Animator.StringToHash("Roll");
    static readonly public int groundSlamTriggerHash = Animator.StringToHash("GroundSlam");
    
    [NonSerialized]
    public Animator animator;

    [NonSerialized]
    public DirectionalMovement movement;

    AnimatorGetFacingDirection.Direction facingDirection = AnimatorGetFacingDirection.Direction.Forward;
    public AnimatorGetFacingDirection.Direction FacingDirection {get{return facingDirection;}}

    public States states = States.Default;

      

    public CooldownCallback dodgeAction;   
    public IAction dodge;

    public CooldownCallback attackAction;   
    public IAction attack;

    public CooldownCallback skillAction;
    public IAction skill;

    public OnDeathEnd onDeathEnd;


    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        movement = GetComponent<DirectionalMovement>();
        AnimatorGetFacingDirection.AssignDelegatesToAnimator(animator, (ctx) => {facingDirection = ctx;});
        attackAction = new(attack);
        dodgeAction = new(dodge);
        skillAction = new(skill);
    }


    public override void OnDodgeInputCancelled(InputAction.CallbackContext context) 
    {
        if (!character.IsActionLocked)
        {
            dodgeAction.Attempt();
        }
    }


    public override void OnAttackInputCancelled(InputAction.CallbackContext context) 
    {
        if (!character.IsActionLocked)
        {
            attackAction.Attempt();
        }
        
    }

    
    public override void OnSkillInputPerformed(InputAction.CallbackContext context) 
    {
        if (!character.IsActionLocked)
        {
            skillAction.Attempt();
        }
        
    }


    void Update()
    {
        if (!character.IsActionLocked)
        {
            animator.SetInteger(horizontalAxisHash, Mathf.RoundToInt(_moveInputVector.x));
            animator.SetInteger(verticalAxisHash, Mathf.RoundToInt(_moveInputVector.y));
            UpdateIdle();

            movement.Move(_moveInputVector * movement.moveSpeed);
            Debug.Log("HEREE!");
        }
    }

    float idleDelayTime = 0.1f;
    float lastStopTime = 0f;
    public void UpdateIdle()
    {
        if (_moveInputVector != Vector2.zero)
        {
            animator.SetBool(moveHash, true);
            lastStopTime = Time.time;
        }
        else if (Time.time > idleDelayTime + lastStopTime)
        {
            animator.SetBool(moveHash, false);
        } 
    }



    // Subscribed to Damageable OnDeath event
    public void OnDeath()
    {
        animator.SetTrigger(deadTriggerHash);
        character.DisableCharacter();
        states = States.Dead;
    }

    // Subscribed to Death Animation
    public void OnDeathAnimationEnd()
    {
        GameOverManager.Instance.StartGameOver();
        onDeathEnd.Invoke(this);
    }

    public void OnActionAnimationEvent(int context)
    {
        actionAnimationEvent(context);
    }



}


