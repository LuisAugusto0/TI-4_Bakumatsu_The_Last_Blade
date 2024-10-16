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

    // PlayerInput from Input.System
    [SerializeField] private InputActionAsset actions;
    private InputAction moveInput;
    private Vector2 _moveInputVector;
    private InputAction slashInput;
    private InputAction dodgeInput;
    private InputAction skillInput;


    void OnEnable()
    {
        moveInput.Enable();
        dodgeInput.Enable();
        slashInput.Enable();
        skillInput.Enable();
    }

    void OnDisable()
    {
        moveInput.Disable();
        dodgeInput.Disable();
        slashInput.Disable();
        skillInput.Enable();
    }

    void Awake()
    {
        s_ActivePlayer = this;
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();

        var playerActionMap = actions.FindActionMap("Player");
        moveInput = playerActionMap.FindAction("Move");
        dodgeInput = playerActionMap.FindAction("Dash");
        slashInput = playerActionMap.FindAction("Slash");
        skillInput = playerActionMap.FindAction("Ability");

        dodgeInput.performed += OnDashInput;
        slashInput.performed += OnSlashInput;
        skillInput.performed += OnSkillInput;
    }

    void Update()
    {
        _moveInputVector = moveInput.ReadValue<Vector2>().normalized;
        if (!character.isActionLocked)
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
        character.StartNonCancellableActionLock();
    }



    // Functions subscribed to input calls
    void OnDashInput(InputAction.CallbackContext context)
    {
        if (!character.isActionLocked)
        {
            dodgeAction.Attempt();

        }
    }

    void OnSlashInput(InputAction.CallbackContext context)
    {
        if (!character.isActionLocked)
        {
            slashAction.Attempt();
        }
    }
    
    void OnSkillInput(InputAction.CallbackContext context)
    {
        // Implement better system to allow multiple casts (need to review input system)
        if (!character.isActionLocked)
        {
            skillActions[_skillIndex].Attempt();
        }
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
