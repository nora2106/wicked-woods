using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public string savePath;
    public GameData data;

    void Start()
    {
        savePath = Application.persistentDataPath + "/gamedata.json";
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, saveData);
    }

    public void GetSaveData()
    {
        if (File.Exists(savePath))
        {
            string loadData = File.ReadAllText(savePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(loadData);
            data = loadedData;
        }
    }

    public void DeleteSaveData()
    {
        File.Delete(savePath);
        GameData loadedData = new GameData();
        data = loadedData;

    }
}
