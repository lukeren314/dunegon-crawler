using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class LevelGenerator
{
    private const int MAX_SEED = 2 << 16; // 2^32
    private System.Random rand;
    private int seed;
    public LevelGenerator(int seed = -1)
    {
        if (seed < 0)
        {
            System.Random tempRand = new System.Random();
            this.seed = tempRand.Next(MAX_SEED);
        }
        rand = new System.Random(this.seed);
    }
    public void GenerateLevel(int cols, int rows, int mainLength, float offshootTemperature, float offshootLengthTemperature)
    {
        LevelTemplate levelTemplate = GenerateLevelTemplate(cols, rows, mainLength, offshootTemperature, offshootLengthTemperature);
        CreateFromLevelTemplate(levelTemplate);
    }

    private LevelTemplate GenerateLevelTemplate(int cols, int rows, int mainLength, float offshootTemperature, float offshootLengthTemperature)
    {
        LevelTemplate levelTemplate = new LevelTemplate(cols, rows);

        levelTemplate.SetSpawn(new int[] { cols / 2, rows / 2 });
        SetFirstMain(levelTemplate);

        for (int i = 0; i < mainLength; i++)
        {
            ProceedMain(levelTemplate);
        }
        levelTemplate.SetExit();
        List<int[]> mainCoords = levelTemplate.GetMainCoords();
        foreach (int[] mainCoord in mainCoords)
        {
            List<int[]> surroundingAvailableCoords = GetSurroundingAvailableCoords(levelTemplate, mainCoord);
            if (surroundingAvailableCoords.Count > 0 && rand.NextDouble() < offshootTemperature)
            {
                int[] offshootCoord = surroundingAvailableCoords[rand.Next(surroundingAvailableCoords.Count)];
                levelTemplate.SetOffshoot(offshootCoord);
                RoomTemplate mainRoom = levelTemplate.GetMap(mainCoord);
                RoomTemplate offshootRoom = levelTemplate.GetMap(offshootCoord);
                ConnectRooms(mainRoom, offshootRoom);
                while (rand.NextDouble() < offshootLengthTemperature)
                {
                    List<int[]> availableNextOffshootCoords = GetSurroundingAvailableCoords(levelTemplate, offshootCoord);
                    if (availableNextOffshootCoords.Count == 0)
                    {
                        break;
                    }
                    int[] oldCoords = offshootCoord;
                    offshootCoord = availableNextOffshootCoords[rand.Next(availableNextOffshootCoords.Count)];
                    levelTemplate.SetOffshoot(offshootCoord);
                    RoomTemplate newOffShootRoom = levelTemplate.GetMap(offshootCoord);
                    ConnectRooms(levelTemplate.GetMap(oldCoords), newOffShootRoom);
                    List<RoomTemplate> potentialConnectRooms = GetPotentialConnectRooms(levelTemplate, offshootCoord, oldCoords);
                    if (potentialConnectRooms.Count < 2)
                    {
                        foreach (RoomTemplate potentialRoom in potentialConnectRooms)
                        {
                            ConnectRooms(newOffShootRoom, potentialRoom);
                        }
                    }
                    offshootLengthTemperature /= 2;
                }
            }
        }

        return levelTemplate;
    }

    private void SetFirstMain(LevelTemplate levelTemplate)
    {
        int[][] surroundingCoords = GetSurroundingCoords(levelTemplate.GetSpawn());
        int[] firstMainCoords = surroundingCoords[rand.Next(surroundingCoords.Length)];
        levelTemplate.SetMain(firstMainCoords);
    }

    private List<int[]> GetSurroundingAvailableCoords(LevelTemplate levelTemplate, int[] coords)
    {
        List<int[]> surroundingAvailableCoords = new List<int[]>();
        int[][] surroundingCoords = GetSurroundingCoords(coords);
        foreach (int[] surroundingCoord in surroundingCoords)
        {
            if (levelTemplate.IsAvailable(surroundingCoord))
            {
                surroundingAvailableCoords.Add(surroundingCoord);
            }
        }
        return surroundingAvailableCoords;
    }

    private List<int[]> GetSurroundingTypeCoords(LevelTemplate levelTemplate, int[] coords, RoomTemplate.RoomType roomType)
    {
        List<int[]> surroundingAvailableCoords = new List<int[]>();
        int[][] surroundingCoords = GetSurroundingCoords(coords);
        foreach (int[] surroundingCoord in surroundingCoords)
        {
            if (levelTemplate.IsInside(surroundingCoord) && levelTemplate.GetMap(surroundingCoord).roomType == roomType)
            {
                surroundingAvailableCoords.Add(surroundingCoord);
            }
        }
        return surroundingAvailableCoords;
    }

    private int[][] GetSurroundingCoords(int[] coords)
    {
        int[][] surroundingCoords = new int[4][];
        surroundingCoords[0] = new int[] { coords[0] - 1, coords[1] };
        surroundingCoords[1] = new int[] { coords[0] + 1, coords[1] };
        surroundingCoords[2] = new int[] { coords[0], coords[1] - 1 };
        surroundingCoords[3] = new int[] { coords[0], coords[1] + 1 };
        return surroundingCoords;
    }

    private void ProceedMain(LevelTemplate levelTemplate)
    {
        int[] lastHeadCoords = levelTemplate.GetMainHead();
        List<int[]> availableCoords = GetNextMainCoords(levelTemplate, lastHeadCoords);
        if (availableCoords.Count == 0)
        {
            return;
        }

        int[] nextMainCoords = availableCoords[rand.Next(availableCoords.Count)];
        RoomTemplate lastHead = levelTemplate.GetMap(lastHeadCoords);
        levelTemplate.SetMain(nextMainCoords);
        // connect the door from the previous
        ConnectRooms(lastHead, levelTemplate.GetMap(nextMainCoords));
    }

    private void CreateFromLevelTemplate(LevelTemplate levelTemplate)
    {
        Debug.Log("Seed: "+seed+"\n"+levelTemplate.ToString());
    }

    private List<int[]> GetNextMainCoords(LevelTemplate levelTemplate, int[] mainHeadCoords)
    {
        List<int[]> availableCoords = new List<int[]>();
        // the potential coordinate has to have at least 3 available squares next to it
        // or else it can get stuck in a loop
        int[][] surroundingCoords = GetSurroundingCoords(mainHeadCoords);
        foreach (int[] coords in surroundingCoords)
        {
            
            if (levelTemplate.IsAvailable(coords) && GetSurroundingTypeCoords(levelTemplate, coords, RoomTemplate.RoomType.MAIN).Count < 2)
            {
                availableCoords.Add(coords);
            }
        }
        return availableCoords;
    }

    private void ConnectRooms(RoomTemplate fromRoom, RoomTemplate toRoom)
    {
        int[] fromCoords = fromRoom.coords;
        int[] toCoords = toRoom.coords;
        if (fromCoords[0] < toCoords[0])
        {
            fromRoom.AddDoorDirection(RoomTemplate.DoorDirection.RIGHT);
            toRoom.AddDoorDirection(RoomTemplate.DoorDirection.LEFT);
        } else if (fromCoords[0] > toCoords[0])
        {
            fromRoom.AddDoorDirection(RoomTemplate.DoorDirection.LEFT);
            toRoom.AddDoorDirection(RoomTemplate.DoorDirection.RIGHT);
        } else if (fromCoords[1] < toCoords[1])
        {
            fromRoom.AddDoorDirection(RoomTemplate.DoorDirection.BOTTOM);
            toRoom.AddDoorDirection(RoomTemplate.DoorDirection.TOP);
        } else
        {
            fromRoom.AddDoorDirection(RoomTemplate.DoorDirection.TOP);
            toRoom.AddDoorDirection(RoomTemplate.DoorDirection.BOTTOM);
        }
    }

    private List<RoomTemplate> GetPotentialConnectRooms(LevelTemplate levelTemplate, int[] coords, int[] previousCoords)
    {
        List<RoomTemplate> potentialRooms = new List<RoomTemplate>();
        int[][] surroundingCoords = GetSurroundingCoords(coords);
        foreach (int[] surroundingCoord in surroundingCoords)
        {
            if (surroundingCoord != previousCoords && levelTemplate.IsInside(surroundingCoord)) 
            {
                RoomTemplate surroundingRoom = levelTemplate.GetMap(surroundingCoord);
                if (surroundingRoom.roomType == RoomTemplate.RoomType.MAIN || surroundingRoom.roomType == RoomTemplate.RoomType.OFFSHOOT)
                {

                    potentialRooms.Add(surroundingRoom);
                }
            }
        }
        return potentialRooms;
    }
}
