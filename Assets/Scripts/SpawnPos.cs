using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//get spawn position in target scene dependent on origin scene
public class SpawnPos : MonoBehaviour
{
    public Vector2 defaultPos = Vector2.zero;
    public Vector2 getPosition(string scene)
    {
        foreach(Transform t in gameObject.transform)
        {
            if(t.gameObject.name == scene)
            {
                return t.position;
            }
        }
        return defaultPos;
    }
} 
