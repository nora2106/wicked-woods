using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenPuzzle : PuzzleManager
{
    public GameObject item;
    // Start is called before the first frame update
    void Start()
    {
        //make key not collectable
        if(item)
        {
            item.GetComponent<Collider2D>().enabled = false;
        }
    }

    public override void DisablePuzzle()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        item = null;
    }

    public override void ResetPuzzle()
    {
        gameObject.GetComponent<OvenCoalstack>().ResetToStart();
    }

    public override bool CheckIfSolved()
    {
        return solved;
    }

    public override void Success(){
        print("success");
        item.GetComponent<Collider2D>().enabled = true;
        solved = true;
    }
}
