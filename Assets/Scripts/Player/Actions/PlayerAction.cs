using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;


public abstract class PlayerAction : MonoBehaviour
{
    public float cooldown = 1f;
    
    protected float lastActivatedTime = 0f;
    protected GameObject playerObject;
    protected PlayerController player;

    public void Awake()
    {
        this.playerObject = transform.parent.gameObject;
        this.player = playerObject.GetComponent<PlayerController>();
    }

    public bool Attempt(int context = 0)
    {
        if (Time.time >= cooldown + lastActivatedTime)
        {
            lastActivatedTime = Time.time; 
            Perform(context);
            return true;
        }
        return false; // Action is still on cooldown
    }

    // Key method that executes the action itself
    protected abstract void Perform(int context = 0);
    
    // Possible Methods

    // Called for each Update() when action is active
    public virtual void Execute() {}

    // Called for each FixedUpdate() when action is active
    public virtual void FixedExecute() {}

    // Called by Animation Events
    public virtual void OnAnimationEvent(int context) {}

    // Called when Action is requested to be canceled. Not all actions need to comply
    // In specific, it is very difficult for AnimationActions to actually allow canceling
    public virtual void Cancel() {}
}

public abstract class PlayerDashBase : PlayerAction 
{
    public int staminaCost = 0; //Not yet implemented

    public Vector2 GetMoveVector()
    {
        Vector2 moveVector = player.moveVector;
        if (moveVector == Vector2.zero)
        {
            moveVector = new Vector2(player.spriteRenderer.flipX ? -1 : 1, 0);  
        }
        return moveVector;
    }

}


public abstract class PlayerSkillBase : PlayerAction
{
    public int energyCost = 1;
}

public abstract class PlayerSlashBase : PlayerAction
{
    protected Collider2D slashCollider;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        slashCollider = GetComponent<Collider2D>();
    }
 
}