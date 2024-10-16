using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Damager : MonoBehaviour
{
    [Serializable]
    public class DamageableEvent : UnityEvent<Damageable, Damager>
    { }
    
    [Serializable]
    public class NonDamageableEvent : UnityEvent<Damager>
    { }

    public int damage = 1;
    public LayerMask hittableLayers;
    public DamageableEvent onDamageableHit;
    public NonDamageableEvent onNonDamageableHit;
    
    private Collider2D m_collider;
    public SpriteRenderer characterSpriteRenderer;
    private Vector2 originalColliderOffset;
    
    void Awake()
    {
        m_collider = GetComponent<Collider2D>();
        originalColliderOffset = m_collider.offset;
        enabled = false;
    }

    void OnEnable()
    {
        m_collider.enabled = true;
        AdjustColliderBasedOnSpriteFlip();
    }

    void OnDisable()
    {
        m_collider.enabled = false;
    }

    void AdjustColliderBasedOnSpriteFlip()
    {
        if (characterSpriteRenderer.flipX)
        {
            m_collider.offset = new Vector2(-originalColliderOffset.x, originalColliderOffset.y);
        }
        else
        {
            m_collider.offset = originalColliderOffset;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        int layer = collider.gameObject.layer;

        // Layer is included in the LayerMask
        if ((hittableLayers & (1 << layer)) != 0)
        {
            Damageable damageable = collider.GetComponent<Damageable>();
            if (damageable != null)
            {
                onDamageableHit.Invoke(damageable, this);
                damageable.TakeDamage(this);
            }
            else 
            {
                onNonDamageableHit.Invoke(this);
            }
        }
    }

    
}
