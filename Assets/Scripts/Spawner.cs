using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemyToSpawn;

    [SerializeField] int numberToSpawn;
    [SerializeField] int spawnRadius;

    [SerializeField] bool spawnOnStartup = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spawnOnStartup)
        {
        Spawn();
        }
    }

    void Spawn()
    {
        for(int i = 0; i < numberToSpawn; i++) {
            Vector2 center = new Vector2(transform.position.x,transform.position.y);
            Vector2 spawnPos = center + UnityEngine.Random.insideUnitCircle * spawnRadius;
            Instantiate(enemyToSpawn, spawnPos, quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
