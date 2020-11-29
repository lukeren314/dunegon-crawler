using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public TileObjects tileObjects;
    private List<DoorTile> doors;
    private Vector2 spawnPoint;
    private List<Vector2> enemySpawnPoints;
    private bool doorsActivated;
    private bool roomCompleted;
    private RoomTemplate.RoomType roomType;
    public void GenerateRoom(RoomTemplate roomTemplate)
    {
        roomType = roomTemplate.GetRoomType();
        doors = new List<DoorTile>();
        for (int col = 0; col < RoomTemplate.MAX_ROOM_WIDTH; col++)
        {
            for (int row = 0; row < RoomTemplate.MAX_ROOM_HEIGHT; row++)
            {
                GenerateTile(col, row, roomTemplate.GetTile(col, row));
            }
        }

    }

    public Vector2 GetSpawnPoint()
    {
        return spawnPoint;
    }

    public void ActivateDoors()
    {
        if (!doorsActivated && !roomCompleted && roomType != RoomTemplate.RoomType.SPAWN && roomType != RoomTemplate.RoomType.EXIT)
        {
            doorsActivated = true;
            foreach (DoorTile door in doors)
            {
                door.Activate();
            }
        }
    }

    public void CompleteRoom()
    {
        if (!roomCompleted)
        {
            roomCompleted = true;
            foreach (DoorTile door in doors)
            {
                door.Deactivate();
            }
        }
    }


    private void GenerateTile(int col, int row, RoomTemplate.TileType tileType)
    {
        GameObject tileObject;
        switch (tileType)
        {
            case RoomTemplate.TileType.EMPTY:
                return;
            case RoomTemplate.TileType.FLOOR:
                tileObject = Instantiate(tileObjects.floorTile, transform);
                break;
            case RoomTemplate.TileType.WALL:
                tileObject = Instantiate(tileObjects.wallTile, transform);
                break;
            case RoomTemplate.TileType.DOOR:
                tileObject = Instantiate(tileObjects.doorTile, transform);
                DoorTile door = tileObject.GetComponent<DoorTile>();
                door.Deactivate();
                doors.Add(door);
                break;
            case RoomTemplate.TileType.SPAWN:
                tileObject = Instantiate(tileObjects.spawnTile, transform);
                tileObject.transform.localPosition = new Vector2(col, -row);
                spawnPoint = tileObject.transform.position;
                break;
            case RoomTemplate.TileType.EXIT:
                tileObject = Instantiate(tileObjects.exitTile, transform);
                break;
            case RoomTemplate.TileType.TRIGGER:
                tileObject = Instantiate(tileObjects.triggerTile, transform);
                tileObject.GetComponent<TriggerTile>().SetRoom(this);
                break;
            default:
                return;
        }
        tileObject.transform.localPosition = new Vector2(col, -row);
    }
}
