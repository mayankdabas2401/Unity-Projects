using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public bool infinite;

        public int enemyCount;
        public int hitsToKill;

        public float spawnRate;
        public float speed;
        public float enemyHealth;

        public Color skinColor;
    }

    public event System.Action<int> OnNewWave;

    public bool developmentMode;

    public Wave[] waves;
    public Enemy enemyPrefab;

    private int enemyRemaining;
    private int enemyAlive;
    private int waveIndex;

    private Wave currentWave;
    private MapGenerator map;
    private LivingEntity playerEntity;
    private Transform playerTransform; 

    private float nextSpawnTime;
    private bool isDisabled;

    private void Awake()
    {
        map = FindObjectOfType<MapGenerator>();
        playerEntity = FindObjectOfType<Player>();
        playerTransform = playerEntity.transform;
    }

    private void Start()
    {
        NextWave();
        playerEntity.OnDeath += OnPlayerDeath;
    }

    private void Update()
    {
        if(!isDisabled)
        {
            if ((enemyRemaining > 0 || currentWave.infinite) && Time.time > nextSpawnTime)
            {
                enemyRemaining -= 1;
                nextSpawnTime = Time.time + currentWave.spawnRate;

                StartCoroutine("SpawnRoutine");
            }
        }

        if(developmentMode)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                StopCoroutine("SpawnRoutine");
                foreach(Enemy enemy in FindObjectsOfType<Enemy>())
                {
                    Destroy(enemy.gameObject);
                }
                NextWave();
            }
        }
    }

    IEnumerator SpawnRoutine()
    {
        float spawnDelay = 1f;
        float flashSpeed = 4f;

        Transform randomTile = map.GetRandomOpenTiles();
        Material tileMaterial = randomTile.GetComponent<Renderer>().material;
        Color initialColor = Color.white;
        Color flashColor = Color.red;

        float spawnTimer = 0f;

        while(spawnTimer < spawnDelay)
        {
            tileMaterial.color = Color.Lerp(
                    initialColor,
                    flashColor,
                    Mathf.PingPong(spawnTimer * flashSpeed, 1)
                );

            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Enemy spawnedEnemy = Instantiate(
                    enemyPrefab,
                    randomTile.position + Vector3.up,
                    Quaternion.identity
                ) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;

        spawnedEnemy.SetCharacterstics(currentWave.hitsToKill, currentWave.enemyHealth, currentWave.speed, currentWave.skinColor);
    }

    void ResetPlayerPosition()
    {
        playerTransform.position = map.GetTilesFromPosition(Vector3.zero).position + Vector3.up * 3;
    }

    void OnEnemyDeath()
    {
        enemyAlive -= 1;

        if (enemyAlive <= 0) NextWave();
    }

    void OnPlayerDeath()
    {
        isDisabled = true;
    }

    private void NextWave()
    {
        waveIndex += 1;

        if(waveIndex - 1 < waves.Length)
        {
            currentWave = waves[waveIndex - 1];

            enemyRemaining = currentWave.enemyCount;
            enemyAlive = enemyRemaining;

            if (OnNewWave != null) OnNewWave(waveIndex);
            //ResetPlayerPosition();
        }
    }
}
