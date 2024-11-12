using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AiMovement))]
[RequireComponent(typeof(DirectionalMovement))]
public class BasicSlashEnemy : MonoBehaviour
{
    [Serializable]
    public class OnAttackEvent : UnityEvent<BasicSlashEnemy>
    { }

    [Serializable]
    public class OnCorpseDestroyedEvent : UnityEvent<BasicSlashEnemy>
    { }

    static readonly int horizontalAxisHash = Animator.StringToHash("HorizontalAxis");
    static readonly int verticalAxisHash = Animator.StringToHash("VerticalAxis");

    static readonly int attackTriggerHash = Animator.StringToHash("BaseAttack");
    static readonly int deathTriggerHash = Animator.StringToHash("Dead");
    static readonly int movingBooleanHash = Animator.StringToHash("Moving");
    private Animator _animator;

    [NonSerialized]
    public AiMovement aiMovement;
    
    [NonSerialized]
    public Character character; 

    [NonSerialized]
    public DirectionalMovement movement;

    public float slashDelayTime = 0.5f;
    public float slashCooldown = 0.75f;
    public float attackDistance = 1f;
    public float corpseDestroyDelay = 4f;
    
    private float lastSlashEndTime = 0f;

    public TriggerDamager damagerForward;
    public TriggerDamager damagerUp;
    public TriggerDamager damagerDown;
    TriggerDamager currentDamager;



    [Tooltip("Event triggered when this action begins.")]
    public OnAttackEvent performed;

    [Tooltip("Event triggered when the slash attack starts (frames with collision ON).")]
    public OnAttackEvent slashStart;

    [Tooltip("Event triggered when the action has ended.")]
    public OnAttackEvent actionEnded;

    [Tooltip("Event triggered when the action is cancelled before it ends.")]
    public OnAttackEvent actionCancelled;

    [Tooltip("Event triggered when the corpse is destroyed.")]
    public OnCorpseDestroyedEvent corpseDestroyed;

    AnimatorGetFacingDirection.Direction facingDirection = AnimatorGetFacingDirection.Direction.Forward;

    void Awake()
    {
        movement = GetComponent<DirectionalMovement>();
        aiMovement = GetComponent<AiMovement>();
        character = GetComponent<Character>();
        currentDamager = damagerForward; //default value

        _animator = GetComponent<Animator>();

        AnimatorGetFacingDirection.AssignDelegatesToAnimator(_animator, (ctx) => {facingDirection = ctx;});
    }


    void Start()
    {
        damagerForward.DisableCollider();
        damagerUp.DisableCollider();
        damagerDown.DisableCollider();
    }


    void Update()
    {
        
        UpdateAnimatorFacing();
        // behaviour can be improved
        if (CanAttack())
        {
            character.StartActionLock(OnSlashCancel, this);
            performed.Invoke(this);
            StartCoroutine(StartSlash());
        }

        if (character.IsActionLocked)
        {
            _animator.SetBool(movingBooleanHash, false);
        }
        else
        {
            _animator.SetBool(movingBooleanHash, true);
        }
        
    }

    // Static member changed among all animator state instances
    void UpdateAnimatorFacing()
    {
        Vector2 direction = movement.LastMoveVector;
        _animator.SetInteger(horizontalAxisHash, Mathf.RoundToInt(direction.x));
        _animator.SetInteger(verticalAxisHash, Mathf.RoundToInt(direction.y));

        movement.UpdateFacingDirection();
    }

    bool CanAttack()
    {
        return aiMovement.CurrentDistance <= attackDistance && !character.IsActionLocked 
            && lastSlashEndTime + slashCooldown < Time.time;
    }


    IEnumerator StartSlash()
    {
        SelectDamager(facingDirection);
        yield return new WaitForSeconds(slashDelayTime);
        _animator.SetTrigger(attackTriggerHash);

    }

    void SelectDamager(AnimatorGetFacingDirection.Direction direction)
    {
        //Debug.Log(direction);
        switch (direction)
        {
            case AnimatorGetFacingDirection.Direction.Up:
                currentDamager = damagerUp;
                break;
            case AnimatorGetFacingDirection.Direction.Down:
                currentDamager = damagerDown;
                break;
            case AnimatorGetFacingDirection.Direction.Forward:
                currentDamager = damagerForward;
                break;
        }
        //Debug.Log(direction);
    }




    // Animation Events for the Slashing Attack
    public void OnSlashCollisionStart()
    {
        currentDamager.EnableCollider();
        slashStart.Invoke(this);
    }

    public void OnSlashCollisionEnd()
    {
        currentDamager.DisableCollider();
    }

    public void OnSlashAnimationEnd()
    {
        character.EndActionLock(this);
        lastSlashEndTime = Time.time;
        actionEnded.Invoke(this);
    }

    public void OnSlashCancel()
    {
        character.EndActionLock(this);
        currentDamager.DisableCollider();
        lastSlashEndTime = Time.time;
        actionCancelled.Invoke(this);
    }



    // Subscribed to Damageable
    public void StartDeath()
    {
        _animator.SetTrigger(deathTriggerHash);
        character.DisableCharacter();
        aiMovement.SwitchState(AiMovement.MovementState.Stop);
    }



    // Animation Event for Death Animation
    public void OnDeathAnimationEnd() 
    { 
        StartCoroutine(DestroyAfterTime());
    }

    public IEnumerator DestroyAfterTime()
    {
        corpseDestroyed.Invoke(this);
        yield return new WaitForSeconds(corpseDestroyDelay);  
        Destroy(gameObject);  
    }


}
