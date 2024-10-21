using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Player data
    public Vector2 playerPosition;

    //Game state
    public List<string> unlockedObjs = new List<string>();
    public List<string> solvedPuzzles = new List<string>();
    public List<string> collectedItems = new List<string>();
    public List<ItemData> inventoryItems = new List<ItemData>();
    public string scene;
    public string test;

    //Dialogue progress
    public List<string> dialogue_mom = new List<string>();
    public List<string> dialogue_dad = new List<string>();
}
