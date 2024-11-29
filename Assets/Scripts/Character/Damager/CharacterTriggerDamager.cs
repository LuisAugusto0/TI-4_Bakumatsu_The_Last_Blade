using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterTriggerDamager : TriggerDamager
{
    [Tooltip("Damage calculated based on character damage values and this multiplier")]
    public float damagePorcentage = 1;
    
    // From parent

    public void Initialize(Character character, CharacterDamage characterDamage)
    {
        character.OnFlipX += FlipX;

        characterDamage.onDamageChange.AddListener(UpdateDamage);
        onDamageableHit.AddListener((target, self) => characterDamage.onAttackHit.Invoke(target, self));
        onDamageableKill.AddListener((target, self) => characterDamage.onKill.Invoke(target, self));
        UpdateDamage(characterDamage.CurrentDamage);
    }




    void UpdateDamage(int characterDamage)
    {
        damage = Mathf.RoundToInt(damagePorcentage * characterDamage);
    }

}
