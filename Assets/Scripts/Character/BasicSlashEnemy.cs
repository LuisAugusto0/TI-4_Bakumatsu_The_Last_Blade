using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AIBehaviourHandler))]
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
    

    public AIBehaviourHandler aiMovement;
    public Character character; 

    public float slashDelayTime = 0.5f;
    public float slashCooldown = 0.75f;
    public float attackDistance = 1f;
    public float corpseDestroyDelay = 4f;
    
    private float lastSlashEndTime = 0f;

    public TriggerDamager damagerForward;
    public TriggerDamager damagerUp;
    public TriggerDamager damagerDown;
    TriggerDamager currentDamager = null;



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
        _animator = GetComponent<Animator>();
        currentDamager = damagerForward;
        AnimatorGetFacingDirection.AssignDelegatesToAnimator(_animator, (ctx) => {facingDirection = ctx;});
    }


    void Start()
    {
        aiMovement.currentState = AIBehaviourHandler.MovementState.Attack;
        damagerForward.DisableCollider();
        damagerUp.DisableCollider();
        damagerDown.DisableCollider();
    }


    void Update()
    {
        
        UpdateAnimatorFacing();
        _animator.SetBool(movingBooleanHash, true);
        // behaviour can be improved
        if (CanAttack())
        {
            character.StartActionLock(OnSlashCancel, this);
            performed.Invoke(this);
            StartCoroutine(StartSlash());
        }

        
    }

    // Static member changed among all animator state instances
    void UpdateAnimatorFacing()
    {
        Vector2 direction = aiMovement.GetFacingDirection();
        _animator.SetInteger(horizontalAxisHash, Mathf.RoundToInt(direction.x));
        _animator.SetInteger(verticalAxisHash, Mathf.RoundToInt(direction.y));

        if (facingDirection == AnimatorGetFacingDirection.Direction.Forward)
        {
            if (direction != Vector2.zero)
            {
                character.FlipX(direction.x < 0);
            }
        }
        else
        {
           character.FlipX(false);
            
        }
    }

    bool CanAttack()
    {
        return aiMovement.CurrentDistance <= attackDistance && !character.IsActionLocked 
            && lastSlashEndTime + slashCooldown < Time.time;
    }


    IEnumerator StartSlash()
    {
        yield return new WaitForSeconds(slashDelayTime);
        SelectDamager(facingDirection);
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
        Debug.Log(direction);
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
