using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject overlay;
    public GameObject player;
    private GameObject[] objs;
    public Camera[] cameras;
    public GameObject UI;
    public static GameManager Instance;
    void Start()
    {
        Instance = this;
        Setup();
        SceneManager.activeSceneChanged += ManageScenes;
    }

    public void Setup()
    {
        overlay = GameObject.FindWithTag("Overlay");
        overlay.GetComponent<Canvas>().worldCamera = Camera.main;
        overlay.SetActive(false);
        overlay.GetComponent<CanvasGroup>().alpha = 0;
        objs = FindGameObjectsInLayer(6);
        foreach (GameObject obj in objs)
        {
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 10);
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

    public void ManageScenes(Scene oldScene, Scene newScene)
    {
        Setup();
    }
}
