using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RoomLock : MonoBehaviour
{
    public GameObject barrierPrefab;

    private Room room;
    private Dungeon dungeon;
    private Spawner spawner;
    private List<GameObject> barriers = new List<GameObject>();
    private bool locked = false;
    private bool cleared = false;
    public float lockDelaySeconds = 0.75f;

    public void Initialize(Room room, Dungeon dungeon, GameObject barrierPrefab)
    {
        this.room = room;
        this.dungeon = dungeon;
        this.barrierPrefab = barrierPrefab;
        this.spawner = GetComponent<Spawner>();
    }

    public void LockRoom()
    {
        if (locked) return;
        locked = true;
        StartCoroutine(LockRoomAfterDelay());
    }

    IEnumerator LockRoomAfterDelay()
    {
        yield return new WaitForSeconds(lockDelaySeconds);
        
        List<Vector2> doorways = FindDoorways();
        foreach (Vector2 pos in doorways)
        {
            GameObject barrier = Instantiate(barrierPrefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
            barrier.transform.SetParent(transform);
            barriers.Add(barrier);
        }
    }

    public void UnlockRoom()
    {
        foreach (GameObject barrier in barriers)
        {
            if (barrier != null)
            {
                Destroy(barrier);
            }
        }
        barriers.Clear();
        cleared = true;
    }

    private List<Vector2> FindDoorways()
    {
        List<Vector2> doorways = new List<Vector2>();

        for (int y = room.y; y < room.y + room.height; y++)
        {
            if (dungeon.IsFloor(room.x - 1, y))
            {
                doorways.Add(new Vector2(room.x - 0.5f, y + 0.5f));
            }
            if (dungeon.IsFloor(room.x + room.width, y))
            {
                doorways.Add(new Vector2(room.x + room.width + 0.5f, y + 0.5f));
            }
        }

        for (int x = room.x; x < room.x + room.width; x++)
        {
        
            if (dungeon.IsFloor(x, room.y - 1))
            {
                doorways.Add(new Vector2(x + 0.5f, room.y - 0.5f));
            }
            if (dungeon.IsFloor(x, room.y + room.height))
            {
                doorways.Add(new Vector2(x + 0.5f, room.y + room.height + 0.5f));
            }
        }
        Debug.Log("Doorways: " + doorways.Count);
        return doorways;
    }

    void Update()
    {
        if (locked && !cleared && spawner != null && spawner.AllWavesCompleted())
        {
            UnlockRoom();
        }
    }
}