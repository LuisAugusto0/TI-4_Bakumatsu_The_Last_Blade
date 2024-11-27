using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameplayUI : MonoBehaviour
{
    public GameObject heartPrefab; // Prefab de um coração para instanciar
    public Transform heartsContainer; // Contêiner onde os corações serão exibidos
    public Sprite fullHeart;  // Sprite de coração cheio
    public Sprite halfHeart;  // Sprite de coração meio vazio
    public Sprite emptyHeart; // Sprite de coração vazio

    private Damageable player; // Referência ao jogador
    private List<Image> heartImages = new List<Image>(); // Lista dinâmica de corações

    void Start()
    {
        // Tenta encontrar o jogador na cena
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<Damageable>();
            if (player == null)
            {
                Debug.LogError("Componente Damageable não encontrado no jogador!");
            }
            else
            {
                // Garante a criação inicial de corações
                UpdateHeartsUI();
            }
        }
        else
        {
            Debug.LogError("Jogador não encontrado! Certifique-se de que o jogador tenha a tag 'Player'.");
        }
    }

    // Atualiza os ícones de coração com base no HP atual e máximo
    public void UpdateHearts()
    {

        if (heartImages == null || heartImages.Count == 0)
        {
            Debug.LogWarning("Nenhum coração disponível para atualizar. Certifique-se de que UpdateHeartsUI foi chamado.");
            UpdateHeartsUI(); // Garante que os corações sejam criados
            return;
        }

        for (int i = 0; i < heartImages.Count; i++)
        {
            int heartStatus = (player.CurrentHealth - (i * 2));

            if (heartStatus >= 2)
            {
                heartImages[i].sprite = fullHeart;
            }
            else if (heartStatus == 1)
            {
                heartImages[i].sprite = halfHeart;
            }
            else
            {
                heartImages[i].sprite = emptyHeart;
            }
        }
    }

    // Ajusta o número de corações exibidos com base no HP máximo
    public void UpdateHeartsUI()
    {
        if (heartPrefab == null || heartsContainer == null)
        {
            Debug.LogError("heartPrefab ou heartsContainer não está configurado!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Jogador não inicializado ao configurar corações.");
            return;
        }

        // Calcula a quantidade necessária de corações com base no HP máximo
        int heartCount = Mathf.CeilToInt(player.CurrentHealth / 2f);

        // Adiciona ou remove corações conforme necessário
        while (heartImages.Count < heartCount)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartsContainer);
            newHeart.transform.SetParent(heartsContainer, false); // Adiciona ao container sem alterar o scale
            Image heartImage = newHeart.GetComponent<Image>();
            if (heartImage != null)
            {
                heartImages.Add(heartImage);
            }
            else
            {
                Debug.LogWarning("Prefab de coração não contém um componente Image.");
            }
        }

        while (heartImages.Count > heartCount)
        {
            Destroy(heartImages[heartImages.Count - 1].gameObject);
            heartImages.RemoveAt(heartImages.Count - 1);
        }

        UpdateHearts(); // Atualiza os estados dos corações

        // Atualiza a largura do container de acordo com os corações
        UpdateContainerWidth();
    }

    // Método para ajustar a largura do container
    private void UpdateContainerWidth()
    {
        // Calcula a largura necessária para os corações e o espaçamento
        float totalWidth = 0f;
        foreach (var heart in heartImages)
        {
            // Aqui você pode calcular o tamanho de cada coração e o espaçamento
            totalWidth += heart.preferredWidth + 10f;
        }

        // Atualiza a largura do container
        RectTransform containerRect = heartsContainer.GetComponent<RectTransform>();
        containerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth);
    }


}
