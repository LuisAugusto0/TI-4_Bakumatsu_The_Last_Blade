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
        UpdateDamage(characterDamage.CurrentDamage);
    }




    void UpdateDamage(int characterDamage)
    {
        damage = Mathf.RoundToInt(damagePorcentage * characterDamage);
    }

}
