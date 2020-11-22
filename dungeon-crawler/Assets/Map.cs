using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject roomPrefab;
    private Vector2 spawnPoint;

    public Vector2 GetSpawnPoint()
    {
        return spawnPoint;
    }

    private void Awake()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        LevelGenerator levelGenerator = new LevelGenerator();
        LevelTemplate levelTemplate = levelGenerator.GenerateLevelTemplate();
        CreateLevelFromTemplate(levelTemplate, transform);
    }

    private void CreateLevelFromTemplate(LevelTemplate levelTemplate, Transform mapTransform)
    {
        for (int col = 0; col < levelTemplate.GetCols(); ++col)
        {
            for (int row = 0; row < levelTemplate.GetRows(); ++row)
            {
                RoomTemplate roomTemplate = levelTemplate.GetMap(new int[] { col, row });
                if (roomTemplate.GetRoomType() != RoomTemplate.RoomType.EMPTY)
                {
                    Room room = GenerateRoom(roomTemplate, col, row, mapTransform);
                    if (roomTemplate.GetRoomType() == RoomTemplate.RoomType.SPAWN)
                    {
                        spawnPoint = room.GetSpawnPoint();
                    }
                }
            }
        }
    }

    private Room GenerateRoom(RoomTemplate roomTemplate, int roomCol, int roomRow, Transform mapTransform)
    {
        GameObject roomObject = Instantiate(roomPrefab, mapTransform);
        roomObject.transform.localPosition = new Vector2(roomCol*RoomTemplate.MAX_ROOM_WIDTH, -roomRow*RoomTemplate.MAX_ROOM_HEIGHT);
        Room room = roomObject.GetComponent<Room>();
        room.GenerateRoom(roomTemplate);
        return room;
    }

}
