using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    string savePath;
    public GameData data;
    // Start is called before the first frame update
    void Awake()
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
            data = JsonUtility.FromJson<GameData>(loadData);
        }
    }

    public void DeleteSaveData()
    {
        File.Delete(savePath);
    }
}
