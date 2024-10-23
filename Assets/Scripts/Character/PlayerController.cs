using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;



[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // Static management Player
    // Player must always exist on the scene otherwise other components will break
    // Not a singleton, as more than one player may exist
    public static PlayerController ActivePlayer { get { return s_ActivePlayer; } }
    protected static PlayerController s_ActivePlayer;


    // Unity events
    [Serializable]
    public class OnDeathEnd : UnityEvent<PlayerController>
    { }


    // Animator clips
    static readonly public int moveHash = Animator.StringToHash("Moving");
    static readonly public int deadTriggerHash = Animator.StringToHash("Dead");
    static readonly public int verticalAxisHash = Animator.StringToHash("VerticalAxis");
    static readonly public int horizontalAxisHash = Animator.StringToHash("HorizontalAxis");

    // Actions
    static readonly public int baseAttackTriggerHash = Animator.StringToHash("BaseAttack");
    static readonly public int rollTriggerHash = Animator.StringToHash("Roll");
    static readonly public int groundSlamTriggerHash = Animator.StringToHash("GroundSlam");
    
    public Animator animator;

    [NonSerialized]
    public PlayerGetFacingDirection animatorFacingDirection;
    


    // Character data
    public Character character;

    // Handle when to swap from moving to idle
    public float toIdleCooldown;
    private float _lastStopTime; 
    private bool _onIdleAnimation = true;
    
    // Event called exactly when death anim ends
    public OnDeathEnd onDeathEnd;
    public Vector2 pointDirectionVector = Vector2.zero;
    Vector2 _moveInputVector = Vector2.zero;

    // Create singleton
    public Camera mainCamera;


    // Action
    public PlayerBaseAttack baseAttack;
    public CharacterCooldownAction dodgeAction;
    public List<CharacterCooldownAction> skillActions;
    private int _skillIndex = 0;


    // Delegates to transmit values received from animation events to the actions
    public delegate void ActionAnimationEvent(int context);    

    
    [NonSerialized]
    public ActionAnimationEvent actionAnimationEvent;


    // Functions subscribed to animation-specific events
    public void OnActionAnimationEvent(int context)
    {
        actionAnimationEvent?.Invoke(context);
    }





    void Awake()
    {
        s_ActivePlayer = this;
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        //mainCamera = CameraController.Instance.MainCamera;
    }




    // Input functions called by Player Input
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (!character.IsActionLocked)
        {
            dodgeAction.Attempt();

        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (!character.IsActionLocked)
        {
            baseAttack.Attempt();
        }
    }
    
    public void OnSkillInput(InputAction.CallbackContext context)
    {
        // Implement better system to allow multiple casts (need to review input system)
        if (!character.IsActionLocked)
        {
            skillActions[_skillIndex].Attempt();
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveInputVector = context.ReadValue<Vector2>().normalized;
        
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        // Uses screen position (for now only Mouse)
        if (context.control.device is Mouse)
        {
            Vector2 mousePosition  = context.ReadValue<Vector2>();
            Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));
            Vector2 dir = worldMousePosition - transform.position;
            character.lastLookDirection = dir.normalized;
            pointDirectionVector= dir.normalized;
        }
        // Uses normalized vector2
        else 
        {
            character.lastLookDirection = context.ReadValue<Vector2>();
            pointDirectionVector = context.ReadValue<Vector2>();
        }
    }



    // Subscribed to Damageable OnDeath event
    public void OnDeath()
    {
        animator.SetTrigger(deadTriggerHash);
        character.DisableCharacter();
    }

    // Subscribed to Death Animation
    public void OnDeathAnimationEnd()
    {
        GameOverManager.Instance.StartGameOver();
        onDeathEnd.Invoke(this);
    }



    void Update()
    {
        animator.SetInteger(verticalAxisHash, Mathf.RoundToInt(_moveInputVector.y));
        animator.SetInteger(horizontalAxisHash, Mathf.RoundToInt(_moveInputVector.x));

        if (!character.IsActionLocked)
        {
            FlipX();
            UpdateMovementAnimation();
            character.Move(_moveInputVector * character.moveSpeed);
        }
        
    }


    // Time to determine when to set move to true
    void UpdateMovementAnimation()
    {
        if (_moveInputVector == Vector2.zero)
        {
            if (_lastStopTime == 0)
            {
                _lastStopTime = Time.time;
            }
            else
            {
                if (Time.time > _lastStopTime + toIdleCooldown)
                {
                    _onIdleAnimation = true;

                }
            }
        }
        else
        {
            _lastStopTime = 0;
            _onIdleAnimation = false;
        }

        animator.SetBool(moveHash, !_onIdleAnimation);
    }

    void FlipX()
    {
        if (AnimatorGetFacingDirection.CurrentDirection == 
            AnimatorGetFacingDirection.Direction.Forward)
        {
            if (_moveInputVector != Vector2.zero)
            {
                character.FlipX(_moveInputVector.x < 0);
            }
        }
        else
        {
           character.FlipX(false);
            
        }
    }
}

