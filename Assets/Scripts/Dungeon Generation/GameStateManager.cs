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
            minEnemiesPerWave = 3 + currentLevel,
            maxEnemiesPerWave = 5 + currentLevel,
            minWaves = 2 + currentLevel,
            maxWaves = 2 + currentLevel,
            enemySpawnWeights = new List<int> {1, 2, 2, 2},
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