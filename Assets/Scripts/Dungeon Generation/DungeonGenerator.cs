using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Settings")]

    public int width = 50;

    public int height = 50;

    public int minRoomWidth = 5;
    public int minRoomHeight = 5;

    public int minRooms = 5;
    public int maxRooms = 10;

    public int roomOffset = 3;

    public DungeonVisualizer dungeonVisualizer;

    private int targetRoomCount;

    public void GenerateDungeon()
    {

        dungeonVisualizer.Clear();

        targetRoomCount = Random.Range(minRooms, maxRooms);

        BoundsInt dungeonBounds = new BoundsInt(Vector3Int.zero, new Vector3Int(width, height, 0));

        List<BoundsInt> rooms = GenerateRooms(dungeonBounds);

        HashSet<Vector2Int> floorPositions = CreateRooms(rooms); 

        List<Vector2Int> roomPositions = new List<Vector2Int>();

        foreach (var room in rooms)
        {
            Vector3Int roomPosition = Vector3Int.RoundToInt(room.center);
            roomPositions.Add((Vector2Int)roomPosition);
        }

        HashSet<Vector2Int> hallways = ConnectRooms(roomPositions);
        floorPositions.UnionWith(hallways);

        dungeonVisualizer.PaintFloorTiles(floorPositions);


    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomPositions)
    {
        HashSet<Vector2Int> hallways = new HashSet<Vector2Int>();

        List<Vector2Int> connectedRooms = new List<Vector2Int>();
        List<Vector2Int> unconnectedRooms = new List<Vector2Int>(roomPositions);

        unconnectedRooms.Shuffle();

        connectedRooms.Add(unconnectedRooms[0]);
        unconnectedRooms.RemoveAt(0);

        while (unconnectedRooms.Count > 0)
        {
            Vector2Int currentRoom = unconnectedRooms[0];
            unconnectedRooms.RemoveAt(0);

            Vector2Int closestConnectedRoom = FindClosestRoom(currentRoom, connectedRooms);
            hallways.UnionWith(CreateHallway(closestConnectedRoom, currentRoom)); 

            int additionalConnections = Random.Range(0, 3);

            if (additionalConnections > 0 && connectedRooms.Count > 1)
            {
                for (int i = 0; i < additionalConnections; i++)
                {
                    Vector2Int randomRoom = connectedRooms[Random.Range(0, connectedRooms.Count)];

                    if (randomRoom != closestConnectedRoom)
                    {
                        hallways.UnionWith(CreateHallway(randomRoom, currentRoom));
                    }
                }
            }

            connectedRooms.Add(currentRoom);
        }

        return hallways;
    }

    private IEnumerable<Vector2Int> CreateHallway(Vector2Int closestConnectedRoom, Vector2Int destination)
    {
        HashSet<Vector2Int> hallway = new HashSet<Vector2Int>();

        Vector2Int position = closestConnectedRoom;

        hallway.Add(position);

        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            } 
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }

            hallway.Add(position);
        }

        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            } 
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }

            hallway.Add(position);
        }

        return hallway;
    }

    private Vector2Int FindClosestRoom(Vector2Int currentRoom, List<Vector2Int> connectedRooms)
    {
        Vector2Int closestRoom = connectedRooms[0];
        float closestDistance = float.MaxValue;

        foreach (var room in connectedRooms)
        {
            float distance = Vector2Int.Distance(currentRoom, room);

            if (distance < closestDistance)
            {
                closestRoom = room;
                closestDistance = distance;
            }
        }

        return closestRoom;
    }

    private List<BoundsInt> GenerateRooms(BoundsInt dungeonBounds)
    {
        List<BoundsInt> rooms = new List<BoundsInt>();

        Queue<BoundsInt> roomQueue = new Queue<BoundsInt>();

        roomQueue.Enqueue(dungeonBounds);

        while (roomQueue.Count > 0)
        {
            var room = roomQueue.Dequeue();
            if (room.size.x >= minRoomWidth * 2 || room.size.y >= minRoomHeight * 2)
            {
                if (Random.value < 0.5f)
                {
                    if (room.size.y >= minRoomHeight * 2)
                    {
                        SplitRoomHorizontal(roomQueue, room);
                    }
                    else
                    {
                        SplitRoomVertical(roomQueue, room);
                    }
                }
                else
                {
                    if (room.size.x >= minRoomWidth * 2)
                    {
                        SplitRoomVertical(roomQueue, room);
                    }
                    else
                    {
                        SplitRoomHorizontal(roomQueue, room);
                    
                    }
                }
            }
            else
            {
                if (rooms.Count < targetRoomCount)
                {
                    rooms.Add(room);
                }
                else
                {
                    break;
                }
            }
        }

        return rooms;

    }

    private void SplitRoomVertical(Queue<BoundsInt> roomQueue, BoundsInt room)
    {
        int xSplit = Random.Range(1, room.size.x / 2) + room.size.x / 4;

        BoundsInt bottomRoom = new BoundsInt(
            room.min,
            new Vector3Int(xSplit, room.size.y , room.size.z)
        );

        BoundsInt topRoom = new BoundsInt(
            new Vector3Int(xSplit, room.min.y, room.min.z),
            new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z)
        );

        roomQueue.Enqueue(bottomRoom);
        roomQueue.Enqueue(topRoom);
    }
    
    private void SplitRoomHorizontal(Queue<BoundsInt> roomQueue, BoundsInt room)
    {
        int ySplit = Random.Range(1, room.size.y / 2) + room.size.y / 4;

        BoundsInt bottomRoom = new BoundsInt(
            room.min,
            new Vector3Int(room.size.x, ySplit, room.size.z)
        );

        BoundsInt topRoom = new BoundsInt(
            new Vector3Int(room.min.x, ySplit, room.min.z),
            new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z)
        );

        roomQueue.Enqueue(bottomRoom);
        roomQueue.Enqueue(topRoom);
    }

    private HashSet<Vector2Int> CreateRooms(List<BoundsInt> rooms)
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        foreach (var room in rooms)
        {
            for (int x = roomOffset; x < room.size.x - roomOffset; x++)
            {
                for (int y = roomOffset; y < room.size.y - roomOffset; y++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(x, y);
                    floorPositions.Add(position);
                }
            }
        }

        return floorPositions;
    }
}


