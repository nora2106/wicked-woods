using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UsableObject : MonoBehaviour
{
    protected Text displayText;
    public string usableItemID;

    private void Update()
    {

    }

    public abstract void Action();
}
