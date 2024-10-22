using UnityEngine;
using UnityEngine.SceneManagement;  // Necessário para carregar outras cenas

public class PauseMenu : SceneSingleton<PauseMenu>
{
    public GameObject pauseMenuUI;  // O Canvas dentro do objeto

    public override void Awake()
    {
        base.Awake();
        Time.timeScale = 1f;  // Certificar-se de que o tempo está correndo normalmente no início
    }
    



    // --- Eventos chamados pelo Gameplay Input Handler no jogador --- 
    // Retoma o jogo
    public void Unpause()
    {
        pauseMenuUI.SetActive(false);  // Esconde o Canvas do menu de pausa
        Time.timeScale = 1f;  // Retorna o tempo à escala normal
    }

    // Pausa o jogo
    public void BeginPause()
    {
        pauseMenuUI.SetActive(true);  // Exibe o Canvas do menu de pausa
        Time.timeScale = 0f;  // Congela o tempo do jogo
    }



    // --- Eventos chamados pelo canvas ---
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
