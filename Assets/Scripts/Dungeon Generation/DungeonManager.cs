using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public DungeonGenerator dungeonGenerator;

    public LevelExit levelExitPrefab;

    public Transform player;

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
        if (levelParent != null)
        {
            Destroy(levelParent.gameObject);
        }
        levelParent = new GameObject("Level Parent").transform;

        currentDungeon = dungeonGenerator.CreateDungeon();
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

}
