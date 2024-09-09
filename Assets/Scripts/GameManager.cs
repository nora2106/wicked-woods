using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public GameObject overlay;
    public GameObject player;
    private GameObject[] objs;
    private void Update()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject go in FindGameObjectsInLayer(6)) {
            var pos = transform.position;
            gameObject.transform.position = new Vector3(pos.x, pos.y, 2);
        }
        overlay.SetActive(false);
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
        player.GetComponent<MoveCharacter>().canMove = false;
        objs = FindGameObjectsInLayer(6);
        foreach(GameObject go in objs) {
            go.layer = 2;
        }
    }

    public void CloseOverlay() {
        overlay.SetActive(false);
        player.GetComponent<MoveCharacter>().canMove = true;
        foreach(GameObject go in objs) {
            go.layer = 6;
        }
    }
}
