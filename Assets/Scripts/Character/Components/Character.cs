
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEditor.Tilemaps;
using UnityEngine;


// Physics movement is required for any character
[RequireComponent(typeof(Rigidbody2D))]

// Generic definition for the movement of character
// Subclasses may be used to handle more specific movement needs
[RequireComponent(typeof(EntityMovement))]

// Main character sprite required
[RequireComponent(typeof(SpriteRenderer))] 

[RequireComponent(typeof(CharacterDamage))]



// Other non-required but generally used components:

/// - Damageable: Allows character to take damage, and send signals on hit or on death

/// - Any State Manager script: Signals when to move to CharacterMovement, Action Locks and when
/// to execute any other behaviour

public class Character : MonoBehaviour
{
    public event Action<bool> OnFlipX;

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
    private bool _isActionLocked = false;

    // Only the script that began the lock can remove it to ensure consitency
    // This object reference key is set to null when its not in use
    private object lockObject = null;


    
    [NonSerialized] public Damageable damageable;
    [NonSerialized] public SpriteRenderer mainSpriteRenderer;
    [NonSerialized] public Rigidbody2D rb;
    [NonSerialized] public EntityMovement entityMovement;
    [NonSerialized] public CharacterDamage damage;

    public List<SpriteRenderer> subSpriteRenderers;

   
  
    // Handle collider flipping
    public List<Collider2D> colliders;
    private List<Vector2> collidersOriginalOffset;
    private List<Vector2> collidersFlipXOffset;



    void Awake() 
    {
        damage = GetComponent<CharacterDamage>();
        damageable = GetComponent<Damageable>();
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        entityMovement = GetComponent<EntityMovement>();

        // Collider facing values
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
        if (value != mainSpriteRenderer.flipX)
        {
            // Flip sprite renderers
            mainSpriteRenderer.flipX = value;
            for (int i = 0; i < subSpriteRenderers.Count; i++)
            {
                subSpriteRenderers[i].flipX = value;
            }

            // Flip colliders tied to the sprite renderers
            if (mainSpriteRenderer.flipX)
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

        OnFlipX.Invoke(value);
    }





    // Action lock methods
    public bool StartActionLock(InterruptAction method, object key)
    {
        //Debug.Log("Lock by " + key.GetType().Name + " on gameobject " + gameObject.name);
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
        //Debug.Log("FORCE LOCK by " + key.GetType().Name + " on gameobject " + gameObject.name);
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
