using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AiMovement))]
public class BasicSlashEnemy : MonoBehaviour
{
    static readonly int attackTriggerHash = Animator.StringToHash("Attack");
    static readonly int deathTriggerHash = Animator.StringToHash("Death");
    static readonly int idleBooleanHash = Animator.StringToHash("Idle");
    private Animator _animator;

    public Damager damager; 
    public AiMovement movement;
    public Character character; 

    public float slashDelayTime = 0.5f;
    public float slashDistance = 2.4f;
    public float corpseDestroyDelay = 4f;
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    void Start()
    {
        movement.currentState = AiMovement.MovementState.Attack;
    }


    void Update()
    {
        if (movement.currentDistance <= slashDistance && !character.isActionLocked)
        {
            _animator.SetBool(idleBooleanHash, true);
            character.StartCancellableActionLock(OnSlashCancel);
            StartCoroutine(StartSlash());
        }
        
    }


    IEnumerator StartSlash()
    {
        yield return new WaitForSeconds(slashDelayTime);
        _animator.SetTrigger(attackTriggerHash);
    }


    // Animation Events for the Slashing Attack
    public void OnSlashCollisionStart()
    {
        damager.enabled = true;

    }

    public void OnSlashCollisionEnd()
    {
        damager.enabled = false;
    }

    public void OnSlashAnimationEnd()
    {
        character.EndCancellableActionLock();
        _animator.SetBool(idleBooleanHash, false);
    }

    public void OnSlashCancel()
    {
        character.EndCancellableActionLock();
        damager.enabled = false;
        _animator.SetBool(idleBooleanHash, false);
    }

    // Subscribed to Damageable
    public void StartDeath()
    {
        _animator.SetTrigger(deathTriggerHash);
        character.StartNonCancellableActionLock();
    }

    // Animation Event for Death Animation
    public void OnDeathAnimationEnd() 
    { 
        StartCoroutine(DestroyAfterTime());
    }

    public IEnumerator DestroyAfterTime()
    {

        yield return new WaitForSeconds(corpseDestroyDelay);  
        Destroy(gameObject);  
    }


}
