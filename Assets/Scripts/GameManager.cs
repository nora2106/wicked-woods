using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public GameObject overlay;
    public GameObject player;
    private GameObject[] objs;

    void Start()
    {
        overlay.SetActive(false);
        overlay.GetComponent<CanvasGroup>().alpha = 0;
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
}
