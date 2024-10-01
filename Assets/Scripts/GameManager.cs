using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject overlay;
    public GameObject player;
    private GameObject[] objs;
    private Camera[] cameras;
    public GameObject UI;
    public static GameManager Instance;
    public Text displayText;
    public GameData gameData;
    private SaveManager sm;
    void Start()
    {
        sm = gameObject.GetComponent<SaveManager>();
        Instance = this;
        SetupAll();
        SceneManager.activeSceneChanged += OnSceneChange;
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (gameData.scene != "" && gameData.scene != SceneManager.GetActiveScene().name)
        {
            //uncomment later, enables changing to last visited scene
            //ChangeScene(gameData.scene);
        }
    }

    public void Setup()
    {
        sm = gameObject.GetComponent<SaveManager>();
        sm.GetSaveData();
        gameData = gameObject.GetComponent<SaveManager>().data;
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
    }

    private void OnSceneChange(Scene lastScene, Scene nextScene)
    {
        gameData.scene = nextScene.name;
        sm.Save();
    }

    private void SetupAll()
    {
        var setupObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISetup>();
        foreach (ISetup obj in setupObjects)
        {
            obj.Setup();
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
    }

    public void CloseOverlay() {
        overlay.SetActive(false);
        overlay.GetComponent<CanvasGroup>().alpha = 0;
        player.GetComponent<MoveCharacter>().canMove = true;
    }

    //currently obsolete
    public void SwitchCamera(int index)
    {
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].enabled = false;

        }
        if (cameras.Length > 0)
        {
            cameras[index].enabled = true;
            UI.GetComponent<Canvas>().worldCamera = cameras[index];
        }
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void ResetPlayerPos(Vector2 pos)
    {
        player.transform.position = pos;
    }

    public void SetText(string text)
    {
        displayText.text = text;
    }
}
