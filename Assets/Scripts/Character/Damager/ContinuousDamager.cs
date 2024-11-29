using System;
using System.Collections.Generic; // Import the collections namespace for HashSet
using UnityEngine;
using UnityEngine.Events;

// Script responsible for handling trigger interactions between Damageables
// and non continuous Damagers such as meele attack or fast projectile. 
// The damage is expected to be done only once to each collider

[RequireComponent(typeof(Collider2D))] // of type trigger
public class ContinuousDamager : Damager
{
    [Serializable]
    public class DamageableEvent : UnityEvent<Damageable, ContinuousDamager> { }

    [Serializable]
    public class NonDamageableEvent : UnityEvent<ContinuousDamager> { }

    public int damage = 1;
    public LayerMask hittableLayers;
    public DamageableEvent onDamageableHit;
    public NonDamageableEvent onNonDamageableHit;

    public float timeUntilNextHit = 0.5f;

    private Collider2D _collider;
    public SpriteRenderer characterSpriteRenderer;
    private Vector2 originalColliderOffset;

    // A set to track colliders that have already been hit
    private HashSet<Collider2D> hitColliders;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        originalColliderOffset = _collider.offset;
        hitColliders = new HashSet<Collider2D>();
        //AdjustColliderBasedOnSpriteFlip();
    }


    void Start()
    {
        if (_collider.isTrigger == false)
        {
            Debug.LogWarning("Trigger collider on " + this + " expected to be trigger. "
                + "This damager may not work");
        }
    }

    public void EnableCollider()
    {
        _collider.enabled = true;
        //AdjustColliderBasedOnSpriteFlip();
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
        ResetHitColliders();
    }


    public void AdjustColliderBasedOnSpriteFlip()
    {
        if (characterSpriteRenderer.flipX)
        {
            _collider.offset = new Vector2(-originalColliderOffset.x, originalColliderOffset.y);
        }
        else
        {
            _collider.offset = originalColliderOffset;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {

        hitColliders.Add(collider);

        int layer = collider.gameObject.layer;

        //Debug.Log("entrou em contato com o fogo");

        // Layer is included in the LayerMask
        if ((hittableLayers & (1 << layer)) != 0)
        {
            Damageable damageable = collider.GetComponent<Damageable>();
            if (damageable != null)
            {
                onDamageableHit.Invoke(damageable, this);
                damageable.HitTakeDamage(this.gameObject, damage);
                hitColliders.Clear();
            }
            else
            {
                onNonDamageableHit.Invoke(this);
            }
            //wait 0.5 seconds
        }
    }

    public void ResetHitColliders()
    {
        hitColliders.Clear();
    }
}
