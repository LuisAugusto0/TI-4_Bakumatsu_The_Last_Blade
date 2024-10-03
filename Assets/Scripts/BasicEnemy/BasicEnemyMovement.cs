using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : EnemyData
{
    GameObject player;
    GameObject enemy;
    SpriteRenderer enemySprite;
    float playerX;
    float playerY;
    float enemyX;
    float enemyY;
    float distance;

    //Função que inicializa todos os status do inimigo
    void Awake()
    {
        maxDist = 1;
        maxHealt = 5;
        damage = 1;
    }

    //Função que Inicializa as variáveis locais após a primeira mudança de frame
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
        enemySprite = enemy.GetComponent<SpriteRenderer>();
    }

    //Variável que redefine valores a cada mudança de frame
    void Update()
    {
        playerX = player.transform.position.x;
        enemyX = enemy.transform.position.x;
        playerY = player.transform.position.y;
        enemyY = enemy.transform.position.y;
        distance = Vector2.Distance(
            new Vector2(enemyX, enemyY),
            new Vector2(playerX, playerY)
        );
        if (distance <= maxDist)
        {
            moveSpeed = 0;
            
        }
        else
        {
           moveSpeed = 3;
           transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        if (playerX - enemyX < 0)
        {
          enemySprite.flipX = true;
        }
        else
        {
            enemySprite.flipX = false;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerAttack"))
        {
            takeDamage(1);
        }
    }
}

