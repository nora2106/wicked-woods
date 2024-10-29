using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InteractionData : ScriptableObject
{
    public string objectName;
    public string defaultComment;
    public List<ItemInteraction> itemInteractions;

    [System.Serializable]
    public class ItemInteraction
    {
        public string requiredItem;
        public string comment;
        public bool action;
    }
}
