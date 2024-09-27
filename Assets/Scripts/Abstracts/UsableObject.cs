using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UsableObject : MonoBehaviour
{
    protected Text displayText;
    public string usableItemID;
    public bool locked;

    private void OnMouseDown() {
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            Action();
        }
    }

    public abstract void Action();

    public void Unlock() {
        //add animation
        locked = false;
        OpenAnimation();
    }

    public abstract void OpenAnimation();
}
