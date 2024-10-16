using UnityEngine;
using UnityEngine.SceneManagement;  // Necessário para carregar outras cenas

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // O Canvas do menu de pausa

    private bool isPaused = false;  // Controle do estado de pausa

    void Start()
    {
        pauseMenuUI.SetActive(false);  // Garantir que o Canvas esteja invisível no início
        Time.timeScale = 1f;  // Certificar-se de que o tempo está correndo normalmente no início
    }

    void Update()
    {
        // Verifica se o jogador apertou a tecla ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();  // Se estiver pausado, retoma o jogo
            }
            else
            {
                Pause();  // Caso contrário, pausa o jogo
            }
        }
    }

    // Retoma o jogo
    public void Resume()
    {
        pauseMenuUI.SetActive(false);  // Esconde o Canvas do menu de pausa
        Time.timeScale = 1f;  // Retorna o tempo à escala normal
        isPaused = false;  // Atualiza o estado de pausa
    }

    // Pausa o jogo
    void Pause()
    {
        pauseMenuUI.SetActive(true);  // Exibe o Canvas do menu de pausa
        Time.timeScale = 0f;  // Congela o tempo do jogo
        isPaused = true;  // Atualiza o estado de pausa
    }

    // Método para reiniciar a cena atual
    public void RestartGame()
    {
        Time.timeScale = 1f;  // Certifica-se de que o tempo está normal antes de reiniciar
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Recarrega a cena atual
    }

    // Método para ir ao menu principal
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;  // Certifica-se de que o tempo está normal antes de carregar o menu
        SceneManager.LoadScene("MainMenu");  // Substitua "MainMenu" pelo nome da sua cena de menu
    }
}
