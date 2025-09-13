using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Класс для описания волны врагов
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public int numberOfEnemies;
        public GameObject enemyPrefab;
        public float spawnDelay;
    }

    public List<Wave> waves;
    public float timeBetweenWaves = 5f;
    public bool continuousSpawning = false;
    public GameObject spawnAreaVisualizer;

    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    private BoxCollider2D spawnerCollider;

    void Start()
    {
        spawnerCollider = GetComponent<BoxCollider2D>();
        if (spawnerCollider == null)
        {
            Debug.LogError("EnemySpawner requires a BoxCollider2D to define the spawn area!");
            this.enabled = false;
            return;
        }

        if (spawnAreaVisualizer != null)
        {
            // Устанавливаем масштаб визуализатора в соответствии с размером BoxCollider2D
            // Примечание: Vector3(size.x, size.y, 1) для 2D-визуализатора
            spawnAreaVisualizer.transform.localScale = new Vector3(spawnerCollider.size.x, spawnerCollider.size.y, 1);
        }

        StartSpawning();
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnWaves());
        }
    }

    public void StopSpawning()
    {
        if (isSpawning)
        {
            isSpawning = false;
            StopAllCoroutines();
        }
    }

    IEnumerator SpawnWaves()
    {
        while (isSpawning)
        {
            if (currentWaveIndex < waves.Count)
            {
                Wave currentWave = waves[currentWaveIndex];
                Debug.Log($"Starting wave: {currentWave.waveName}");

                for (int i = 0; i < currentWave.numberOfEnemies; i++)
                {
                    Vector3 spawnPosition = GetRandomPointInCollider();
                    Instantiate(currentWave.enemyPrefab, spawnPosition, Quaternion.identity);
                    yield return new WaitForSeconds(currentWave.spawnDelay);
                }

                currentWaveIndex++;

                if (currentWaveIndex < waves.Count)
                {
                    Debug.Log($"Wave {currentWave.waveName} finished. Waiting for next wave in {timeBetweenWaves} seconds.");
                    yield return new WaitForSeconds(timeBetweenWaves);
                }
                else if (continuousSpawning)
                {
                    Debug.Log("All waves completed. Restarting from the beginning.");
                    currentWaveIndex = 0;
                    yield return new WaitForSeconds(timeBetweenWaves);
                }
                else
                {
                    Debug.Log("All waves completed. Spawning stopped.");
                    StopSpawning();
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    private Vector3 GetRandomPointInCollider()
    {
        Bounds bounds = spawnerCollider.bounds;
        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            0 // z-координата должна быть 0 для 2D-игры
        );

        return randomPoint;
    }

    void OnDrawGizmos()
    {
        BoxCollider2D box2D = GetComponent<BoxCollider2D>();
        if (box2D != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(box2D.bounds.center, box2D.bounds.size);
        }
    }
}