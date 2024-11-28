using System;
using System.Collections.Generic; // Import the collections namespace for HashSet
using UnityEngine;
using UnityEngine.Events;

// Script responsible for handling trigger interactions between Damageables
// and non continuous Damagers such as meele attack or fast projectile. 
// The damage is expected to be done only once to each collider

[RequireComponent(typeof(Collider2D))] // of type trigger
public class TriggerDamager : Damager
{
    [Serializable]
    public class DamageableEvent : UnityEvent<Damageable, TriggerDamager> { }

    [Serializable]
    public class NonDamageableEvent : UnityEvent<TriggerDamager> { }



    [SerializeField] protected int damage = 1;
    public LayerMask hittableLayers;
    public DamageableEvent onDamageableHit;
    public NonDamageableEvent onNonDamageableHit;

    private Collider2D _collider;

    // A set to track colliders that have already been hit
    private HashSet<Collider2D> hitColliders;

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
        hitColliders = new HashSet<Collider2D>(); 
        //AdjustColliderBasedOnSpriteFlip();
    }

    public void FlipX(bool flipX)
    {
        int xScale = flipX ? -1 : 1;
        transform.localScale = new Vector3(xScale, 1, 1);
    }



    protected virtual void Start()
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



    void OnTriggerEnter2D(Collider2D collider)
    {
        // Ignore colliders that accidentally re-enter
        if (hitColliders.Contains(collider))
        {
            return; 
        }

        hitColliders.Add(collider);

        int layer = collider.gameObject.layer;

        // Layer is included in the LayerMask
        if ((hittableLayers & (1 << layer)) != 0)
        {
            Damageable damageable = collider.GetComponent<Damageable>();
            if (damageable != null)
            {
                onDamageableHit.Invoke(damageable, this);
                damageable.HitTakeDamage(this.gameObject, damage);
            }
            else
            {
                onNonDamageableHit.Invoke(this);
            }
        }
    }

    public void ResetHitColliders()
    {
        hitColliders.Clear();
    }
}
