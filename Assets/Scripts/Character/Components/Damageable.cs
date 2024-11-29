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
    public class OnHitEvent : UnityEvent<object, Damageable>
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

    
    [SerializeField] int baseHp = 6;
    [SerializeField] int baseHpBonus = 0;
    [SerializeField] int currentBaseHp = 6;

    public int CurrentHealth {get {return currentHp;}}
    public int CurrentBaseHealth {get {return currentBaseHp;}}
    [SerializeField] protected int currentHp = 6;


    [NonSerialized]
    public bool isDead = false;

    // Track different sources of immunity
    private HashSet<object> _immunitySources = new();

    public UniqueStatusEffectManager uniqueStatusEffectManager; // can be null


    public HealthSetEvent onHealthSet;
    public HealEvent onHeal;
    public OnHitEvent onHit;
    public DeathEvent onDeath;
    public AddedImmunityEvent addedImmunity;
    public EndedImmunityEvent endedImmunity; 


    void Start()
    {
        uniqueStatusEffectManager = GetComponent<UniqueStatusEffectManager>();
        currentHp = baseHp;
        RecalculateBaseHealth();
    }

    public void IncreaseBaseHealthBonus(int value)
    {
        baseHpBonus += value;
        currentHp += value; // Aumenta a saúde atual junto com o máximo permitido
        RecalculateBaseHealth();

        // Atualiza a interface quando o HP máximo muda
        // GameplayUI gameplayUI = FindObjectOfType<GameplayUI>();
        // if (gameplayUI != null)
        // {
        //     gameplayUI.UpdateHeartsUI();
        // }
    }

    public void RecalculateBaseHealth()
    {
        int newBaseHp = baseHp + baseHpBonus;

        if (newBaseHp <= 0)
        {
            currentBaseHp = 1;        
        }

        currentBaseHp = newBaseHp;
        onHealthSet?.Invoke(this);
        // GameplayUI gameplayUI = FindObjectOfType<GameplayUI>();
        // if (gameplayUI != null)
        // {
        //     gameplayUI.UpdateHearts();
        // }
        // else
        // {
        //     Debug.LogWarning("GameplayUI não encontrado! Certifique-se de que ele está na cena.");
        // }
    }

    
    // Health setters
    public void ResetHealth()
    {
        currentHp = currentBaseHp;
    }

    
    // Methods for handling immunity
    public bool IsImmune()
    {
        return _immunitySources.Count > 0;
    } 

    IEnumerator OnHitImmunity(object damager)
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
    public bool HitTakeDamage(object damagerObject, int damage) 
    {
        bool dead = false;
        if (!IsImmune())
        {
            currentHp -= damage;
            onHit.Invoke(damagerObject, this);

            FindObjectOfType<GameplayUI>().DrawHearts();

            if (currentHp <= 0)
            {
                dead = true;
                Death();
            }

            // Use object as unique hash identifier
            StartCoroutine(OnHitImmunity(damagerObject));
        }   
        return dead;
    }


    public void TakeDamage(object damagerObject, int damage) 
    {
        currentHp -= damage;
        onHit.Invoke(damagerObject, this);

        FindObjectOfType<GameplayUI>().DrawHearts();

        if (currentHp <= 0)
        {
            Death();
        }

        // Use object as unique hash identifier
        StartCoroutine(OnHitImmunity(damagerObject));
    }
    


    public void Heal(int value)
    {
        currentHp = Math.Min(baseHp, currentHp + value);
        onHeal.Invoke(value, this);

        FindObjectOfType<GameplayUI>().DrawHearts();
    }

    public void Death()
    {
        isDead = true;
        onDeath.Invoke(this);
        AddImmunity(this);
    }


}
