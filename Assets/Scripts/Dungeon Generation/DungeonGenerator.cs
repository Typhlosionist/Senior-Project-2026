using System;
using System.Collections.Generic;
using System.IO.Hashing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{

    public int dungeonWidth = 90;
    public int dungeonHeight = 80;

    public int roomCount = 5;
    public int minRoomSize = 15;
    public int maxRoomSize = 20;

    public DungeonVisualizer dungeonVisualizer;
    public void GenerateDungeon()
    {
        Dungeon dungeon = new Dungeon(dungeonWidth, dungeonHeight, roomCount, minRoomSize, maxRoomSize);
        List<Room> rooms = GenerateRooms(dungeon);

        ConnectRooms(dungeon, rooms);
        SetStartAndExitRooms(dungeon, rooms);

        dungeonVisualizer.Clear();
        dungeonVisualizer.PaintFloorTiles(dungeon);

    }

    public Dungeon CreateDungeon()
    {
        Dungeon dungeon = new Dungeon(dungeonWidth, dungeonHeight, roomCount, minRoomSize, maxRoomSize);
        List<Room> rooms = GenerateRooms(dungeon);
        ConnectRooms(dungeon, rooms);
        SetStartAndExitRooms(dungeon, rooms);

        

        dungeonVisualizer.Clear();
        dungeonVisualizer.PaintFloorTiles(dungeon);

        return dungeon;
    }


    private List<Room> GenerateRooms(Dungeon dungeon)
    {
        List<Room> rooms = new List<Room>();
        List<Room> intersectionRooms = new List<Room>();
        System.Random random = new System.Random();


        Room startRoom = PlaceRoomInZone(dungeon, intersectionRooms, random, 2, dungeonWidth / 3, 2, dungeonHeight / 3);
        dungeon.AddRoom(startRoom);
        rooms.Add(startRoom);
        intersectionRooms.Add(startRoom);
        

        Room exitRoom = PlaceRoomInZone(dungeon, intersectionRooms, random, dungeonWidth / 3 * 2, dungeonWidth - 2, dungeonHeight / 3 * 2, dungeonHeight - 2);
        intersectionRooms.Add(exitRoom);

        for (int i = 0; i < roomCount - 2; i++)
        {
            for (int attempt = 0; attempt < 50; attempt++)
            {
                int roomWidth = random.Next(minRoomSize, maxRoomSize);
                int roomHeight = random.Next(minRoomSize, maxRoomSize);
                int roomX = random.Next(2, dungeonWidth - roomWidth - 2);
                int roomY = random.Next(2, dungeonHeight - roomHeight - 2);

                Room newRoom = new Room(roomX, roomY, roomWidth, roomHeight);

                bool intersects = false;

                foreach (var room in intersectionRooms)
                {
                    if (newRoom.Intersects(room))
                    {
                        intersects = true;
                        break;
                    }
                }

                if (!intersects)
                {
                    rooms.Add(newRoom);
                    intersectionRooms.Add(newRoom);
                    dungeon.AddRoom(newRoom);
                    break;
                }

            }
        }

        rooms.Add(exitRoom);
        dungeon.AddRoom(exitRoom);

        return rooms;
    }

    private Room PlaceRoomInZone(Dungeon dungeon, List<Room> existingRooms, System.Random random,
    int xMin, int xMax, int yMin, int yMax)
{
    int roomWidth = random.Next(minRoomSize, maxRoomSize);
    int roomHeight = random.Next(minRoomSize, maxRoomSize);

    for (int attempt = 0; attempt < 50; attempt++)
    {
        
        int roomX = random.Next(xMin, xMax - roomWidth);
        int roomY = random.Next(yMin, yMax - roomHeight);

        Room candidate = new Room(roomX, roomY, roomWidth, roomHeight);

        bool intersects = false;
        foreach (var room in existingRooms)
        {
            if (candidate.Intersects(room))
            {
                intersects = true;
                break;
            }
        }

        if (!intersects)
            return candidate;
    }

    // Fallback: place at the zone's minimum corner if all attempts fail
    return new Room(xMin, yMin, roomWidth, roomHeight);
}

    private void ConnectRooms(Dungeon dungeon, List<Room> rooms)
    {
        System.Random random = new System.Random();
        for (int i = 1; i < rooms.Count; i++)
        {
            var (x1, y1) = rooms[i-1].Center();
            var (x2, y2) = rooms[i].Center();

            if(random.Next(0, 2) == 0)
            {
                CreateHorizontalHallway(dungeon, x1, x2, y1);
                CreateVerticalHallway(dungeon, y1, y2, x2);
            }
            else
            {
                CreateVerticalHallway(dungeon, y1, y2, x2);
                CreateHorizontalHallway(dungeon, x1, x2, y1);
                
            }

            dungeon.ConnectRooms(rooms[i-1], rooms[i]);
        }
    }

    private void CreateVerticalHallway(Dungeon dungeon, int y1, int y2, int x)
    {
        int start = Math.Min(y1, y2);
        int end = Math.Max(y1, y2);

        for (int y = start; y <= end; y++)
        {
            dungeon.AddFloor(x, y);
            dungeon.AddFloor(x + 1, y);
            dungeon.AddFloor(x - 1, y);
        }
    }
    

    private void CreateHorizontalHallway(Dungeon dungeon, int x1, int x2, int y)
    {
        int start = Math.Min(x1, x2);
        int end = Math.Max(x1, x2);

        for (int x = start; x <= end; x++)
        {
            dungeon.AddFloor(x, y);
            dungeon.AddFloor(x, y + 1);
            dungeon.AddFloor(x, y - 1);
        }
    }

    private void SetStartAndExitRooms(Dungeon dungeon, List<Room> rooms)
    {
        
        System.Random random = new System.Random();

        //Room startRoom = rooms[random.Next(0, rooms.Count)];
        //Room exitRoom = SelectExitRoom(startRoom, rooms);

        Room startRoom = rooms[0];
        Room exitRoom = rooms[rooms.Count - 1];

        dungeon.SetStartLocation(startRoom.Center().x, startRoom.Center().y);
        dungeon.SetExitRoom(exitRoom);
        SetExitDoor(dungeon, exitRoom);


    }

    private Room SelectExitRoom(Room startRoom, List<Room> rooms)
    {
        System.Random random = new System.Random();
        List<Room> possibleRooms = new List<Room>();

        foreach (var room in rooms)
        {
            if (room != startRoom)
            {
                List<Room> connectedRooms = room.GetConnectedRooms();

                if (!connectedRooms.Contains(startRoom))
                {
                    possibleRooms.Add(room);
                }
            }
        }

        if (possibleRooms.Count == 0)
        {
            // if dungeon only contains one room
            possibleRooms = new List<Room>(rooms);
            possibleRooms.Remove(startRoom);

            if (possibleRooms.Count == 0)
            {
                possibleRooms.Add(startRoom);
            }
        }

        return possibleRooms[random.Next(0, possibleRooms.Count)];
        
    }

    private void SetExitDoor(Dungeon dungeon, Room room)
    {
        List<(int x, int y)> wallPositions = new List<(int x, int y)>();

        for (int x = room.x - 1; x <= room.x + room.width; x++)
        {
            for (int y = room.y - 1; y <= room.y + room.height; y++)
            {
                if (dungeon.IsWall(x, y) && IsCenterWall(dungeon, x, y))
                {
                    wallPositions.Add((x, y));
                }
            }
        }

        if (wallPositions.Count > 0)
        {
            System.Random random = new System.Random();
            var (x, y) = wallPositions[random.Next(0, wallPositions.Count)];
            dungeon.SetExitLocation(x, y);
        }
    }

    

    private bool IsCenterWall(Dungeon dungeon, int x, int y)
    {
        
        string binaryTileType = "";
        foreach (var direction in dungeonVisualizer.wallGenerator.directions)
        {
            int neighborX = x + direction.x;
            int neighborY = y + direction.y;

            if (dungeon.InBounds(neighborX, neighborY))
            {
                if (dungeon.IsFloor(neighborX, neighborY))
                {
                    binaryTileType += "1";
                }
                else
                {
                    binaryTileType += "0";
                }
            }
        }

        int tileType = Convert.ToInt32(binaryTileType, 2);

        return (
            WallTypesHelper.topWall.Contains(tileType) ||
            WallTypesHelper.rightWall.Contains(tileType) ||
            WallTypesHelper.bottomWall.Contains(tileType) ||
            WallTypesHelper.leftWall.Contains(tileType)
        );
    }
}


