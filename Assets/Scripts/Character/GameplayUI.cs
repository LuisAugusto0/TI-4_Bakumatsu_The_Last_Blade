using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public Image[] heartImages;  // Array de imagens dos corações
    public Sprite fullHeart;     // Sprite de coração cheio (2 HP)
    public Sprite halfHeart;     // Sprite de coração meio vazio (1 HP)
    public Sprite emptyHeart;    // Sprite de coração vazio (0 HP)

    private Damageable player;  // Referência ao jogador para acessar o HP

        void Start()
    {
        // Procura pelo jogador usando a tag "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Damageable>();  // Encontra o componente Damageable no jogador
            UpdateHearts();
        }
        else
        {
            Debug.LogError("Jogador não encontrado!");
        }
    }



    // Método para atualizar os corações
    public void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            int heartStatus = (player.CurrentHealth - (i * 2));  // Determina o status de cada coração

            if (heartStatus >= 2)
            {
                heartImages[i].sprite = fullHeart;  // Coração cheio
            }
            else if (heartStatus == 1)
            {
                heartImages[i].sprite = halfHeart;  // Coração meio vazio
            }
            else
            {
                heartImages[i].sprite = emptyHeart;  // Coração vazio
            }
        }
    }
}
