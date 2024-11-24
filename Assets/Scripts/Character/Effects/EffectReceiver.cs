using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(EntityMovement))]
[RequireComponent(typeof(CharacterDamage))]
public class EffectReceiver : MonoBehaviour
{    
    
    // Components that upgrade can request changes
    [NonSerialized] public Character character;
    [NonSerialized] public EntityMovement entityMovement;
    [NonSerialized] public Damageable damageable;
    [NonSerialized] public CharacterDamage characterDamage;

    readonly HashSet<IChargeStatusEffect> chargeStatusEffects = new();
    readonly HashSet<ITimedStatusEffect> timedStatusEffects = new();
    
    void Awake()
    {
        character = GetComponent<Character>();
        entityMovement = GetComponent<EntityMovement>();
        damageable = GetComponent<Damageable>();
        characterDamage = GetComponent<CharacterDamage>();
    }

    // Displayed as total time remaning
    public bool AddTimedStatusEffect(ITimedStatusEffect effect) => timedStatusEffects.Add(effect);
    public bool AddChargeStatusEffect(IChargeStatusEffect effect) => chargeStatusEffects.Add(effect);

    public bool RemoveTimedStatusEffect(ITimedStatusEffect effect) => timedStatusEffects.Remove(effect);
    public bool RemoveChargeStatusEffect(IChargeStatusEffect effect) => chargeStatusEffects.Remove(effect);

    void Update()
    {
        foreach (var effect in timedStatusEffects)
        {
            //Debug.Log(effect.GetType());
        }

        foreach (var effect in chargeStatusEffects)
        {
            //Debug.Log(effect.GetType());
        }
    }

}
