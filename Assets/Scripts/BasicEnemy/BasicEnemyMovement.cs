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
    public Animator animator;
    SpriteRenderer enemySprite;
    float playerX;
    float playerY;
    float enemyX;
    float enemyY;
    float distance;
    static readonly public int stopedTriggerHash = Animator.StringToHash("Stoped");
    static readonly public int deadTriggerHash = Animator.StringToHash("Dead");

    // Função que inicializa todos os status do inimigo
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

    // Função que Inicializa as variáveis locais após a primeira mudança de frame
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemySprite = GetComponent<SpriteRenderer>();
    }

    // Variável que redefine valores a cada mudança de frame
    void Update()
    {
        playerX = player.transform.position.x;
        enemyX = transform.position.x;
        playerY = player.transform.position.y;
        enemyY = transform.position.y;

        distance = Vector2.Distance(new Vector2(enemyX, enemyY), new Vector2(playerX, playerY));

        if (distance <= maxDist)
        {
            animator.SetBool(stopedTriggerHash, true);
            moveSpeed = 0;
        }
        else
        {
            animator.SetBool(stopedTriggerHash, false);
            moveSpeed = 3;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }

        // Atualizar o lado que o inimigo está olhando
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
        yield return new WaitForSeconds(1.6f);  // Tempo para a animação de morte ocorrer
        Destroy(gameObject);  // Destrói a instância deste inimigo
    }
}
