using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityMovement : MonoBehaviour
{
    Vector2 _lastMoveVector = Vector2.zero;
    public Vector2 LastMoveVector { get {return _lastMoveVector;} }

    public float baseMoveSpeed = 2;
    public float moveSpeed = 2;
    Rigidbody2D rb;

    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    public virtual void Move(Vector2 moveVector)
    {
        Vector2 newPos = rb.position + moveVector * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    public virtual void Teleport(Vector2 offset)
    {
        Vector2 newPos = rb.position + offset;
        rb.MovePosition(newPos);
    }

}
