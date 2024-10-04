using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float moveSpeed = 3;
    public float maxHealt = 20;
    public float maxDist = 1;
    public float damage = 1;
    public float currentDamage = 1;
    public float currentMoveSpeed = 3;
    public float currentHealt = 20;
    GameObject player;
    GameObject enemyAttack;
    public Animator animator;
    SpriteRenderer enemySprite;
    float playerX;
    float playerY;
    float enemyX;
    float enemyY;
    float distance;
    static readonly public int stopedTriggerHash = Animator.StringToHash("Stoped");
    static readonly public int deadTriggerHash = Animator.StringToHash("Dead");

    // Fun��o que inicializa todos os status do inimigo
    void Awake()
    {
        animator.SetBool(deadTriggerHash, false);
        maxDist = 1;
        maxHealt = 1;
        damage = 5;
        currentDamage = damage;
        currentHealt = maxHealt;
        currentMoveSpeed = moveSpeed;
    }

    // Fun��o que Inicializa as vari�veis locais ap�s a primeira mudan�a de frame
    void Start()
    {
        enemyAttack = GameObject.FindGameObjectWithTag("AtaqueInimigo");
        enemyAttack.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        enemySprite = GetComponent<SpriteRenderer>();
    }

    // Vari�vel que redefine valores a cada mudan�a de frame
    void Update()
    {
        playerX = player.transform.position.x;
        enemyX = transform.position.x;
        playerY = player.transform.position.y;
        enemyY = transform.position.y;

        distance = Vector2.Distance(new Vector2(enemyX, enemyY), new Vector2(playerX, playerY));

        if (distance <= maxDist)
        {
            enemyAttack.SetActive(true);
            animator.SetBool(stopedTriggerHash, true);
            moveSpeed = 0;
        }
        else
        {
            enemyAttack.SetActive(false);
            animator.SetBool(stopedTriggerHash, false);
            moveSpeed = 3;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }

        // Atualizar o lado que o inimigo est� olhando
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

    void takeDamage(float dmg)
    {
        currentHealt -= dmg;

        if (currentHealt <= 0)
        {
            animator.SetBool(deadTriggerHash, true);
            moveSpeed = 0;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        player = this; // Referenciando o player a si mesmo para quando morrer ele ficar parado
        yield return new WaitForSeconds(1.6f);  // Tempo para a anima��o de morte ocorrer
        Destroy(gameObject);  // Destr�i a inst�ncia deste inimigo
    }

    //função implitcita para retornar o gameObject do proprio inimigo com this
    public static implicit operator GameObject(Enemy v)
    {
        return v.gameObject;
    }
}
