using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    private AbstractPlayerBehaviourHandler player;
    public GameObject enemyPrefab;  
    public float spawnDelay = 5f;  

    void Start()
    {
        player = AbstractPlayerBehaviourHandler.ActivePlayer;
        StartCoroutine(GenerateEnemy());
    }

    // Corrotina para gerar inimigos
    IEnumerator GenerateEnemy()
    {
        while (player.character.damageable.CurrentHealth > 0)  // Enquanto o jogador estiver vivo
        {
            yield return new WaitForSeconds(spawnDelay);  // Aguarda o tempo definido

            // Instancia um novo inimigo a partir do prefab original
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
            newEnemy.SetActive(true);

        }
    }

    
}
