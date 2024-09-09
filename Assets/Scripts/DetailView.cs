using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailView : MonoBehaviour
{
    public Button closeBtn;
    public GameObject obj;
    private GameObject manager;
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(false);
        manager = GameObject.FindWithTag("GameController");
        closeBtn.onClick.AddListener(Close);
    }

    public void Open(){
        transform.gameObject.SetActive(true);
        manager.GetComponent<GameManager>().OpenOverlay();
    }

    public void Close() {
        transform.gameObject.SetActive(false);
        manager.GetComponent<GameManager>().CloseOverlay();
        if(obj) {
            obj.GetComponent<OpenObject>().Close();
        }
    }
}
