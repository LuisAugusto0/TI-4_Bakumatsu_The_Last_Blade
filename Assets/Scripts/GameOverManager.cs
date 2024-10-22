using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : SceneSingleton<GameOverManager>
{
    public CanvasGroup gameOverCanvasGroup;

    public float delayUntilTransition = 1f;
    public float fadeDuration = 0.8f;

    public void StartGameOver()
    {
        StartCoroutine(ShowGameOverScreenWithFade());
    }

    
    // Corrotina para fazer o fade in da tela de Game Over
    public IEnumerator ShowGameOverScreenWithFade()
    {

        yield return new WaitForSeconds(delayUntilTransition); // Espera pelo tempo de atraso

        // Inicia com o alpha (transparência) do CanvasGroup em 0 (totalmente invisível)
        gameOverCanvasGroup.gameObject.SetActive(true); // Ativa o Game Over UI
        gameOverCanvasGroup.alpha = 0f;

        float timer = 0f;

        // Loop para fazer o fade in
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime; // Incrementa o tempo
            gameOverCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration); // Interpola o alpha
            yield return null; // Espera até o próximo frame
        }

        gameOverCanvasGroup.alpha = 1f; // Garante que o alpha seja 1 (completamente visível) no final
    }
}
