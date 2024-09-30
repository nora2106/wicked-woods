using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPuzzle : PuzzleManager
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public override void Success()
    {
        print("Success");
    }
}
