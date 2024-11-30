using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public Transform[] spawnPoints;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        // Selecciona un punto de spawn aleatorio
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instancia el enemigo
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}
