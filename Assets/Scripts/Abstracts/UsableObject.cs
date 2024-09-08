using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UsableObject : MonoBehaviour
{
    protected Text displayText;
    public string usableItemID;

    private void OnMouseDown() {
        Action();
    }

    public abstract void Action();
}
