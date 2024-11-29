using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{   
    public int[] chanceOfEnemies;
    public Transform[] _spawnPoints;
    public GameObject[] _enemiesPrefab;

    protected int enemyCount = 0;

    public float _spawnBreakTime = 2f;
    public float _hordeBreakTime = 5f;
    public float _hordeDurationTime = 10f;

    public int _currentHorde = 0;
    private float _spawnTimer = 0f;
    private float _hordeBreakTimer = 0f;
    private float _hordeTimer = 0f;
    private bool hordeReady = false;

    public static EnemyGenerator Instance { get; private set; }

    public static int LastHordeSurvived { get; private set; } = 0;

    public int nextHorde => Mathf.Max(0, (int)(_hordeBreakTime - _hordeBreakTimer));

    void Start()
    {   
        Instance = this;
        _currentHorde = 1;
        LastHordeSurvived = _currentHorde;
        _spawnTimer = 0f;
        _hordeBreakTimer = _hordeBreakTime;
        ValidateEnemyChances();
    }

    void ValidateEnemyChances()
    {
        if (chanceOfEnemies.Length != _enemiesPrefab.Length)
            Debug.LogError("Chance of enemies must have the same length as the enemies prefab array");
    }

    void Update()
    {
        if (_hordeBreakTimer < _hordeBreakTime)
        {
            // Contador do tempo de pausa
            _hordeBreakTimer += Time.deltaTime;

            // Verifica se a pausa terminou
            if (_hordeBreakTimer >= _hordeBreakTime && !hordeReady)
            {
                hordeReady = true; // Marca que a próxima horda está pronta para começar
                _currentHorde++;   // Incrementa a horda atual
                Debug.Log("Início da Horda " + _currentHorde);
            }
        }
        else
        {
            // Executa a lógica da horda
            if (_hordeTimer < _hordeDurationTime)
            {
                if (_spawnTimer >= _spawnBreakTime)
                {
                    SpawnEnemy();
                    _spawnTimer = 0f;
                }
                _spawnTimer += Time.deltaTime;
                _hordeTimer += Time.deltaTime;
            }
            else
            {
                // Finaliza a horda e prepara para a próxima
                EndCurrentHorde();
            }
        }
    }


    void SpawnEnemy()
    {
        int enemyIndex = GetEnemyIndex();
        int spawnIndex = Random.Range(0, _spawnPoints.Length);
        GameObject obj = Instantiate(_enemiesPrefab[enemyIndex], _spawnPoints[spawnIndex].position, Quaternion.identity);
        obj.SetActive(true);
    }

    void EndCurrentHorde()
    {
        LastHordeSurvived = _currentHorde;
        _hordeBreakTimer = 0f;  // Reinicia o timer de pausa
        _hordeTimer = 0f;       // Reinicia o timer da duração da horda
        hordeReady = false;     // Reseta a flag para a próxima pausa
        _hordeDurationTime += _hordeDurationTime * 0.1f; // Aumenta a duração da próxima horda
        Debug.Log("Horda " + _currentHorde + " finalizada. Próxima horda em " + _hordeBreakTime + " segundos.");
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
        return 0;
    }
}
