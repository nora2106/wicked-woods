using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu]
public class InteractionData : ScriptableObject
{
    public LocalizedString objectName;
    public LocalizedString defaultComment;
    public List<ItemInteraction> itemInteractions;

    [System.Serializable]
    public class ItemInteraction
    {
        public string requiredItem;
        public LocalizedString comment;
        public bool action;
    }
}
