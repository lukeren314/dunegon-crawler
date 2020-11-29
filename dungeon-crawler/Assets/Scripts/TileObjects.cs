using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileObjects", menuName = "TileObjects", order = 1)]
public class TileObjects : ScriptableObject
{
    public GameObject floorTile;
    public GameObject wallTile;
    public GameObject doorTile;
    public GameObject spawnTile;
    public GameObject exitTile;
    public GameObject triggerTile;
}
