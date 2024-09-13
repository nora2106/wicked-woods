using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorBehavior : UsableObject
{
    private static System.Timers.Timer aTimer;
    public string nextScene;
    public bool open;
    private bool isColliding;

    public void Start()
    {

    }

    void Update() {
        if(locked == false && gameObject.GetComponent<InspectObject>()) {
            Destroy(gameObject.GetComponent<InspectObject>());
        }
        if(open == true &&isColliding == true )
        {
            if (nextScene != "")
            {
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            }
        }
    }

    //unlock door
    override public void Action()
    {
        if(locked == false) {
            open = true;
            //open animation
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isColliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isColliding = false;
        }
    }

    IEnumerator LoadNext(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    public override void OpenAnimation()
    {
        //add animation
    }
}
