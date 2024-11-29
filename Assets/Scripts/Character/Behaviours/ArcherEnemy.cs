using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Assertions;
using Unity.VisualScripting;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AiMovementArcher))]
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
    static readonly int updateDirBooleanHash = Animator.StringToHash("UpdateDir");

    private Animator _animator;

    [NonSerialized]
    public AiMovementArcher aiMovement;
    
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

    private GameObject player;

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
        aiMovement = GetComponent<AiMovementArcher>();
        character = GetComponent<Character>();
        _animator = GetComponent<Animator>();

        character.OnFlipX += FlipForwardArrowTransform;
    }

    void Start()
    {
        // Start as idle
        player = GameObject.FindGameObjectWithTag("Player");
        _animator.SetBool(movingBooleanHash, false);
    }


    
    void Update()
    {
        
        // behaviour can be improved


        if (!character.IsActionLocked)
        {
            if (CanAttack())
            {
                character.StartActionLock(OnAttackCancel, this);
                performed.Invoke(this);
                StartCoroutine(StartAttack());
            }
            else
            {
                MoveUpdateFacingDirection();
                movement.UpdateRendererFlipOnMove();    
                IdleWalkTransitions();
            }

            
        }

    }

    bool idle = true;
    Coroutine idleCoroutine = null;
    void IdleWalkTransitions()
    {
        if (!idle) 
        {
            // Somehow the last move vector when its supposed to be stopped is 0.11
            // Fix this later
            
            //Debug.Log(movement.LastMoveVector);
            if (movement.LastMoveVector == Vector2.zero && idleCoroutine == null)
            {
                Debug.Log("Idle!!");
                idleCoroutine = StartCoroutine(ToggleIdle());
            }
            else if (movement.LastMoveVector != Vector2.zero && idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }
        }
        else
        {
            if (movement.LastMoveVector != Vector2.zero) 
            {
                idle = false;
                _animator.SetBool(movingBooleanHash, true);
            }
        }
        
        
    }

    public float secondsTillIdle = 0.6f;
    IEnumerator ToggleIdle()
    {
        yield return new WaitForSeconds(secondsTillIdle);

        idle = true;
        _animator.SetBool(movingBooleanHash, false);
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
                //Debug.Log("Up");
                break;
            case AnimatorGetFacingDirection.Direction.Down:
                currentTransform = transformDown;
                //Debug.Log("Down");
                break;
            case AnimatorGetFacingDirection.Direction.Forward:
                currentTransform = transformForward;
                //Debug.Log("Forward");
                break;
        }
    }


    void UpdateFacingDirectionTowards(Vector2 vectorDirection)
    {
        Vector2 cardinalDirection = DirectionHelper.GetCardinalDirection(vectorDirection);
        
        _animator.SetTrigger(updateDirBooleanHash);
        _animator.SetInteger(horizontalAxisHash, Mathf.RoundToInt(cardinalDirection.x));
        _animator.SetInteger(verticalAxisHash, Mathf.RoundToInt(cardinalDirection.y));

        if (cardinalDirection.y == 0)
        {
            character.FlipX(cardinalDirection.x < 0);
        }
        else
        {
            character.FlipX(false);
        }

    }

    // Static member changed among all animator state instances
    void MoveUpdateFacingDirection()
    {
        Vector2 direction = movement.LastMoveVector;
        _animator.SetInteger(horizontalAxisHash, Mathf.RoundToInt(direction.x));
        _animator.SetInteger(verticalAxisHash, Mathf.RoundToInt(direction.y));
    }

    bool CanAttack()
    {
        return aiMovement.CurrentDistance <= attackDistance && !character.IsActionLocked 
            && lastSlashEndTime + attackCooldown < Time.time;
    }


    IEnumerator StartAttack()
    {
        Vector3 vector = aiMovement.Target.transform.position - transform.position;
        UpdateFacingDirectionTowards(vector);

        yield return new WaitForSeconds(preAttackDelay);
        _animator.SetTrigger(attackTriggerHash);
    }




    public void OnAttackEnd()
    {
        character.EndActionLock(this);
        lastSlashEndTime = Time.time;
        actionEnded.Invoke(this);

        // Instantiate arrow

        float angle = 0;
        Vector2 direction = Vector2.right; 

        SelectArrowSpawnPosition(movement.FacingDirection);
        

        // Considering arrow is facing right
        if (currentTransform == transformDown) 
        {
            angle = 270;
            direction = Vector2.down;
        }
        else if (currentTransform == transformUp) 
        {
            angle = 90;
            direction = Vector2.up;
        }
        else if (character.mainSpriteRenderer.flipX == true) 
        {
            angle = 180;
            direction = Vector2.left;
        }
        
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        GameObject newObject = Instantiate(arrow, currentTransform.position, rotation);
        Projectile projectile = newObject.GetComponent<Projectile>();
        projectile.target = player.transform;
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
        aiMovement.SwitchState(AiMovementArcher.MovementState.Stop);
        
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



public static class DirectionHelper
{
    public static Vector2 GetCardinalDirection(Vector2 direction)
    {
        // Normalize the vector to avoid issues with magnitude
        direction.Normalize();

        // Compare absolute values of x and y to determine the dominant axis
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Dominant axis is x
            return direction.x > 0 ? new Vector2(1, 0) : new Vector2(-1, 0); // Right or Left
        }
        else
        {
            // Dominant axis is y
            return direction.y > 0 ? new Vector2(0, 1) : new Vector2(0, -1); // Up or Down
        }
    }
}