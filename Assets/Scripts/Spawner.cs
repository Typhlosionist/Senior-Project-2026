using System.Collections.Generic;
using System.Collections;
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
    bool waitingForWave = false;
    int enemiesPerWave;
    List<int> spawnWeights;

    [SerializeField] public List<GameObject> enemyTypes;
    [SerializeField] public float spawnDelaySeconds = 2.0f;
    [SerializeField] public float waveDelaySeconds = 3.0f;

    List<GameObject> CurrentWave = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navGrid = GetComponentInParent<NavGrid>();
        nodeList = navGrid.nodes;
    }

    public void InitiateSpawn(int perWave, int waves, List<int> enemySpawnWeights)
    {
        spawnInitiated = true;
        waitingForWave = true;
        requiredWaves = waves;
        enemiesPerWave = perWave;
        spawnWeights = enemySpawnWeights;

        StartCoroutine(SpawnAfterDelay(spawnDelaySeconds));
    }

    IEnumerator SpawnAfterDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        waitingForWave = false;
        SpawnWave(spawnWeights);
    }

    void SpawnWave(List<int> enemySpawnWeights)
    {
        CurrentWave = new List<GameObject>();

        for(int i = 0; i < enemiesPerWave; i++) {
            GameObject spawnNode = nodeList[UnityEngine.Random.Range(0, nodeList.Count)];
         
            Vector2 spawnPos = spawnNode.transform.position;
            
            //GameObject enemy = Instantiate(enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)], spawnPos, quaternion.identity);
            GameObject enemy = Instantiate(SelectEnemy(enemySpawnWeights), spawnPos, quaternion.identity);
            enemy.transform.SetParent(navGrid.transform, true);
            enemy.GetComponent<EnemyBase>().navGrid = navGrid;
            CurrentWave.Add(enemy);
        }
    }

    public bool AllWavesCompleted()
    {
        return allWavesCompleted;
    }

    GameObject SelectEnemy(List<int> weights)
    {
        int totalWeight = 0;
        foreach (int weight in weights) totalWeight += weight;
        int rand = UnityEngine.Random.Range(0, totalWeight);
        int cumulative = 0;

        for (int i = 0; i < weights.Count; i++)
        {
            cumulative += weights[i];
            if (rand < cumulative)
            {
                return enemyTypes[i];
            }
        }

        return enemyTypes[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentWave != null)
        {
            CurrentWave.RemoveAll(enemy => enemy == null);
        }

        if(spawnInitiated && !allWavesCompleted && !waitingForWave){
            if(CurrentWave.Count == 0)
            {
                numWavesCompleted++;

                if (numWavesCompleted < requiredWaves)
                {
                    waitingForWave = true;
                    StartCoroutine(SpawnAfterDelay(waveDelaySeconds));
                }
                else
                {
                    allWavesCompleted = true;
                }
            }
        }
    }
}
