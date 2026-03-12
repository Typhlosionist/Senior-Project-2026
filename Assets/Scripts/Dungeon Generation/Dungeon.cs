using NUnit.Framework.Constraints;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class Dungeon
{
    private int width;
    private int height;
    private int roomCount;
    private int minRoomSize;
    private int maxRoomSize;

    private char[,] dungeonLayout;

    private Vector3Int startLocation;
    private Vector3Int exitLocation;

    private const char WALL_CHAR = '#';
    private const char FLOOR_CHAR = '.';

    private const char START_CHAR = 'S';
    private const char END_CHAR = 'E';

    public Dungeon(int width, int height, int roomCount, int minRoomSize, int maxRoomSize)
    {
        this.width = width;
        this.height = height;
        this.roomCount = roomCount;
        this.minRoomSize = minRoomSize;
        this.maxRoomSize = maxRoomSize;

        dungeonLayout = new char[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                dungeonLayout[x, y] = WALL_CHAR;
            }
        }


    }

    public void AddRoom(Room room)
    {
        for (int x = room.x; x < room.x + room.width; x++)
                    {
                        for (int y = room.y; y < room.y + room.height; y++)
                        {
                            dungeonLayout[x, y] = FLOOR_CHAR;
                        }
                    }
    }

    public void AddFloor(int x, int y)
    {
        if (InBounds(x, y))
        {
            dungeonLayout[x, y] = FLOOR_CHAR;
        }
    }

    public bool IsFloor(int x, int y)
    {
        if (InBounds(x,y))
        {
            return dungeonLayout[x, y] == FLOOR_CHAR;
        }

        return false;
    }

    public bool IsWall(int x, int y)
    {
        if (InBounds(x,y))
        {
            return dungeonLayout[x, y] == WALL_CHAR;
        }

        return false;
    }

    public bool IsStart(int x, int y)
    {
        if (InBounds(x,y))
        {
            return dungeonLayout[x, y] == START_CHAR;
        }

        return false;
    }

    public bool IsExit(int x, int y)
    {
        if (InBounds(x,y))
        {
            return dungeonLayout[x, y] == END_CHAR;
        }

        return false;
    }

    public void ConnectRooms(Room room1, Room room2)
    {
        room1.ConnectRoom(room2);
        room2.ConnectRoom(room1);
    }

    public void SetStartLocation(int x, int y)
    {
        if (InBounds(x, y))
        {
            dungeonLayout[x, y] = START_CHAR;
            startLocation = new Vector3Int(x, y);
        }
    }

    public void SetExitLocation(int x, int y)
    {
        if (InBounds(x, y))
        {
            dungeonLayout[x, y] = END_CHAR;
            exitLocation = new Vector3Int(x, y);
        }
    }

    public char [,] GetLayout()
    {
        return dungeonLayout;
    }

    public Vector3Int GetStartLocation() => startLocation;

    public Vector3Int GetExitLocation() => exitLocation;
    public bool InBounds(int x, int y)
    {
        return 
            x >= 0 &&
            x < width &&
            y >= 0 &&
            y < height;
    }
}