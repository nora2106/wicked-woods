using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public string useOn;
    public bool reusable;
    public Sprite sprite;
    public GameObject prefab;
}
