using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Reflection;

public class GameManager : MonoBehaviour
{
    public GameObject overlay;
    public GameObject player;
    private GameObject[] objs;
    private Camera[] cameras;
    public static GameManager Instance;
    private string currentScene;
    public SaveManager save;
    public SpawnPos spawnPos;
    public MonologueSystem textSystem;
    public GameObject dialoguePanel;
    public string selectedItemID;
    void Start()
    {
        textSystem = FindObjectOfType<MonologueSystem>();
        currentScene = SceneManager.GetActiveScene().name;
        Instance = this;
        save = gameObject.GetComponent<SaveManager>();
        dialoguePanel = GameObject.FindWithTag("DialogueCanvas");
        dialoguePanel.SetActive(false);
        SceneManager.activeSceneChanged += OnSceneChange;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SetupAll();
    }

    public void Setup()
    {
        spawnPos = FindObjectOfType<SpawnPos>();
        overlay = GameObject.FindWithTag("Overlay");
        overlay.GetComponent<Canvas>().worldCamera = Camera.main;
        overlay.SetActive(false);
        overlay.GetComponent<CanvasGroup>().alpha = 0;
        objs = FindGameObjectsInLayer(6);
        if(objs != null)
        {
            foreach (GameObject obj in objs)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 10);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupAll();
        player.GetComponent<MoveCharacter>().setPosition(spawnPos.getPosition(currentScene));
        currentScene = scene.name;
    }

    private void OnSceneChange(Scene lastScene, Scene nextScene)
    {
        save.data.scene = nextScene.name;
        save.Save();
    }

    private void SetupAll()
    {
        save.savePath = Application.persistentDataPath + "/gamedata.json";
        save.GetSaveData();
        var setupObjects = Resources.FindObjectsOfTypeAll<MonoBehaviour>().OfType<ISetup>();
        foreach (ISetup obj in setupObjects)
        {
            GameObject objGameObject = ((MonoBehaviour)obj).gameObject;

            // Only call Setup on objects that are part of an active scene
            if (objGameObject.scene.IsValid() && objGameObject.scene.isLoaded)
            {
                obj.Setup();
            }
        }
        Setup();
    }

    GameObject[] FindGameObjectsInLayer(int layer)
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new System.Collections.Generic.List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layer)
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList.ToArray();
    }

    public void OpenOverlay() {
        overlay.SetActive(true);
        overlay.GetComponent<CanvasGroup>().alpha = 1;
        player.GetComponent<MoveCharacter>().canMove = false;
        player.layer = 2;
    }

    public void CloseOverlay() {
        overlay.SetActive(false);
        overlay.GetComponent<CanvasGroup>().alpha = 0;
        player.GetComponent<MoveCharacter>().canMove = true;
        player.layer = 8;
    }

    //currently obsolete
    //public void SwitchCamera(int index)
    //{
    //    for (int i = 1; i < cameras.Length; i++)
    //    {
    //        cameras[i].enabled = false;

    //    }
    //    if (cameras.Length > 0)
    //    {
    //        cameras[index].enabled = true;
    //        UI.GetComponent<Canvas>().worldCamera = cameras[index];
    //    }
    //}

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void SetText(string text)
    {
        textSystem.setText(text);
    }

    public void OpenDialogue(TextAsset text)
    {
        dialoguePanel.SetActive(true);
        GetComponent<DialogueManager>().StartDialogue(text);
        player.GetComponent<MoveCharacter>().canMove = false;
        player.layer = 2;
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        player.GetComponent<MoveCharacter>().canMove = true;
        player.layer = 8;
    }

    //function to update story progress from anywhere
    public void SetSpecificVar(string storyName, string varName)
    {
        string saveName = "dialogue_" + storyName;
        FieldInfo fieldInfo = save.data.GetType().GetField(saveName);
        
        if (fieldInfo != null)
        {
            object fieldValue = fieldInfo.GetValue(save.data);
            List<string> valueList = fieldValue as List<string>;
            if(!valueList.Contains(varName))
            {
                fieldInfo.SetValue(fieldInfo, varName);
            }
        }
    }
}
