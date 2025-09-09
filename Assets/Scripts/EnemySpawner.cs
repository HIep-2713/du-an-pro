using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    [Header("Spawn Settings")]
    public float initialSpawnInterval = 1.5f; // ban ??u spawn nhanh h?n
    public float minSpawnInterval = 0.3f;     // gi?i h?n th?p h?n
    public float difficultyIncreaseRate = 0.1f; // m?i l?n gi?m nhi?u h?n
    private float spawnInterval;
    private float spawnTimer;
    private float timeElapsed;

    [Header("Difficulty Settings")]
    public float difficultyStepTime = 5f; // gi?m sau m?i 5 giây

    [Header("Spawn Area")]
    public Transform topSpawn;
    public Transform bottomSpawn;
    public Transform leftSpawn;
    public Transform rightSpawn;

    [Header("Enemy Settings")]
    public float minDistance = 1.5f;

    void Start()
    {
        spawnInterval = initialSpawnInterval;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // t?ng ?? khó nhanh h?n
        if (timeElapsed >= difficultyStepTime)
        {
            spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - difficultyIncreaseRate);
            timeElapsed = 0f;
        }

        // spawn
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        int side = Random.Range(0, 4);
        Vector3 pos = Vector3.zero;

        switch (side)
        {
            case 0: pos = topSpawn.position; break;
            case 1: pos = bottomSpawn.position; break;
            case 2: pos = leftSpawn.position; break;
            case 3: pos = rightSpawn.position; break;
        }

        // tránh spawn ch?ng
        Collider2D hit = Physics2D.OverlapCircle(pos, minDistance, LayerMask.GetMask("Enemy"));
        if (hit == null)
        {
            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
        else
        {
            SpawnEnemy(); // th? l?i
        }
    }
}
