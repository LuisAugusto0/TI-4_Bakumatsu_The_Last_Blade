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
    public static PlayerController ActivePlayer { get { return s_ActivePlayer; } }
    protected static PlayerController s_ActivePlayer;


    [Serializable]
    public class OnDeathEnd : UnityEvent<PlayerController>
    { }

    // Reference to character data
    public Character character;

    // Animator clips
    static readonly public int moveParameterHash = Animator.StringToHash("IsMoving");
    static readonly public int deadParameterHash = Animator.StringToHash("IsDead");
    static readonly public int slashTriggerHash = Animator.StringToHash("SlashTrigger");
    static readonly public int rollTriggerHash = Animator.StringToHash("RollTrigger");
    public Animator animator;
    

    // Delegates for Animation specific events for communication for 
    // animation specific actions
    public delegate void SlashEventAction(int context);    
    public delegate void RollEventAction(int context);    

    [NonSerialized]
    public SlashEventAction onSlashEvent;
    
    [NonSerialized]
    public RollEventAction onRollEvent;

    // Actions
    public CharacterAction slashAction;
    public CharacterAction dodgeAction;
    public List<CharacterAction> skillActions;
    private int _skillIndex = 0;

    // Handle when to swap from moving to idle
    public float toIdleCooldown;
    private float _lastStopTime; 
    private bool _onIdleAnimation = true;
    
    // Event called exactly when death anim ends
    public OnDeathEnd onDeathEnd;
    Vector2 _moveInputVector = Vector2.zero;
    

    // Create singleton
    public Camera mainCamera;

    void Awake()
    {
        s_ActivePlayer = this;
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
    }



    // Input functions called by Player Input
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (!character.IsActionLocked)
        {
            dodgeAction.Attempt();

        }
    }

    public void OnSlashInput(InputAction.CallbackContext context)
    {
        if (!character.IsActionLocked)
        {
            slashAction.Attempt();
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
            Vector2 dir = (worldMousePosition - transform.position);
            character.lastLookDirection = dir.normalized;
        }
        // Uses normalized vector2
        else 
        {
            character.lastLookDirection = context.ReadValue<Vector2>();
        }
    }

    void Update()
    {
        if (!character.IsActionLocked)
        {
            UpdateMovementAnimation();
            character.Move(_moveInputVector * character.moveSpeed);
        }
        
    }
    
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

        animator.SetBool(moveParameterHash, !_onIdleAnimation);
    }

    // Subscribed to Damageable OnDeath event
    public void OnDeath()
    {
        animator.SetBool(deadParameterHash, true);
        character.DisableCharacter();
    }





    
    // Functions subscribed to animation-specific events
    public void OnSlashAnimation(int context)
    {
        onSlashEvent?.Invoke(context);
    }

    public void OnRollAnimation(int context)
    {
        onRollEvent?.Invoke(context);
    }

    public void OnDeathAnimationEnd()
    {
        GameOverManager.Instance.StartGameOver();
        onDeathEnd.Invoke(this);
        Debug.Log("DeathAnimEnd");
    }



    
}
