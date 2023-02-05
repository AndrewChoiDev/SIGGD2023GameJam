using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{

    [System.Serializable]
    public class EnemyEntry {
        public GameObject enemyPrefab;
        public float baseSpawnPeriod;
        public float startSpawnTime;
        public float timeOfMaxSpawnRate;
        public float finalSpawnPeriod;
        public float lastSpawnTime {get; set;}

        public float currentPeriod() {
            var t = (Time.timeSinceLevelLoad - startSpawnTime) / (timeOfMaxSpawnRate - startSpawnTime);
            return Mathf.Lerp(baseSpawnPeriod, finalSpawnPeriod, t);
        }
    }

    [SerializeField] private List<EnemyEntry> enemyEntries;



    // Start is called before the first frame update
    void Start()
    {
    }

    float timeOfLastSpawn = -999.0f;

    private int lastLaneSpawn = -1;

    // Update is called once per frame
    void Update()
    {
        var grid = FindObjectOfType<GridManager>();
        if (Time.timeSinceLevelLoad > timeOfLastSpawn + 0.4f) {
            foreach (var enemy in enemyEntries) {
                if (Time.timeSinceLevelLoad > enemy.startSpawnTime 
                    && Time.timeSinceLevelLoad > enemy.lastSpawnTime + enemy.currentPeriod()) {
                    enemy.lastSpawnTime = Time.timeSinceLevelLoad;
                    timeOfLastSpawn = Time.timeSinceLevelLoad;

                    var laneSpawn = Random.Range(0, 4);
                    if (lastLaneSpawn == laneSpawn) {
                        laneSpawn = (laneSpawn + 1) % 4;
                    }

                    var spawnPos = grid.gridPosToWorldPos(new Vector2Int(10, laneSpawn));
                    Instantiate(enemy.enemyPrefab, spawnPos, Quaternion.identity);
                    lastLaneSpawn = laneSpawn;
                }
            }
                
        }

    }
}
