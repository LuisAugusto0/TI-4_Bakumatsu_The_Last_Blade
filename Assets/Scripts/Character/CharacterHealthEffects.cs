using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Character))]
public class CharacterHealthEffects : MonoBehaviour
{
    Character character;
    public float damageFlashDuration = 0.1f;
    Color originalColor;

    void Start()
    {
        character = GetComponent<Character>();
        originalColor = character.spriteRenderer.color;
    }

    public void DamageFlash()
    {
        //StartCoroutine(FlashCoroutine());
        StartCoroutine(ColorFlashCoroutine(Color.black));
    }

    

    private IEnumerator FlashCoroutine()
    {
        // Enable emission and set it to full white
        character.spriteRenderer.material.EnableKeyword("_EMISSION");
        character.spriteRenderer.material.SetColor("_EmissionColor", Color.white);

        // Wait for the defined flash duration
        yield return new WaitForSeconds(damageFlashDuration);

        // Disable emission to return the material to its original state
        character.spriteRenderer.material.DisableKeyword("_EMISSION");
    }

    private IEnumerator ColorFlashCoroutine(Color flashColor)
    {
        character.spriteRenderer.color = flashColor;

        yield return new WaitForSeconds(damageFlashDuration);

        character.spriteRenderer.color = originalColor;
    }

    public void ChangeToDamageShader()
    {
        character.spriteRenderer.color = Color.black;
    }

    public void ResetShader()
    {
        character.spriteRenderer.color = originalColor;
    }
}
