using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPuzzle : PuzzleManager
{
    public override bool CheckIfSolved()
    {
        foreach(Transform t in transform)
        {
            if(t.gameObject.GetComponent<DoorPlank>() && !t.gameObject.GetComponent<DoorPlank>().complete)
            {
                return false;
            }
        }
        return true;
    }

    //TODO add success animation
    public override void Success()
    {
        print("Success");
        DisablePuzzle();
    }

    public override void ResetPuzzle()
    {
    }

    public override void DisablePuzzle()
    {
        GameObject door = gameObject.GetComponent<DetailView>().obj;
        door.GetComponent<DoorBehavior>().enabled = true;
        if(door.GetComponent<OpenObject>())
        {
            Destroy(door.GetComponent<OpenObject>());
        }
        if(door.transform.childCount > 0)
        {
            Destroy(door.transform.GetChild(0).gameObject);
        }
    }
}
