using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Player data
    public Vector3 playerPosition;

    //Game state
    public List<string> unlockedObjs = new List<string>();
    public List<string> solvedPuzzles = new List<string>();
    public List<string> collectedItems = new List<string>();
    public List<string> inventoryItems = new List<string>();
    public string scene;
}
