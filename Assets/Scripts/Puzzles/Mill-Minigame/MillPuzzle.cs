using System;
using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MillPuzzle : PuzzleManager
{
    public GameObject pointPrefab;
    public float spacing = 1.452f;
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
        // BoardPosition testPos = model.GameBoard[22];
        // var neighbors = model.GetNeighbors(testPos, 22);
        
        var viewFactory = new MillViewFactory();
        var view = viewFactory.View;
        
        var controllerFactory = new MillControllerFactory(model, view);
        var controller = controllerFactory.Controller;

        model.InitializeBoard();
        controller.SyncViewToModel();
        view.InitializeBoard(pointPrefab, spacing);
    }
}