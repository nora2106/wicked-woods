using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Reflection;
using Ink.Runtime;

public class GameManager : MonoBehaviour
{
    public SpawnPos spawnPos;
    public static GameManager Instance;
    [HideInInspector] public GameObject dialoguePanel;
    [HideInInspector] public SaveManager save;
    [HideInInspector] public MonologueSystem textSystem;
    [HideInInspector] public GameObject overlay;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Inventory inventory;
    [HideInInspector] public string selectedItemID;
    [HideInInspector] public bool itemSelected = false;
    [HideInInspector] public bool dialoguePlaying = false;
    [HideInInspector] public bool paused = false;
    [HideInInspector] public DialogueManager dialogue;
    [HideInInspector] public MoveCharacter movement;
    private GameObject[] objs;
    private string currentScene;
    public GameObject actionText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        currentScene = SceneManager.GetActiveScene().name;
        dialogue = GetComponent<DialogueManager>();
        dialoguePanel = GameObject.FindWithTag("DialogueCanvas");
        dialoguePanel.SetActive(false);
        save = gameObject.GetComponent<SaveManager>();
        inventory = GameObject.FindWithTag("inventory").GetComponent<Inventory>();
        textSystem = GameObject.FindWithTag("Monologue").GetComponent<MonologueSystem>();
        player = GameObject.FindWithTag("Player");
        movement = player.GetComponent<MoveCharacter>();
        actionText = GameObject.FindWithTag("Text");
        actionText.transform.parent.gameObject.SetActive(false);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SetupAll();
        //player.GetComponent<MoveCharacter>().SetPosition(save.data.playerPosition);
    }

    public void Setup()
    {
        spawnPos = FindObjectOfType<SpawnPos>();
        overlay = GameObject.FindWithTag("Overlay");
        if(overlay != null)
        {
            overlay.GetComponent<Canvas>().worldCamera = Camera.main;
            overlay.SetActive(false);
            overlay.GetComponent<CanvasGroup>().alpha = 0;
        }
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
        save.Save();
        SetupAll();
        player.GetComponent<MoveCharacter>().SetPosition(spawnPos.getPosition(currentScene));
        currentScene = scene.name;
        save.data.scene = currentScene;
        ManageScene(currentScene);
    }

    private void SetupAll()
    {
        save.savePath = Application.persistentDataPath + "/gamedata.json";
        save.GetSaveData();
        Setup();
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
        PauseCharacterMovement();
        player.layer = 2;
    }

    public void CloseOverlay() {
        overlay.SetActive(false);
        overlay.GetComponent<CanvasGroup>().alpha = 0;
        ResumeCharacterMovement();
        player.layer = 8;
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    // TODO add capitalization helper function to text

    //set monologue text
    public void SetText(string text)
    {
        textSystem.SetText(text);
    }

    //set action text
    public void SetActionText(string text)
    {
        if (!dialoguePlaying && !paused)
        {
            actionText.GetComponent<Text>().text = text;
            actionText.transform.parent.gameObject.SetActive(true);
        }
    }

    public void ResetActionText ()
    {
        actionText.GetComponent<Text>().text = "";
        actionText.transform.parent.gameObject.SetActive(false);
    }

    public void OpenDialogue(TextAsset text)
    {
        dialoguePlaying = true;
        dialoguePanel.SetActive(true);
        GetComponent<DialogueManager>().StartDialogue(text);
        PauseCharacterMovement();
        player.layer = 2;
    }
    // TODO replace by general pause & resume function

    public void CloseDialogue()
    {
        dialoguePlaying = false;
        dialoguePanel.SetActive(false);
        ResumeCharacterMovement();
        player.layer = 8;
    }

    //pause and resume the character's ability to move
    public void PauseCharacterMovement()
    {
        player.GetComponent<MoveCharacter>().canMove = false;
    }

    public void ResumeCharacterMovement()
    {
        player.GetComponent<MoveCharacter>().canMove = true;
    }

    //function to update story progress from anywhere
    public void SetSpecificVar(string storyName, string varName)
    {
        string saveName = "dialogue_" + storyName;
        var field = save.data.GetType().GetField(saveName);

        if (field != null && typeof(IList).IsAssignableFrom(field.FieldType))
        {
            var list = (IList)field.GetValue(save.data);

            if(!list.Contains(varName))
            {
                list?.Add(varName);
            }
        }
    }

    //check if specific story var is true (exists in save data)
    public bool GetStoryVar(string storyName, string varName)
    {
        string saveName = "dialogue_" + storyName;
        var field = save.data.GetType().GetField(saveName);
        if (field != null && typeof(IList).IsAssignableFrom(field.FieldType))
        {
            var list = (IList)field.GetValue(save.data);

            if (list.Contains(varName))
            {
                return true;
            }
        }
        return false;
    }

    // manage settings for specific scenes
    private void ManageScene(string scene)
    {
        if(scene == "Street")
        {
            player.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
    }
}