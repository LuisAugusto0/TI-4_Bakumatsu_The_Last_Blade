using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Character))]
public class CharacterDamage : MonoBehaviour
{

    [Serializable]
    public class CharacterDamageChangeEvent : UnityEvent<int> {}

    [Serializable]
    public class OnAttackHit : UnityEvent<Damageable, TriggerDamager> {}

    [Serializable]
    public class OnKill : UnityEvent<Damageable, TriggerDamager> {}

    [SerializeField] 
    int baseDamage = 1;

    [SerializeField] //for debug
    int damageBonus = 0;

    [SerializeField] //for debug
    float damageMultiplier = 1;

    public int CurrentDamage { get{return currentDamage;}}
    [SerializeField] //for debug
    int currentDamage;


    public CharacterDamageChangeEvent onDamageChange;
    public OnAttackHit onAttackHit;
    public OnKill onKill;

    void Start()
    {
        UpdateCurrentDamage();
        onKill.AddListener((target, damage)=>Debug.Log("KILL"));
    }

    public void AddDamageBonus(int value)
    {
        damageBonus += value;
        UpdateCurrentDamage();
    }

    
    public void AddDamageMultiplier(float value)
    {
        damageMultiplier *= value;
        UpdateCurrentDamage();
    }

    public void AddDamageMultiplierAndNotUpdate(float value)
    {
        damageMultiplier *= value;
    }


    public void RemoveDamageMultiplier(float value)
    {
        damageMultiplier /= value;
        UpdateCurrentDamage();    
    }

    public void RemoveDamageMultiplierAndNotUpdate(float value)
    {
        damageMultiplier /= value;
    }



    void UpdateCurrentDamage()
    {
       
        int totalDamageValue = baseDamage + damageBonus;
        if (totalDamageValue <= 0)
        {
            currentDamage = 0;
        }
        else
        {
            currentDamage = Mathf.RoundToInt(totalDamageValue * damageMultiplier);
        }

        onDamageChange.Invoke(currentDamage);
        
    }



}
