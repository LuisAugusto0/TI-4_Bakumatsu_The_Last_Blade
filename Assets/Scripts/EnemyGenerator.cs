using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public PlayerController player;
    public GameObject enemyPrefab;  // Prefab do inimigo
    public float spawnDelay = 5f;   // Tempo entre cada gera��o de inimigos

    // Start � chamado uma vez no in�cio
    void Start()
    {
        StartCoroutine(GenerateEnemy());
    }

    // Corrotina para gerar inimigos
    IEnumerator GenerateEnemy()
    {
        while (player.hp > 0)  // Enquanto o jogador estiver vivo
        {
            yield return new WaitForSeconds(spawnDelay);  // Aguarda o tempo definido

            // Instancia um novo inimigo a partir do prefab original
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
            newEnemy.SetActive(true);

            // Voc� pode adicionar l�gica adicional para o inimigo, se necess�rio
        }
    }
}
