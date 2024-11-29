using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(TriggerDamager))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 4f;
    public float lifeSpan = 10f; 
    public int pierce = 1;
    // Vector2 direction = Vector2.right;
    // public Vector2 Direction { set {direction = value; } }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyAfterTime());
    }


    void FixedUpdate()
    {
        // Move the projectile
        Vector2 direction = transform.right;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }

    public void DestroyOnHitObstacle()
    {
        Destroy(gameObject);
    }

    public void UpdatePierce()
    {
        pierce--;
        //Debug.Log("HIT!");
        if (pierce <= 0)
        {
            Destroy(gameObject);
        }
    }
    
}