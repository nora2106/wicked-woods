using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset dialogueJSON;

    private void OnMouseDown()
    {
        if(GameManager.Instance.selectedItemID == "")
        {
            GameManager.Instance.OpenDialogue(dialogueJSON);
        }
    }
}
