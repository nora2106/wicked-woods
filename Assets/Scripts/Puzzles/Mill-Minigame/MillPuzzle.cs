using System;
using JetBrains.Annotations;
using UnityEngine;

public class MillPuzzle : PuzzleManager
{
    public override bool CheckIfSolved()
    {
        throw new System.NotImplementedException();
    }

    public override void DisablePuzzle()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetPuzzle()
    {
        throw new System.NotImplementedException();
    }

    public override void Success()
    {
        throw new System.NotImplementedException();
    }

    void Awake()
    {
        var modelFactory = new MillModelFactory();
        var model = modelFactory.Model;
        model.InitializeBoard();
        BoardPosition testPos = model.GameBoard[11];
        Debug.Log(testPos.x + "" + testPos.y);
        var neighbors = model.GetNeighborIDs(testPos, 11);
        // Debug.Log(neighbors);

        
        var viewFactory = new MillViewFactory();
        var view = viewFactory.View;
    }
}