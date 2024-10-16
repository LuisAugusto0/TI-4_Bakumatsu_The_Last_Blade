using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    // Static management for unique GameOverManager
    public static GameOverManager Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;


            Create ();

            return s_Instance;
        }
    }
    protected static GameOverManager s_Instance;

    public static void Create ()
    {
        GameOverManager controllerPrefab = Resources.Load<GameOverManager> ("GameOverManager");
        s_Instance = Instantiate (controllerPrefab);
    }



    public GameObject gameOverScreen;
    public CanvasGroup gameOverCanvasGroup;
    public float delayUntilTransition = 1f;
    public float fadeDuration = 0.8f;

    void Awake()
    {
        s_Instance = this;
    }

    public void StartGameOver()
    {
        StartCoroutine(ShowGameOverScreenWithFade());
    }
    
    // Corrotina para fazer o fade in da tela de Game Over
    public IEnumerator ShowGameOverScreenWithFade()
    {

        yield return new WaitForSeconds(delayUntilTransition); // Espera pelo tempo de atraso

        // Inicia com o alpha (transparência) do CanvasGroup em 0 (totalmente invisível)
        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.gameObject.SetActive(true); // Ativa o Game Over UI

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
