using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    static readonly public int moveParameterHash = Animator.StringToHash("IsMoving"); 
    static readonly public int deadParameterHash = Animator.StringToHash("IsDead"); 
    static readonly public int slashTriggerHash = Animator.StringToHash("SlashTrigger"); 
    static readonly public int rollTriggerHash = Animator.StringToHash("RollTrigger"); 

    // Player attributes
    public int hp = 3;
    public int energy = 0; // Used for throwing / special attacks

    // Basic movement 
    public Vector2 moveVector; // Input movement Vector
    public float moveSpeed = 2f;
    private float lastStopTime = 0f;
    public float toIdleCooldown = 0.5f;

    // Perfect dodge
    public float perfectDashWindow = 0.2f;
    public float lastDashTime = 0f;
    
    
    // States
    public bool onMove = false; // Used to manage when to switch to idle animation
    public bool onDash = false; 
    public bool onSlash = false;
    public bool onAbility = false;
    public bool isImmune = false;



    // Map Boundary
    [SerializeField] private float minX = -10f;   
    [SerializeField] private float maxX = 10f;    
    [SerializeField] private float minY = -10f;   
    [SerializeField] private float maxY = 10f;    


    // Components     
    public Animator animator;
    private Rigidbody2D rb;
    [NonSerialized] public SpriteRenderer spriteRenderer;

    // Actions
    public PlayerDashBase dash;
    public PlayerSkillBase ability;

    public GameObject slashObject;
    private PlayerSlashBase slash;

    // PlayerInput from Input.System
    [SerializeField] private InputActionAsset actions;
    private InputAction moveAction;
    private InputAction slashAction;
    private InputAction dashAction;
    private InputAction abilityAction;



    void Awake()
    {
        // Assign actions from ActionMap
        var playerActionMap = actions.FindActionMap("Player");
        moveAction = playerActionMap.FindAction("Move");
        dashAction = playerActionMap.FindAction("Dash");
        slashAction = playerActionMap.FindAction("Slash");
        abilityAction = playerActionMap.FindAction("Ability");

        // Call OnDashAction when the dash action is performed
        dashAction.performed += OnDashAction;
        slashAction.performed += OnSlashAction;
        abilityAction.performed += OnAbilityAction;

        // Get self components
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        slash = slashObject.GetComponent<PlayerSlashBase>();

        //Instantiate if prefab <IMPLEMENT>
    }



    void OnEnable()
    {
        moveAction.Enable();
        dashAction.Enable();
        slashAction.Enable();
        abilityAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        dashAction.Disable();
        slashAction.Disable();
        abilityAction.Enable();
    }


    void Update() 
    {
        UpdateMovementInput();
    }

    void UpdateMovementInput()
    {
        moveVector = moveAction.ReadValue<Vector2>().normalized;
        
        if (moveVector == Vector2.zero)
        {
            if (lastStopTime == 0)
            {
                lastStopTime = Time.time;
            }
            else
            {
                if (Time.time > lastStopTime + toIdleCooldown)
                {
                    onMove = false;

                }
            }
        }
        else
        {
            lastStopTime = 0;
            onMove = true;
        }

        animator.SetBool(moveParameterHash, onMove);
    }


    void FixedUpdate() 
    {
        if (onSlash)
        {
            slash.Execute();
            return;
        }

        if (onDash) 
        {  
            dash.FixedExecute();
            return;
        } 
        
        BasicMovement();
    }


    void BasicMovement() 
    {
        if (moveVector != Vector2.zero) 
        {
            spriteRenderer.flipX = moveVector.x < 0;

            Move(moveVector, moveSpeed);
        }

    }


    // Definitions for Movement of the Player
    public void Move(Vector2 normalizedVector, float speed)
    {
        Vector2 newPos = rb.position + normalizedVector * speed * Time.fixedDeltaTime;
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        rb.MovePosition(newPos);
    }

    public void InstantMove(Vector2 normalizedVector, float distance)
    {
        Vector2 newPos = rb.position + normalizedVector * distance * Time.fixedDeltaTime;
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        rb.MovePosition(newPos);
    }

    // Scripts called by InputAction.performed function calls
    void OnDashAction(InputAction.CallbackContext context) 
    {
        Debug.Log("HERE");
        if (!onDash && !onSlash && !onAbility)
        {
            dash.Attempt();

        }
    }

    void OnSlashAction(InputAction.CallbackContext context) 
    {
        if (!onDash && !onSlash && !onAbility)
        {
            slash.Attempt();
        }
    }


    // Scripts called by Animation Events
    public void SlashAnimationEvent(int context = 0)
    {
        slash.OnAnimationEvent(context);
    }

    public void SkillAnimationEvent(int context = 0)
    {
        ability.OnAnimationEvent(context);
    }

    public void DashAnimationEvent(int context = 0)
    {
        dash.OnAnimationEvent(context);
    }

    void OnAbilityAction(InputAction.CallbackContext context)
    {
        if (!onDash && !onSlash && !onAbility)
        {
            ability.Attempt();

        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyAttack") && !isImmune)
        {
            AttackData attackData = other.GetComponent<AttackData>();
            if (attackData == null) return;

            HealthDecrease(attackData.damage);
            if (attackData.isProjectile) 
            {
                Destroy(other.gameObject);
            } 
        }
        if (other.CompareTag("Energy"))
        {
            EnergyData energyData = other.GetComponent<EnergyData>();
            if (energyData == null) return;

            energy += energyData.energyNum;
            Destroy(other.gameObject);
        }

    }

    void HealthDecrease(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            animator.SetBool(deadParameterHash, true);
            this.enabled = false;
        }
    }
}
