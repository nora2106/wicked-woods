using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<OpenObject>() && gameObject.GetComponent<OpenObject>().detail.GetComponent<PuzzleManager>().solved)
        {
            gameObject.AddComponent<DoorBehavior>();
            gameObject.GetComponent<DoorBehavior>().locked = true;
            gameObject.GetComponent<DoorBehavior>().usableItemID = "housedoor_key";
            gameObject.GetComponent<DoorBehavior>().lockedText = "Ich habe keinen Haust�rschl�ssel.";
            //gameObject.GetComponent<DoorBehavior>().nextScene = "";
            Destroy(gameObject.GetComponent<OpenObject>());
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}