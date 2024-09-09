using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorBehavior : UsableObject
{
    private static System.Timers.Timer aTimer;
    public string nextScene;

    public void Start()
    {

    }

    void Update() {
        if(locked == false && gameObject.GetComponent<InspectObject>()) {
            Destroy(gameObject.GetComponent<InspectObject>());
        }
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
        if (nextScene != "")
        {
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
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
