using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UsableObject : MonoBehaviour
{
    protected Text displayText;
    public string usableItemID;
    public bool locked;

    private void OnMouseDown() {
        Action();
    }

    public abstract void Action();

    public void Unlock() {
        //add animation
        locked = false;
        OpenAnimation();
    }

    public abstract void OpenAnimation();
}
