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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.name == gameObject.name)
            {
                if(!locked) {
                    Action();
                }
            }
        }
    }

    override public void Action()
    {
        LoadNext(2);
        usableItemID = null;
        if(newSprite != null) {
            gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }

    IEnumerator LoadNext(float duration)
    {
        print(nextScene);
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    private static void SetTimer(int time)
    {
        aTimer = new System.Timers.Timer(time);
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }
}
