
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))] //Must face right!!
public class Character : MonoBehaviour
{
    // Actions Lock disables character movement / actions
    // They are used when taking damage, doing attack animations, etc
    // All actions can be force cancelled (for example when the character dies)

    // If a certain action needs proper treatment to be cancelled, 
    // it can send a InterruptAction() reference
    public delegate void InterruptAction();

    [NonSerialized]
    public InterruptAction cancellableAction;

    public bool IsActionLocked { get {return _isActionLocked;} }

    [SerializeField]
    private bool _isActionLocked;

    // Only the script that began the lock can remove it to ensure consitency
    // This object reference key is set to null when its not in use
    private object lockObject = null;




    // Main attributes
    public int baseMoveSpeed = 2;
    public int moveSpeed = 2;


    // Manages hp and immunity (can be on child)
    public Damageable damageable;


    // Get relative movement direction from other scripts
    Vector2 _lastMoveVector = Vector2.zero;
    public Vector2 LastMoveVector { get {return _lastMoveVector;} }
    
    [NonSerialized]
    public Vector2 lastLookDirection = Vector2.zero;


    // Acessible Character gameObject components
    [NonSerialized]
    public Rigidbody2D rb;

    [NonSerialized]
    public SpriteRenderer spriteRenderer;

  
    // Handle collider flipping
    public List<Collider2D> colliders;
    private List<Vector2> collidersOriginalOffset;
    private List<Vector2> collidersFlipXOffset;



    void Awake() 
    {
        damageable = GetComponent<Damageable>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        collidersOriginalOffset = new(colliders.Count);
        collidersFlipXOffset = new(colliders.Count);
        for (int i = 0; i < colliders.Count; i++)
        {
            Vector2 offset = colliders[i].offset;
            collidersOriginalOffset.Add(offset);
            collidersFlipXOffset.Add(new Vector2(-offset.x, offset.y));
        }
    }


    public void FlipX(bool value)
    {
        if (value != spriteRenderer.flipX)
        {
            spriteRenderer.flipX = value;
            if (spriteRenderer.flipX)
            {
                for (int i = 0; i < colliders.Count; i++)
                {
                    colliders[i].offset = collidersFlipXOffset[i];
                }
            }
            else
            {
                for (int i = 0; i < colliders.Count; i++)
                {
                    colliders[i].offset = collidersOriginalOffset[i];
                }
            }
        }
        
        
    }

    public void Move(Vector2 moveVector)
    {
        _lastMoveVector = moveVector;
        Vector2 newPos = rb.position + moveVector * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    public void Teleport(Vector2 offset)
    {
        Vector2 newPos = rb.position + offset;
        rb.MovePosition(newPos);
    }


    // Action lock methods

    public bool StartActionLock(InterruptAction method, object key)
    {
        if (_isActionLocked) {
            return false;
        }

        _isActionLocked = true;
        cancellableAction = method;
        lockObject = key;
        return true;
    }

    // Ignore previous action
    public void ForceActionLock(InterruptAction method, object key)
    {
        CancelAction();
        _isActionLocked = true;
        cancellableAction = method;
        lockObject = key;
        
    }

    // Used to force action lock until destroyed (non cancellable)
    public void DisableCharacter()
    {
        ForceActionLock(null, this);
    }

    public bool EndActionLock(object key)
    {
        if (lockObject != key)
        {
            return false;
        }
        cancellableAction = null;
        _isActionLocked = false;
        lockObject = null;
        return true;
    }

    public bool CancelAction()
    {
        if (cancellableAction == null)
        {
            return false;
        }
        cancellableAction.Invoke();
        cancellableAction = null;
        _isActionLocked = false;
        lockObject = null;
        
        return true;
    }


}
