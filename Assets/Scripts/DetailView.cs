using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//opens the detail view of an interactable object
public class DetailView : MonoBehaviour
{
    public Button closeBtn;
    public GameObject obj;
    private GameManager gm;
    
    void Start()
    {
        transform.gameObject.SetActive(false);
        gm = GameManager.Instance;
        closeBtn.onClick.AddListener(Close);
    }

    public void Open(){
        transform.gameObject.SetActive(true);
        gm.OpenOverlay();
    }

    public void Close() {
        transform.gameObject.SetActive(false);
        gm.CloseOverlay();
        if(obj && obj.GetComponent<OpenObject>()) {
            obj.GetComponent<OpenObject>().Close();
        }
    }
}
