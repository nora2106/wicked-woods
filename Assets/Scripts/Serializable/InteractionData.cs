using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu]
public class InteractionData : ScriptableObject
{
    public LocalizedString objectName;
    public string objectID;
    public LocalizedString defaultComment;
    public List<ItemInteraction> itemInteractions;

    [System.Serializable]
    public class ItemInteraction
    {
        public string requiredItem;

        //individual comment for mismatched items
        public LocalizedString comment;
        
        //variable used in dialogue procession
        public string varName;
        public bool action;
    }
}
