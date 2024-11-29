using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{

    public TextMeshProUGUI hordeText;

    void Start()
    {
        hordeText.text = "Hordas sobrevividas: " + EnemyGenerator.LastHordeSurvived.ToString();
    }

    
    public void RestartButton(){
        SceneManager.LoadScene("Gameplay");
    }

    public void MenuButton(){
        SceneManager.LoadScene("MainMenu");
    }

    
}
