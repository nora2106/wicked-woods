using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// interactable door object
public class DoorObject : UsableObject
{
    public string nextScene;

    override public void Action()
    {
        gm.QueueInteraction(new ChangeSceneCommand(nextScene));
    }

}
