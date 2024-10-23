using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Damageable))]
public class CharacterHealthEffects : MonoBehaviour
{
    public Color flashColor = Color.red;
    public Color immunityColor = Color.white;

    public float damageFlashDuration = 0.1f;
    public float delay = 0.1f;

    Character _character;
    Damageable _damageable;
    Color _originalColor;
    bool isOnFlashEffect = false;

    void Start()
    {
        _character = GetComponent<Character>();
        _damageable = GetComponent<Damageable>();
        _originalColor = _character.spriteRenderer.color;

    }

    public void AttemptImmunityEffect()
    {
        if (_damageable.IsImmune() && !isOnFlashEffect)
        {
            _character.spriteRenderer.color = immunityColor;
        }
    }

    public void EndImmunityEffect()
    {
        _character.spriteRenderer.color = _originalColor;
    }



    // Called on event OnHit() by Damageable
    public void DamageFlash()
    {
        StartCoroutine(ColorFlashCoroutine(flashColor));
    }

    private IEnumerator ColorFlashCoroutine(Color flashColor)
    {
        _character.spriteRenderer.color = flashColor;
        _character.StartActionLock(null, this);
        isOnFlashEffect = true;

        yield return new WaitForSeconds(damageFlashDuration);

        _character.spriteRenderer.color = _originalColor;
        
        _character.EndActionLock(this);

        isOnFlashEffect = false;

        AttemptImmunityEffect();
    }   



    // Shaders not implemented. Using color for now instead

    // private IEnumerator FlashCoroutine()
    // {
    //     // Enable emission and set it to full white
    //     character.spriteRenderer.material.EnableKeyword("_EMISSION");
    //     character.spriteRenderer.material.SetColor("_EmissionColor", Color.white);

    //     // Wait for the defined flash duration
    //     yield return new WaitForSeconds(damageFlashDuration);

    //     // Disable emission to return the material to its original state
    //     character.spriteRenderer.material.DisableKeyword("_EMISSION");
    // }
}
