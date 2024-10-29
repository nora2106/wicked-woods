using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectObject : MonoBehaviour
{
    public string comment;
    private GameManager gm;
    void Start()
    {
        gm = GameManager.Instance;
    }

    private void OnMouseDown()
    {
        gm.SetText(comment);
    }
}
