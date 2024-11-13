using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(EntityMovement))]
[RequireComponent(typeof(CharacterDamage))]
public class EffectReceiver : MonoBehaviour
{    // Components that upgrade can request changes
    [NonSerialized] public Character character;
    [NonSerialized] public EntityMovement entityMovement;
    [NonSerialized] public Damageable damageable;
    [NonSerialized] public CharacterDamage characterDamage;

    void Awake()
    {
        character = GetComponent<Character>();
        entityMovement = GetComponent<EntityMovement>();
        damageable = GetComponent<Damageable>();
        characterDamage = GetComponent<CharacterDamage>();
    }
}
