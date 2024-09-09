using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailView : MonoBehaviour
{
    private GameObject player;
    public Button closeBtn;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        Close();
        closeBtn.onClick.AddListener(Close);
    }

    public void Open(){
        transform.gameObject.SetActive(true);
        player.GetComponent<MoveCharacter>().canMove = false;
    }

    public void Close() {
        transform.gameObject.SetActive(false);
        player.GetComponent<MoveCharacter>().canMove = true;
    }
}
