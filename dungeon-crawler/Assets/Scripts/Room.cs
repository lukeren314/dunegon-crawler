using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public TileObjects tileObjects;
    private List<GameObject> doors;
    private Vector2 spawnPoint;
    private List<Vector2> enemySpawnPoints;
    public void GenerateRoom(RoomTemplate roomTemplate)
    {
        doors = new List<GameObject>();
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
                doors.Add(tileObject);
                break;
            case RoomTemplate.TileType.SPAWN:
                tileObject = Instantiate(tileObjects.spawnTile, transform);
                tileObject.transform.localPosition = new Vector2(col, -row);
                spawnPoint = tileObject.transform.position;
                break;
            case RoomTemplate.TileType.EXIT:
                tileObject = Instantiate(tileObjects.exitTile, transform);
                break;
            default:
                return;
        }
        tileObject.transform.localPosition = new Vector2(col, -row);
    }
}
