using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWithPuzzle : UsableObject
{
    public GameObject puzzle;

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Action()
    {
        puzzle.GetComponent<PuzzleManager>().OpenPuzzle();
    }

    public override void OpenAnimation()
    {
        //
    }
}
