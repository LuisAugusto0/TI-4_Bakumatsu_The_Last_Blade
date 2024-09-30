using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float durationInSeconds = 15f;
    private float lifeTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        Vector2 direction = transform.right;
        rb.velocity = direction * speed;
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > durationInSeconds)
        {
            Destroy(gameObject);   
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle") 
        {
            Destroy(gameObject);
        }
    }
}
