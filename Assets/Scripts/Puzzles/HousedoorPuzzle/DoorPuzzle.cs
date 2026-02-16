using UnityEngine;

public class DoorPuzzle : PuzzleManager
{
    public void Start()
    {
        //disable puzzle if it was already solved
        if(solved)
        {
            DisablePuzzle();
        }
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
    }
}
