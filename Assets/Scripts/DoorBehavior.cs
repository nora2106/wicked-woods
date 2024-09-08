using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorBehavior : UsableObject
{
    public Sprite newSprite;
    private static System.Timers.Timer aTimer;
    public string nextScene;
    public bool locked;

    public void Start()
    {
        
    }

    void Update() {
        /* if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.name == gameObject.name)
            {
                OpenDoor();
            }
        } */
    }

    //unlock door
    override public void Action()
    {
        if(locked == false) {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        if (newSprite != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        }

        if (!locked && nextScene != "")
        {
            StartCoroutine(LoadNext(1));
        }
    }
    IEnumerator LoadNext(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
}
