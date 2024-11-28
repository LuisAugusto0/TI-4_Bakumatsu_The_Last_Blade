using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Assertions;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AiMovement))]
[RequireComponent(typeof(DirectionalMovement))]
public class ArcherEnemy : MonoBehaviour
{
    [Serializable]
    public class OnAttackEvent : UnityEvent<ArcherEnemy>
    { }

    [Serializable]
    public class OnCorpseDestroyedEvent : UnityEvent<ArcherEnemy>
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

    public float preAttackDelay = 0.5f;
    public float attackCooldown = 0.75f;
    public float attackDistance = 1f;
    public float corpseDestroyDelay = 4f;
    
    private float lastSlashEndTime = 0f;


    public Transform transformForward;
    public Transform transformUp;
    public Transform transformDown;
    Transform currentTransform;

    public GameObject arrow;



    [Tooltip("Event triggered when this action begins.")]
    public OnAttackEvent performed;

    [Tooltip("Event triggered when the slash attack starts (frames with collision ON).")]
    public OnAttackEvent attackStart;

    [Tooltip("Event triggered when the action has ended.")]
    public OnAttackEvent actionEnded;

    [Tooltip("Event triggered when the action is cancelled before it ends.")]
    public OnAttackEvent actionCancelled;

    [Tooltip("Event triggered when the corpse is destroyed.")]
    public OnCorpseDestroyedEvent corpseDestroyed;

    AnimatorGetFacingDirection.Direction facingDirection = AnimatorGetFacingDirection.Direction.Forward;

    void Awake()
    {
        Debug.Assert(transformUp != null, "Serialized data not received");
        Debug.Assert(transformDown != null, "Serialized data not received");
        Debug.Assert(transformForward != null, "Serialized data not received");
        currentTransform = transformForward;

        Debug.Assert(arrow != null, "Serialized data not received");

        movement = GetComponent<DirectionalMovement>();
        aiMovement = GetComponent<AiMovement>();
        character = GetComponent<Character>();
        _animator = GetComponent<Animator>();

        AnimatorGetFacingDirection.AssignDelegatesToAnimator(_animator, (ctx) => {facingDirection = ctx;});

        character.OnFlipX += FlipForwardArrowTransform;
    }


    void Update()
    {
        
        UpdateAnimatorFacing();
        // behaviour can be improved
        if (CanAttack())
        {
            character.StartActionLock(OnAttackCancel, this);
            performed.Invoke(this);
            StartCoroutine(StartAttack());
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

    void FlipForwardArrowTransform(bool value)
    {
        int scale = value ? -1 : 1;
        transformForward.localScale = new Vector3(scale, 1, 1);
    }


    void SelectArrowSpawnPosition(AnimatorGetFacingDirection.Direction direction)
    {
        switch (direction)
        {
            case AnimatorGetFacingDirection.Direction.Up:
                currentTransform = transformUp;
                break;
            case AnimatorGetFacingDirection.Direction.Down:
                currentTransform = transformDown;
                break;
            case AnimatorGetFacingDirection.Direction.Forward:
                currentTransform = transformForward;
                break;
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
            && lastSlashEndTime + attackCooldown < Time.time;
    }


    IEnumerator StartAttack()
    {
        SelectArrowSpawnPosition(facingDirection);
        yield return new WaitForSeconds(preAttackDelay);
        _animator.SetTrigger(attackTriggerHash);
    }

    public void OnAttackEnd()
    {
        character.EndActionLock(this);
        lastSlashEndTime = Time.time;
        actionEnded.Invoke(this);

        // Instantiate arrow
        float rotationX = 0f;

        // Considering arrow is facing right
        if (currentTransform == transformDown) rotationX = -90f;
        else if (currentTransform == transformUp) rotationX = 90f;
        
        Quaternion xOnlyRotation = Quaternion.Euler(rotationX, 0, 0);
        GameObject newObject = Instantiate(arrow, currentTransform.position, xOnlyRotation);
    }

    public void OnAttackCancel()
    {
        character.EndActionLock(this);
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
