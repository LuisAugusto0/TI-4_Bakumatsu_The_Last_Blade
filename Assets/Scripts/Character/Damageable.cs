using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

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
    public class DamagerEvent : UnityEvent<Damager, Damageable>
    { }

    [Serializable]
    public class DeathEvent : UnityEvent<Damageable>
    { }
  

    public int baseHealth = 5;
    public int currentHealth = 5;
    public float immunityTime = 2f;
    public bool isDead = false;
    
    // Ensures only unique object IDs are added
    private HashSet<object> _immunitySources = new();

    
    public HealthSetEvent onHealthSet;
    public HealEvent onHeal;
    public DamagerEvent onHit;
    public DeathEvent onDeath;

    private CharacterHealthEffects characterHealthEffects;
   
    void Awake()
    {
        characterHealthEffects = GetComponent<CharacterHealthEffects>();
    }

    public bool IsImmune()
    {
        return _immunitySources.Count > 0;
    } 

    public void AddImmunity(object source)
    {
        if (source == null)
        {
            Debug.LogError("Immunity source cannot be null!");
            return;
        }

        _immunitySources.Add(source);
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
        }
    }

    public void ClearAllImmunity()
    {
        _immunitySources.Clear();
    }




    public void TakeDamage(Damager damager) 
    {
        if (!IsImmune())
        {
            currentHealth -= damager.damage;
            onHit.Invoke(damager, this);
            if (currentHealth <= 0)
            {
                Death();
            }
            StartCoroutine(DamageImmunity(damager));
        }
        
    }

    IEnumerator DamageImmunity(Damager damager)
    {
        AddImmunity(damager);
        
        if (characterHealthEffects != null)
        {
            characterHealthEffects.ChangeToDamageShader();
        }

        yield return new WaitForSeconds(immunityTime);

        if (characterHealthEffects != null)
        {
            characterHealthEffects.ResetShader();
        }

        RemoveImmunity(damager);
    }

    // Other sources of damage
    public void TakeDamage(int damage)
    {
        if (!IsImmune())
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        isDead = true;
        onDeath.Invoke(this);
        AddImmunity(this);
    }

    public void Heal(int value)
    {
        currentHealth = Math.Min(baseHealth, currentHealth + value);
        onHeal.Invoke(value, this);
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
}
