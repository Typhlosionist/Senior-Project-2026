using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemyToSpawn;

    [SerializeField] int numberToSpawn;

    [SerializeField] bool spawnOnStartup = false;

    NavGrid navGrid;
    List<GameObject> nodeList;

    bool spawnInitiated = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navGrid = GetComponentInParent<NavGrid>();
        nodeList = navGrid.nodes;

        if (spawnOnStartup)
        {
            spawnInitiated = true;
        }
    }

    public void Spawn()
    {
        if(nodeList.Count < 1)
        {
            nodeList = navGrid.nodes;
        }
        else{

            for(int i = 0; i < numberToSpawn; i++) {
                GameObject spawnNode = nodeList[UnityEngine.Random.Range(0, nodeList.Count)];
                
                Vector2 spawnPos = spawnNode.transform.position;
                
                Instantiate(enemyToSpawn, spawnPos, quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnInitiated){
            if(nodeList.Count < 1)
            {
                nodeList = navGrid.nodes;
            }
            else
            {
                Spawn();
                spawnInitiated = false;
            }
        }
    }
}
