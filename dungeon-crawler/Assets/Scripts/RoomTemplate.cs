using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomTemplate
{
    public const int MAX_ROOM_WIDTH = 25;
    public const int MAX_ROOM_HEIGHT = 25;
    public const int DOOR_WIDTH = 5;
    public const int ROOM_WIDTH = 21;
    public const int ROOM_HEIGHT = 17;

    public enum RoomType
    {
        EMPTY,
        MAIN,
        OFFSHOOT,
        SPAWN,
        EXIT
    }
    private RoomType roomType;

    public enum DoorDirection
    {
        LEFT,
        RIGHT,
        TOP,
        BOTTOM
    }
    private int[] coords;

    public enum TileType
    {
        EMPTY,
        FLOOR,
        WALL,
        DOOR,
        SPAWN,
        EXIT
    }
    private TileType[][] tiles;

    private HashSet<DoorDirection> doorDirections;
    private int width;
    private int height;
    public RoomTemplate(int[] coords, RoomType roomType = RoomType.EMPTY)
    {
        this.coords = coords;
        this.roomType = roomType;
        doorDirections = new HashSet<DoorDirection>();

        tiles = new TileType[MAX_ROOM_WIDTH][];
        for (int col = 0; col < MAX_ROOM_HEIGHT; col++)
        {
            tiles[col] = new TileType[MAX_ROOM_HEIGHT];
            for (int row = 0; row < MAX_ROOM_WIDTH; row++)
            {
                tiles[col][row] = TileType.EMPTY;
            }
        }

        width = ROOM_WIDTH;
        height = ROOM_HEIGHT;

        GenerateTiles();
    }

    public TileType GetTile(int col, int row)
    {
        return tiles[col][row];
    }

    public RoomType GetRoomType()
    {
        return roomType;
    }

    public int[] GetCoords()
    {
        return coords;
    }

    public void SetRoomType(RoomType roomType)
    {
        this.roomType = roomType;
    }

    public void AddDoorDirection(DoorDirection doorDirection)
    {
        doorDirections.Add(doorDirection);
        GenerateDoor(doorDirection);
    }

    public HashSet<DoorDirection> GetDoorDirections(DoorDirection doorDirection)
    {
        return doorDirections;
    }

    public bool ContainsDoorDirection(DoorDirection doorDirection)
    {
        return doorDirections.Contains(doorDirection);
    }

    public override string ToString()
    {
        switch (roomType) 
        {
            case RoomType.EMPTY:
                return "#";
            case RoomType.EXIT:
                return "e";
            case RoomType.MAIN:
                return "m";
            case RoomType.OFFSHOOT:
                return "o";
            case RoomType.SPAWN:
                return "s";
        }
        return "RoomType Null";
    }

    private void GenerateTiles()
    {
        switch(roomType)
        {
            case RoomType.EMPTY:
                return;
            case RoomType.EXIT:
                GenerateExitRoom();
                break;
            case RoomType.MAIN:
            case RoomType.OFFSHOOT:
                GenerateDefaultRoom();
                break;
            case RoomType.SPAWN:
                GenerateSpawnRoom();
                break;
        }
    }
   
    private void GenerateDefaultRoom()
    {
        int leftCol = (MAX_ROOM_WIDTH - width) / 2;
        int topRow = (MAX_ROOM_HEIGHT - height) / 2;
        // generate the floor
        GenerateRectangle(leftCol, topRow, width, height, TileType.FLOOR);
        // generate the walls
        GenerateVerticalTiles(leftCol, topRow, topRow+height, TileType.WALL);
        GenerateVerticalTiles(leftCol+width, topRow, topRow+height, TileType.WALL);
        GenerateHorizontalTiles(topRow, leftCol, leftCol+width, TileType.WALL);
        GenerateHorizontalTiles(topRow+height, leftCol, leftCol + width, TileType.WALL);
    }

    private void GenerateVerticalTiles(int col, int startRow, int endRow, TileType tileType)
    {
        for (int row = startRow; row < endRow; row++)
        {
            tiles[col][row] = tileType;
        }
    }

    private void GenerateHorizontalTiles(int row, int startCol, int endCol, TileType tileType)
    {
        for (int col = startCol; col < endCol; col++)
        {
            tiles[col][row] = tileType;
        }
    }

    private void GenerateRectangle(int startCol, int startRow, int width, int height, TileType tileType)
    {
        for (int col = startCol; col < startCol+width; col++)
        {
            for (int row = startRow; row < startRow+height; row++)
            {
                tiles[col][row] = tileType;
            }
        }
    }

    private void GenerateSpawnRoom()
    {
        GenerateDefaultRoom();
        tiles[MAX_ROOM_WIDTH / 2][MAX_ROOM_HEIGHT / 2] = TileType.SPAWN;
    }

    private void GenerateExitRoom()
    {
        GenerateDefaultRoom();
        tiles[MAX_ROOM_WIDTH / 2][MAX_ROOM_HEIGHT / 2] = TileType.EXIT;
    }

    private void GenerateDoor(DoorDirection doorDirection)
    {
        switch (doorDirection)
        {
            case DoorDirection.LEFT:
                GenerateLeftDoor();
                break;
            case DoorDirection.RIGHT:
                GenerateRightDoor();
                break;
            case DoorDirection.TOP:
                GenerateTopDoor();
                break;
            case DoorDirection.BOTTOM:
                GenerateBottomDoor();
                break;
        }
    }

    private void GenerateLeftDoor()
    {
        int startCol = 0;
        int startRow = MAX_ROOM_HEIGHT / 2 - DOOR_WIDTH / 2;
        int hallWidth = (MAX_ROOM_WIDTH - width) / 2;
        // generate floor for the hallways
        GenerateRectangle(startCol, startRow, hallWidth, DOOR_WIDTH, TileType.FLOOR);

        // generate the door on the side of the wall
        GenerateVerticalTiles(hallWidth, startRow, startRow+DOOR_WIDTH, TileType.DOOR);

        // generate the walls of the hallway
        GenerateHorizontalTiles(startRow, startCol, startCol + hallWidth, TileType.WALL);
        GenerateHorizontalTiles(startRow+DOOR_WIDTH, startCol, startCol + hallWidth, TileType.WALL);
    }

    private void GenerateRightDoor()
    {
        int startCol = (MAX_ROOM_WIDTH - width) / 2 + width;
        int startRow = MAX_ROOM_HEIGHT / 2 - DOOR_WIDTH / 2;
        int hallWidth = (MAX_ROOM_WIDTH - width) / 2;
        // generate floor for the hallways
        GenerateRectangle(startCol, startRow, hallWidth, DOOR_WIDTH, TileType.FLOOR);

        // generate the door on the side of the wall
        GenerateVerticalTiles(startCol, startRow, startRow + DOOR_WIDTH, TileType.DOOR);

        // generate the walls of the hallway
        GenerateHorizontalTiles(startRow, startCol, startCol + hallWidth, TileType.WALL);
        GenerateHorizontalTiles(startRow + DOOR_WIDTH, startCol, startCol + hallWidth, TileType.WALL);
    }

    private void GenerateTopDoor()
    {
        int startCol = MAX_ROOM_WIDTH / 2 - DOOR_WIDTH / 2;
        int startRow = 0;
        int hallWidth = (MAX_ROOM_HEIGHT - height) / 2;
        // generate floor for the hallways
        GenerateRectangle(startCol, startRow, DOOR_WIDTH, hallWidth, TileType.FLOOR);

        // generate the door on the side of the wall
        GenerateHorizontalTiles(hallWidth, startCol, startCol + DOOR_WIDTH, TileType.DOOR);

        // generate the walls of the hallway
        GenerateVerticalTiles(startCol, startRow, startRow + hallWidth, TileType.WALL);
        GenerateVerticalTiles(startCol + DOOR_WIDTH, startRow, startRow + hallWidth, TileType.WALL);
    }
    private void GenerateBottomDoor()
    {
        int startCol = MAX_ROOM_WIDTH / 2 - DOOR_WIDTH / 2;
        int startRow = (MAX_ROOM_HEIGHT - height) / 2 + height;
        int hallWidth = (MAX_ROOM_HEIGHT - height) / 2;
        // generate floor for the hallways
        GenerateRectangle(startCol, startRow, DOOR_WIDTH, hallWidth, TileType.FLOOR);

        // generate the door on the side of the wall
        GenerateHorizontalTiles(startRow, startCol, startCol + DOOR_WIDTH, TileType.DOOR);

        // generate the walls of the hallway
        GenerateVerticalTiles(startCol, startRow, startRow + hallWidth, TileType.WALL);
        GenerateVerticalTiles(startCol + DOOR_WIDTH, startRow, startRow + hallWidth, TileType.WALL);
    }
}
