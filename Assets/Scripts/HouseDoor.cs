using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//specific behavior of the house door
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
            //gameObject.GetComponent<DoorBehavior>().nextScene = "";
            Destroy(gameObject.GetComponent<OpenObject>());
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
