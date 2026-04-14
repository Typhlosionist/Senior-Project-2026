using UnityEngine;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
{
    public DungeonGenerator dungeonGenerator;

    public LevelExit levelExitPrefab;

    public NavGrid navGridPrefab;

    public Transform player;

    public List<GameObject> enemyTypes;

    //public ExitRoomWarning exitRoomWarningPrefab;

    public int exitRoomWarningBuffer = 2;

    private Dungeon currentDungeon;

    private Vector3 spawnOffset = new Vector3(0.5f, 0.5f);

    private Transform levelParent;

    private void Start()
    {
        player.transform.position = NextLevel();
    }

    public Vector3 NextLevel()
    {
        GameStateManager.Instance.CompleteLevel();
        SpawnerParameters spawnerParameters = GameStateManager.Instance.CreateSpawnerParameters();
        if (levelParent != null)
        {
            Destroy(levelParent.gameObject);
        }
        levelParent = new GameObject("Level Parent").transform;

        currentDungeon = dungeonGenerator.CreateDungeon();

        List<Room> rooms = currentDungeon.GetRooms();
        SetNavGrids(rooms, spawnerParameters);


        LevelExit exit = Instantiate(levelExitPrefab, currentDungeon.GetExitLocation() + spawnOffset, Quaternion.identity);

        exit.dungeonManager = this;
        exit.transform.parent = levelParent;

        // Room exitRoom = currentDungeon.GetExitRoom();

        // var (exitX, exitY) = exitRoom.Center();
        // Vector3 warningCenter = new Vector3(exitX, exitY) + spawnOffset;

        // ExitRoomWarning warning = Instantiate(exitRoomWarningPrefab, warningCenter, Quaternion.identity);
        // warning.GetComponent<BoxCollider2D>().size = new Vector2(
        //     exitRoom.width + exitRoomWarningBuffer * 2,
        //     exitRoom.height + exitRoomWarningBuffer * 2
        // );

        // warning.transform.parent = levelParent;

        return currentDungeon.GetStartLocation() + spawnOffset;
    }

    public void SetNavGrids(List<Room> rooms, SpawnerParameters spawnerParameters)
    {
        for (int i = 1; i < rooms.Count; i++)
        {
            Room room = rooms[i];
            Vector3 spawnPosition = new Vector3(room.x + 0.5f, room.y + 0.5f, 0f);

            NavGrid navGrid = Instantiate(navGridPrefab, spawnPosition, Quaternion.identity);
            navGrid.transform.SetParent(levelParent, true);

            navGrid.nodesVertical = room.height;
            navGrid.nodesHorizontal = room.width;

            Spawner spawner = navGrid.gameObject.AddComponent<Spawner>();
            spawner.enemyTypes = enemyTypes;
            navGrid.spawnWeights = spawnerParameters.enemySpawnWeights;

            navGrid.enemiesPerWave = Random.Range(spawnerParameters.minEnemiesPerWave, spawnerParameters.maxEnemiesPerWave + 1);
            navGrid.waveCount = Random.Range(spawnerParameters.minWaves, spawnerParameters.maxWaves + 1);

            navGrid.Initialize();
        }

        
    }
}
