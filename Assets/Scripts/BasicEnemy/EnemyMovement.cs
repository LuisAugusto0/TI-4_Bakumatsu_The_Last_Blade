using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    GameObject player;
    public float moveSpeed;
    GameObject enemy;
    SpriteRenderer enemySprite;
    float playerX;
    float playerY;
    float enemyX;
    float enemyY;
    float distance;
    public float maxDist;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
        enemySprite = enemy.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
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
}
