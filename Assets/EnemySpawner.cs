using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    private float budget;

    [System.Serializable]
    public class EnemyEntry {
        public GameObject enemyPrefab;
        public float cost {get; set;}
        public float costDecreaseRate;
        public float baseCost;
    }

    [SerializeField] private List<EnemyEntry> enemyEntries;



    // Start is called before the first frame update
    void Start()
    {
        foreach (var entry in enemyEntries)
        {
            entry.cost = entry.baseCost;
        }
    }

    // Update is called once per frame
    void Update()
    {
        budget += Mathf.Log(Time.time + 5.0f) * Time.deltaTime;

        foreach (var entry in enemyEntries)
        {
            entry.cost = Mathf.MoveTowards(entry.cost, 0.0f, Time.deltaTime);
        }       

        var grid = FindObjectOfType<GridManager>();
        // grid.pos

        var bestChoice = enemyEntries.OrderBy((e) => e.cost).First();
        if (bestChoice.cost < budget) {
            budget -= bestChoice.cost;
            var spawnPos = grid.gridPosToWorldPos(new Vector2Int(6, 2));
            Instantiate(bestChoice.enemyPrefab, spawnPos, Quaternion.identity);
            bestChoice.cost = bestChoice.baseCost;
        }
        
    }
}
