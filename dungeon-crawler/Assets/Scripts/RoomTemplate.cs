using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomTemplate
{
    public enum RoomType
    {
        EMPTY,
        MAIN,
        OFFSHOOT,
        SPAWN,
        EXIT
    }

    public RoomType roomType;
    public enum DoorDirection
    {
        LEFT,
        RIGHT,
        TOP,
        BOTTOM
    }
    public int[] coords;

    private HashSet<DoorDirection> doorDirections;
    public RoomTemplate(int[] coords, RoomType roomType = RoomType.EMPTY)
    {
        this.coords = coords;
        this.roomType = roomType;
        doorDirections = new HashSet<DoorDirection>();
    }

    public void SetRoomType(RoomType roomType)
    {
        this.roomType = roomType;
    }

    public void AddDoorDirection(DoorDirection doorDirection)
    {
        doorDirections.Add(doorDirection);
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
}
