using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AiMovement))]
public class BasicSlashEnemy : MonoBehaviour
{
    [Serializable]
    public class OnAttackEvent : UnityEvent<BasicSlashEnemy>
    { }

    [Serializable]
    public class OnCorpseDestroyedEvent : UnityEvent<BasicSlashEnemy>
    { }

    static readonly int attackTriggerHash = Animator.StringToHash("Attack");
    static readonly int deathTriggerHash = Animator.StringToHash("Death");
    static readonly int idleBooleanHash = Animator.StringToHash("Idle");
    private Animator _animator;

    public TriggerDamager damager; 
    public AiMovement aiMovement;
    public Character character; 

    public float slashDelayTime = 0.5f;
    public float slashCooldown = 0.75f;
    public float attackDistance = 1f;
    public float corpseDestroyDelay = 4f;
    
    private float lastSlashEndTime = 0f;



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



    void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    void Start()
    {
        aiMovement.currentState = AiMovement.MovementState.Attack;
    }


    void Update()
    {
        // behaviour can be improved
        if (CanAttack())
        {
            _animator.SetBool(idleBooleanHash, true);
            character.StartActionLock(OnSlashCancel, this);
            performed.Invoke(this);
            StartCoroutine(StartSlash());
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
        _animator.SetTrigger(attackTriggerHash);
    }




    // Animation Events for the Slashing Attack
    public void OnSlashCollisionStart()
    {
        damager.EnableCollider();
        slashStart.Invoke(this);
    }

    public void OnSlashCollisionEnd()
    {
        damager.DisableCollider();
    }

    public void OnSlashAnimationEnd()
    {
        character.EndActionLock(this);
        _animator.SetBool(idleBooleanHash, false);
        lastSlashEndTime = Time.time;
        actionEnded.Invoke(this);
    }

    public void OnSlashCancel()
    {
        character.EndActionLock(this);
        damager.DisableCollider();
        _animator.SetBool(idleBooleanHash, false);
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
