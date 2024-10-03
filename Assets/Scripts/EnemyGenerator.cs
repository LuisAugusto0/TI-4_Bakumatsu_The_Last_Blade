using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public PlayerController player;
    public GameObject enemyPrefab;  // Prefab do inimigo
    public float spawnDelay = 5f;   // Tempo entre cada geração de inimigos

    // Start é chamado uma vez no início
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

            // Você pode adicionar lógica adicional para o inimigo, se necessário
        }
    }
}
