using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTile : MonoBehaviour
{
    private Room room;
    public Room GetRoom()
    {
        return room;
    }
    public void SetRoom(Room room)
    {
        this.room = room;
    }
}
