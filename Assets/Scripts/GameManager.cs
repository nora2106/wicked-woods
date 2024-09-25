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
    GameObject leftArrow;
    GameObject rightArrow;
    public string nextScene;

    void Start()
    {
        overlay.SetActive(false);
        overlay.GetComponent<CanvasGroup>().alpha = 0;
        leftArrow = UI.transform.GetChild(0).gameObject;
        rightArrow = UI.transform.GetChild(1).gameObject;
        SceneManager.activeSceneChanged += ManageScenes;
        ManageScenes(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
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
        objs = FindGameObjectsInLayer(6);
        foreach(GameObject go in objs) {
            go.layer = 2;
        }
    }

    public void CloseOverlay() {
        overlay.SetActive(false);
        overlay.GetComponent<CanvasGroup>().alpha = 0;
        player.GetComponent<MoveCharacter>().canMove = true;
        foreach(GameObject go in objs) {
            go.layer = 6;
        }
    }

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

    public void ChangeScene()
    {
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    public void ResetPlayerPos(Vector2 pos)
    {
        player.transform.position = pos;
    }

    public void ManageScenes(Scene oldScene, Scene newScene)
    {


        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
        switch (newScene.name) {
            default:
                break;
            case "Livingroom":
                {
                    leftArrow.SetActive(true);
                    rightArrow.SetActive(false);
                    nextScene = "Kitchen";
                }
                break;
            case "Kitchen":
                {
                    leftArrow.SetActive(false);
                    rightArrow.SetActive(true);
                    nextScene = "Livingroom";
                }
                break;
        }
    }
}
