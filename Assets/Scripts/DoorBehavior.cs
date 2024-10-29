using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorBehavior : UsableObject
{
    public string nextScene;
    public bool open;
    private bool isColliding;

    void Update() {
        if(open == true &&isColliding == true )
        {
            if (nextScene != "")
            {
                gm.ChangeScene(nextScene);
            }
        }
    }

    //unlock door
    override public void Action()
    {
        if(!locked) {
            open = true;
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

    public override void OpenAnimation()
    {
        //add animation
    }
}
