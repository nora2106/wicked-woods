using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string id;
    public LocalizedString displayName;
    public string combineWith;
    public bool reusable;
    public Sprite sprite;
    public GameObject prefab;
    public ItemData newItem;
}
