using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterTriggerDamager : TriggerDamager
{
    [Tooltip("Damage calculated based on character damage values and this multiplier")]
    public float damagePorcentage = 1;
    
    // From parent
    public CharacterDamage characterDamage;

    // Cant find a better solution to allow updating from outside
    public void UpdateCharacterDamage(CharacterDamage characterDamage)
    {
        characterDamage.onDamageChange.AddListener(UpdateDamage);
        UpdateDamage(characterDamage.CurrentDamage);
    }


    void UpdateDamage(int characterDamage)
    {
        damage = Mathf.RoundToInt(damagePorcentage * characterDamage);
    }

}
