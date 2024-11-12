using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterTriggerDamager : TriggerDamager
{
    [Tooltip("Damage calculated based on character damage values and this multiplier")]
    public float bonusDamageMultiplier = 1;
    
    // From parent
    public CharacterDamage characterDamage;

    protected override void Awake()
    {
        base.Awake();
        characterDamage.onDamageChange.AddListener(UpdateDamage);
        
    }
   
    protected override void Start()
    {
        base.Start();
        UpdateDamage(characterDamage.CurrentDamage);
    }

    void UpdateDamage(int characterDamage)
    {
        damage = Mathf.RoundToInt(bonusDamageMultiplier * characterDamage);
    }

}
