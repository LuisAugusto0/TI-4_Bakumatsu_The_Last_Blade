using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityMovement : MonoBehaviour
{
    protected Vector2 lastMoveVector = Vector2.zero;
    public Vector2 LastMoveVector { get {return lastMoveVector;} }

    public float baseMoveSpeed = 2;
    public float moveSpeed = 2;
    
    [NonSerialized]
    public Rigidbody2D rb;


    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    protected void Move(Vector2 moveVector)
    {
        lastMoveVector = moveVector;
        Vector2 newPos = rb.position + moveVector * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }


    // @warning: No casting methods to ensure the teleportation ends inside collision
    public void TeleportTowards(Vector2 offset)
    {
        Vector2 newPos = rb.position + offset;
        rb.MovePosition(newPos);
    }

    public void TeleportToWorldPosition(Vector2 pos)
    {
        rb.MovePosition(pos);
    }



    public virtual void MoveTowardsPoint(Vector2 worldPosition)
    {
        Vector2 currentPos = rb.position; 

        float distance = moveSpeed * Time.deltaTime;
        Vector2 newPos = Vector2.MoveTowards(currentPos, worldPosition, distance); 

        lastMoveVector = newPos - currentPos;
        rb.MovePosition(newPos);
        
    }
    
    public virtual void MoveTowardsDirection(Vector2 direction)
    {
        Move(direction * moveSpeed);
    }

    public virtual void MoveTowardsMoveVector(Vector2 moveVector)
    {
        Move(moveVector);
    }

}
