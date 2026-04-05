using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    NavGrid navGrid;
    List<GameObject> nodeList;

    bool spawnInitiated = false;
    

    int numWavesCompleted = 0;
    int requiredWaves;
    bool allWavesCompleted = false;
    int enemiesPerWave;

    [SerializeField] public List<GameObject> enemyTypes;

    List<GameObject> CurrentWave;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navGrid = GetComponentInParent<NavGrid>();
        nodeList = navGrid.nodes;
    }

    public void InitiateSpawn(int perWave, int waves)
    {
        spawnInitiated = true;
        requiredWaves = waves;
        enemiesPerWave = perWave;

        SpawnWave();
    }

    void SpawnWave()
    {
        for(int i = 0; i < enemiesPerWave; i++) {
            GameObject spawnNode = nodeList[UnityEngine.Random.Range(0, nodeList.Count)];
         
            Vector2 spawnPos = spawnNode.transform.position;
            
            GameObject enemy = Instantiate(enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)], spawnPos, quaternion.identity);
            enemy.GetComponent<EnemyBase>().navGrid = navGrid;
            CurrentWave.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CurrentWave.RemoveAll(enemy => enemy == null);

        if(spawnInitiated && !allWavesCompleted){
            if(CurrentWave.Count == 0)
            {
                numWavesCompleted += 1;

                if(numWavesCompleted < requiredWaves)
                {
                    SpawnWave();
                }
                else
                {
                    allWavesCompleted = true;
                }
            }
        }
    }
}
