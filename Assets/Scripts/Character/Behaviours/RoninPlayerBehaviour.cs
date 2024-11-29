using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(DirectionalMovement))] 
[RequireComponent(typeof(Character))] // Primarily for action locks
[RequireComponent(typeof(Animator))] // For changing animation states

public class RoninPlayerBehaviour : BasePlayerBehaviour
{
    [Serializable]
    public enum PlayerState
    {
        Default,
        AttackLock,
        DodgeLock,
        SkillLock,
        Dead
    }

    // Unity events
    [Serializable]
    public class OnDeathEnd : UnityEvent<BasePlayerBehaviour>
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

    [SerializeField]
    PlayerState states = PlayerState.Default;

    public RoninPlayerActionManager dodgeAction;
    public RoninPlayerActionManager attackAction; 
    public RoninPlayerActionManager skillAction;

    public Material FlameSwordMaterial;
    public Material BaseMaterial;

    public void OnAttackEnd()
    {

    }

    public OnDeathEnd onDeathEnd;

    [NonSerialized]
    public bool canDodgeCancel = false;


    public GameObject dodgeRollPrefab;
    public GameObject attackPrefab;
    public GameObject skillPrefab;

    protected override void Awake()
    {
        base.Awake();

        

        animator = GetComponent<Animator>();
        movement = GetComponent<DirectionalMovement>();
    }

    protected override void Start()
    {
        base.Start();
        dodgeAction = new RoninPlayerActionManager(
            this, character, gameObject.transform, OnAttackEnd
        );   
        dodgeAction.SetRoninPlayerCancellableAction(dodgeRollPrefab);

        attackAction = new RoninPlayerActionManager(
            this, character, gameObject.transform, OnAttackEnd
        );   
        attackAction.SetRoninPlayerCancellableAction(attackPrefab);

        skillAction = new RoninPlayerActionManager(
            this, character, gameObject.transform, OnAttackEnd
        );   
        skillAction.SetPlayerAction(skillPrefab);
    }

    void ExitStateLock()
    {
        states = PlayerState.Default;
    }





    public override void OnDodgeInputCancelled(InputAction.CallbackContext context) 
    {
        if (!character.IsActionLocked)
        {
            states = PlayerState.DodgeLock;
            dodgeAction.ManagedAction.Attempt();
        }
        else if (states == PlayerState.AttackLock)
        {
            if (attackAction.ManagedAction.AttemptCancel())
            {
                states = PlayerState.DodgeLock;
                dodgeAction.ManagedAction.Attempt();
            }

        } 
    }




    public override void OnAttackInputCancelled(InputAction.CallbackContext context) 
    {
        if (!character.IsActionLocked)
        {
            states = PlayerState.AttackLock;
            attackAction.ManagedAction.Attempt();
        }
        
    }


    
    public override void OnSkillInputPerformed(InputAction.CallbackContext context) 
    {
        if (!character.IsActionLocked)
        {
            states = PlayerState.SkillLock;
            skillAction.ManagedAction.Attempt();
        }
        
    }


    void Update()
    {
        if (!character.IsActionLocked)
        {
            animator.SetInteger(horizontalAxisHash, Mathf.RoundToInt(_moveInputVector.x));
            animator.SetInteger(verticalAxisHash, Mathf.RoundToInt(_moveInputVector.y));
            UpdateIdle();
            movement.UpdateRendererFlipOnMove();
        }
    }

    
    void FixedUpdate()
    {
        if (!character.IsActionLocked)
        {
            movement.MoveTowardsDirection(_moveInputVector);
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
        states = PlayerState.Dead;
    }

    // Subscribed to Death Animation
    public void OnDeathAnimationEnd()
    {
        GameOverManager.Instance.StartGameOver();
        onDeathEnd.Invoke(this);
    }

    public void OnActionAnimationEvent(int context)
    {
        if (actionAnimationEvent != null)
        actionAnimationEvent(context);
    }
}


