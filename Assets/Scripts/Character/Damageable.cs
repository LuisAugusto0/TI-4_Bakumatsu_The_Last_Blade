using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    [Serializable]
    public class HealthSetEvent : UnityEvent<Damageable>
    { }
    
    [Serializable]
    public class HealEvent : UnityEvent<int, Damageable>
    { }

    [Serializable]
    public class OnHitEvent : UnityEvent<GameObject, Damageable>
    { }

    [Serializable]
    public class DeathEvent : UnityEvent<Damageable>
    { }


    [Serializable]
    public class AddedImmunityEvent : UnityEvent<Damageable>
    { }

    [Serializable]
    public class EndedImmunityEvent : UnityEvent<Damageable>
    { }

    public float onHitImmunityTime = 2f;

    
    public int baseHealth = 6;
    public int currentHealth = 6;


    [NonSerialized]
    public bool isDead = false;

    
    // Track different sources of immunity
    private HashSet<object> _immunitySources = new();

    
    public HealthSetEvent onHealthSet;
    public HealEvent onHeal;
    public OnHitEvent onHit;
    public DeathEvent onDeath;
    public AddedImmunityEvent addedImmunity;
    public EndedImmunityEvent endedImmunity; 


    // Health setters

    public void SetBaseHealth(int newBaseHealth)
    {
        if (newBaseHealth <= 0) 
        {
            Debug.LogWarningFormat(
                "Illegal health value of {0} received for {1} with ID {2}",
                newBaseHealth, GetType().Name, GetInstanceID()
            );    
        }
        baseHealth = newBaseHealth;
    }

    public void SetHealth(int health)
    {
        if (health <= 0) 
        {
            Debug.LogWarningFormat(
                "Illegal health value of {0} received for {1} with ID {2}",
                health, GetType().Name, GetInstanceID()
            );    
        }
        currentHealth = Math.Min(baseHealth, health);
    }

    
    // Methods for handling immunity
    public bool IsImmune()
    {
        return _immunitySources.Count > 0;
    } 

    IEnumerator OnHitImmunity(GameObject damager)
    {
        AddImmunity(damager);
        
        yield return new WaitForSeconds(onHitImmunityTime);

        RemoveImmunity(damager);
    }

    public void AddImmunity(object source)
    {
        if (source == null)
        {
            Debug.LogError("Immunity source cannot be null!");
            return;
        }

        _immunitySources.Add(source);
        addedImmunity.Invoke(this);
    }

    public void RemoveImmunity(object source)
    {
        if (source == null)
        {
            Debug.LogError("Immunity source cannot be null!");
            return;
        }

        if (_immunitySources.Contains(source))
        {
            _immunitySources.Remove(source);

            // Removed last stack of immunity
            if (!IsImmune()) endedImmunity.Invoke(this); 
        }
    }

    public void ClearAllImmunity()
    {
        _immunitySources.Clear();
    }


    // Main TakeDamage (calle from Damager scripts)
    public void TakeDamage(GameObject damagerObject, int damage) 
    {
        if (!IsImmune())
        {
            currentHealth -= damage;
            onHit.Invoke(damagerObject, this);

            FindObjectOfType<GameplayUI>().UpdateHearts();

            if (currentHealth <= 0)
            {
                Death();
            }

            // Use object as unique hash identifier
            StartCoroutine(OnHitImmunity(damagerObject));
        }
        
    }

    public void Heal(int value)
    {
        currentHealth = Math.Min(baseHealth, currentHealth + value);
        onHeal.Invoke(value, this);

        FindObjectOfType<GameplayUI>().UpdateHearts();
    }

    public void Death()
    {
        isDead = true;
        onDeath.Invoke(this);
        AddImmunity(this);
    }


}
