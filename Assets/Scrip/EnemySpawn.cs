using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [SerializeField] private GameObject enemyPrefab;
    private List<GameObject> enemies;
    private List<GameObject> enemyPool;
    private float timer = 5f;  // Timer to control spawn frequency
    [SerializeField] private float delay = 2f;  // Delay between spawns
    [SerializeField] private Vector3 spawnArea = new Vector3(100f, 0f, 100f);
    [SerializeField] private float groundY = 0f;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private float spawnDelay = 5f;
    private bool gameStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (enemyPrefab == null)
        {
            Debug.LogError("EnemyPrefab is not assigned! Please assign the enemyPrefab in the Inspector.");
            return;
        }

        enemies = new List<GameObject>();
        enemyPool = new List<GameObject>();

        // Initialize the enemy pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    private void Start()
    {
        // Start spawning after the spawnDelay
        Invoke(nameof(StartGame), spawnDelay);
    }

    private void StartGame()
    {
        gameStarted = true;
    }

    private void Update()
    {
        if (!gameStarted) return;

        // Check if enemies are dead and remove them
        CheckDead();

        // Spawn enemies based on the timer
        if (timer >= delay && enemies.Count < maxEnemies)
        {
            Spawn();
        }

        timer += Time.deltaTime;
    }

    public void SpawnNewEnemy()
    {
        Spawn();
    }

    private void Spawn()
    {
        timer = 0;  // Reset timer for next spawn

        GameObject enemy = GetEnemyFromPool();

        if (enemy != null)
        {
            // Generate random position within spawn area
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnArea.x, spawnArea.x),
                groundY,
                Random.Range(-spawnArea.z, spawnArea.z)
            );
            enemy.transform.position = randomPosition;
            enemy.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            enemy.SetActive(true);

            // Reset the enemy's health upon spawning
            EnemyCtrl enemyCtrl = enemy.GetComponent<EnemyCtrl>();
            if (enemyCtrl != null)
            {
                enemyCtrl.ResetEnemy();  // Reset health and other properties in the enemy
            }

            enemies.Add(enemy);  // Add to active enemies list
        }
    }

    private GameObject GetEnemyFromPool()
    {
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;  // Return an inactive enemy from the pool
            }
        }

        return null;  // If no available enemy, return null
    }

    private void CheckDead()
    {
        // Efficiently remove inactive enemies from the active list
        enemies.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);
    }
}
