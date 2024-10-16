
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))] //Must face right!!
public class Character : MonoBehaviour
{
    // Actions Lock action executed by character that can be force cancelled
    // Also includes Controller Specific animations
    // Only one action lock action can happen at once
    public delegate void CancellableAction();
    [NonSerialized]
    public CancellableAction cancellableAction;

    public int baseMoveSpeed = 2;
    public int moveSpeed = 2;

    // Cant move / execute any action
    public bool isActionLocked;

    // Manages hp and immunity
    public Damageable damageable;

    // Manages status effect pool
    public StatusEffectManager statusEffectManager;

    // Get relative movement direction from other scripts
    //[NonSerialized]
    public Vector2 lastMoveVector = Vector2.zero;

    // Acessible Character gameObject components
    [NonSerialized]
    public Rigidbody2D rb;

    [NonSerialized]
    public SpriteRenderer spriteRenderer;


    void Awake() 
    {
        damageable = GetComponent<Damageable>();
        statusEffectManager = GetComponent<StatusEffectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isActionLocked && lastMoveVector != Vector2.zero)
        {
            spriteRenderer.flipX = lastMoveVector.x < 0;
        }
    }
    

    public void Move(Vector2 moveVector)
    {
        lastMoveVector = moveVector;
        Vector2 newPos = rb.position + moveVector * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    public void Teleport(Vector2 offset)
    {
        Vector2 newPos = rb.position + offset;
        rb.MovePosition(newPos);
    }


    // Cancel action lock methods
    public void StartCancellableActionLock(CancellableAction method)
    {
        isActionLocked = true;
        cancellableAction = method;
    }

    public void EndCancellableActionLock()
    {
        isActionLocked = false;
        cancellableAction = null;
    }

    public void StartNonCancellableActionLock()
    {
        isActionLocked = true;
    }

    public void EndNonCancellableActionLock()
    {
        isActionLocked = false;
    }
}
