/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    private BasePlayerBehaviour player;
    public GameObject enemyPrefab;  
    public float spawnDelay = 5f;  

    void Start()
    {
        player = BasePlayerBehaviour.ActivePlayer;
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

    
}*///Código Original

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{   
    public int [] chanceOfEnemies;
    public Transform [] _spawnPoints;
    public GameObject [] _enemiesPrefab;
    protected int enemyCount = 0;

    public float _spawnBreakTime = 2f;

    public float _hordeBreakTime = 5f;

    public float _hordeDurationTime = 10f;

    public int _currentHorde = 0;

    private float _spawnTimer = 0f;

    private float _hordeBreakTimer = 0f;

    private float _hordeTimer = 0f;

    void Start()
    {   
        
        _spawnTimer = 0f;
        _hordeBreakTimer = _hordeBreakTime;
        ValidateEnemyChances();
        
    }

    void ValidateEnemyChances()
    {
        if(chanceOfEnemies.Length != _enemiesPrefab.Length)
            Debug.LogError("Chance of enemies must have the same length as the enemies prefab array");
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(_hordeBreakTimer < _hordeBreakTime){
            
            if(_hordeBreakTimer - (int) _hordeBreakTimer < 0.01f){
                Debug.Log("Horda começa em: " + ((int)(_hordeBreakTime - _hordeBreakTimer)));
            }
            _hordeBreakTimer += Time.deltaTime;
        } else {
            
            if(_hordeTimer < _hordeDurationTime){
                if(_hordeTimer - (int) _hordeTimer < 0.01f){
                    Debug.Log("A Horda acaba em: " + ((int)(_hordeDurationTime - _hordeTimer)));
                }
                if(_spawnTimer >= _spawnBreakTime){
                    int enemyIndex = GetEnemyIndex();
                    int spawnIndex = Random.Range(0, _spawnPoints.Length);
                    GameObject obj = Instantiate(_enemiesPrefab[enemyIndex], _spawnPoints[spawnIndex].position, Quaternion.identity);
                    obj.SetActive(true);
                    _spawnTimer = 0f; // Reset the timer after spawning an enemy
                }
                _spawnTimer += Time.deltaTime;
                _hordeTimer += Time.deltaTime;
            }
            else{
                _hordeDurationTime += _hordeDurationTime * 0.1f;
                _hordeBreakTimer = 0f;
                _hordeTimer = 0f;
                _currentHorde++;
            }
        }
    }

    int GetEnemyIndex()
    {
        int totalChance = 0;
        foreach (int chance in chanceOfEnemies)
        {
            totalChance += chance;
        }

        int randomValue = Random.Range(0, totalChance);
        int cumulative = 0;
        for (int i = 0; i < chanceOfEnemies.Length; i++)
        {
            cumulative += chanceOfEnemies[i];
            if (randomValue < cumulative)
            {
                return i;
            }
        }
        return 0; // Default case, should not reach here if chances are valid
    }
}

