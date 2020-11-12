using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplate
{
    private RoomTemplate[][] map;
    private int cols;
    private int rows;
    private int[] spawnCoords;
    private List<int[]> mainCoords;
    private List<int[]> offShootCoords;
    private int[] exitCoords;

    public LevelTemplate(int cols, int rows)
    {
        this.cols = cols;
        this.rows = rows;
        ResetMap();
    }
    public RoomTemplate GetMap(int[] coords)
    {
        return map[coords[0]][coords[1]];
    }

    public void SetMap(int[] coords, RoomTemplate roomTemplate)
    {
        map[coords[0]][coords[1]] = roomTemplate;
    }

    public int[] GetSpawn()
    {
        return spawnCoords;
    }

    public void SetSpawn(int[] coords)
    {
        SetMap(coords, new RoomTemplate(coords, RoomTemplate.RoomType.SPAWN));
        spawnCoords = coords;
    }

    public int[] GetMainHead()
    {
        return mainCoords[mainCoords.Count - 1];
    }

    public List<int[]> GetMainCoords()
    {
        return mainCoords;
    }

    public void SetMain(int[] coords)
    {
        SetMap(coords, new RoomTemplate(coords, RoomTemplate.RoomType.MAIN));
        mainCoords.Add(coords);
    }

    public void SetOffshoot(int[] coords)
    {
        SetMap(coords, new RoomTemplate(coords, RoomTemplate.RoomType.OFFSHOOT));
        offShootCoords.Add(coords);
    }

    public void SetExit()
    {
        int[] exitCoords = mainCoords[mainCoords.Count - 1];
        GetMap(exitCoords).SetRoomType(RoomTemplate.RoomType.EXIT);
    }

    public override string ToString()
    {
        string str = "LevelType Cols: "+cols+" Rows: "+rows+"\n";
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int[] coords = new int[] { col, row };
                str += GetMap(coords).ToString() + "  ";
            }
            str += "\n";
        }
        return str;
    }
    public bool IsInside(int[] coords)
    {
        return coords[0] > -1 && coords[0] < cols && coords[1] > -1 && coords[1] < rows;
    }
    public bool IsAvailable(int[] coords)
    {
        return IsInside(coords) && IsAvailableRoomType(coords);
    }

    public bool IsAvailableRoomType(int[] coords)
    {
        RoomTemplate.RoomType roomType = GetMap(coords).roomType;
        switch (roomType)
        {
            case RoomTemplate.RoomType.EMPTY:
                return true;
        }
        return false;
    }

    private void ResetMap()
    {
        map = new RoomTemplate[cols][];
        for (int col = 0; col < cols; col++)
        {
            map[col] = new RoomTemplate[cols];
            for (int row = 0; row < rows; row++)
            {
                int[] coords = new int[] { col, row };
                SetMap(coords, new RoomTemplate(coords));
            }
        }
        mainCoords = new List<int[]>();
        offShootCoords = new List<int[]>();
    }

}
