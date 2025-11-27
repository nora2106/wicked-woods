using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

//output comment about an inspectable object to monologue if exists
public class InspectObject : MonoBehaviour
{
    public LocalizedString localizedComment;
    private string comment;
    private GameManager gm;
    void Start()
    {
        gm = GameManager.Instance;
        localizedComment.StringChanged += setComment;
    }

    private void OnMouseDown()
    {
        gm.QueueInteraction(new ActionCommand(SetText));
    }

    public void SetText()
    {
        if (comment != "")
        {
            gm.SetText(comment);
        }
    }

    public void setComment(string newStr)
    {
        comment = newStr;
    }
}
