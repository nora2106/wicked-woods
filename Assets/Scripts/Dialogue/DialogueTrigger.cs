using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset dialogueJSON;

    private void OnMouseDown()
    {
        GameManager.Instance.OpenDialogue(dialogueJSON);
    }
}
