using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistence : MonoBehaviour
{

    private static GameObject[] persistentObjects = new GameObject[3];
    public int index;

    // Start is called before the first frame update
    public void Awake()
    {
        print(persistentObjects[index]);
        if(persistentObjects[index] == null)
        {
            persistentObjects[index] = gameObject;
            DontDestroyOnLoad(gameObject);
        }

        else if(persistentObjects[index] != gameObject)
        {
            Destroy(gameObject);
        }
    }
}