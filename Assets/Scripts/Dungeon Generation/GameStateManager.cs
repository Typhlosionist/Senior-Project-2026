using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public struct SpawnerParameters
{
    public int minEnemiesPerWave;
    public int maxEnemiesPerWave;
    public int minWaves;
    public int maxWaves;
    public List<int> enemySpawnWeights;


}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public int currentLevel = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public SpawnerParameters CreateSpawnerParameters()
    {
        return new SpawnerParameters
        {
            minEnemiesPerWave = 2 + currentLevel,
            maxEnemiesPerWave = 4 + currentLevel,
            minWaves = 1 + currentLevel,
            maxWaves = 2 + currentLevel,
            enemySpawnWeights = new List<int> {4, 3, 2, 1},
        };
    }

    public void CompleteLevel()
    {
        currentLevel++;
    }
    
    public void ResetGame()
    {
        currentLevel = 0;
    }
}