using UnityEngine;
using TMPro;

public class hordeStatsUI : MonoBehaviour
{
    public TextMeshProUGUI currentHorde;
    public TextMeshProUGUI nextHorde;

    void Update()
    {
        // Certifique-se de que a instância de EnemyGenerator existe antes de acessar
        if (EnemyGenerator.Instance != null)
        {
            currentHorde.text = "Horda atual: " + EnemyGenerator.Instance._currentHorde.ToString();
            nextHorde.text = "Proxima horda em: " + EnemyGenerator.Instance.nextHorde.ToString();
        }
        else
        {
            Debug.LogWarning("EnemyGenerator.Instance não está inicializado.");
        }
    }
}
