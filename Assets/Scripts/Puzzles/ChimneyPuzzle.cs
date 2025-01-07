using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimneyPuzzle : PuzzleManager
{
    public GameObject item;
    private bool fire = true;
    // Start is called before the first frame update
    void Start()
    {
        if(item)
        {
            item.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void Update()
    {
        if(fire && !solved)
        {
            CheckProgress();
        }
    }

    public override bool CheckIfSolved()
    {
        if (gm.GetStoryVar("dad", "dad_awake"))
        {
            return true;
        }
        return false;
    }

    public override void ResetPuzzle()
    {
    }

    public override void Success()
    {
        //extinguish fire
        print("fire gets blown out");
        fire = false;
        solved = true;
        item.GetComponent<Collider2D>().enabled = true;
    }

    public override void DisablePuzzle()
    {
        fire = false;
        // fire not burning
    }
}
