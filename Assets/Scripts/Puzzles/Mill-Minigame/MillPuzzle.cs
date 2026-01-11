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
        
        var viewFactory = new MillViewFactory();
        var view = viewFactory.View;

        var ruleFactory = new MillRulesFactory();
        var rules = ruleFactory.Rules;

        var controllerFactory = new MillControllerFactory(model, view, rules);
        var controller = controllerFactory.Controller;

        model.InitializeBoard();
        view.InitializeBoard(pointPrefab, spacing);
        controller.StartGame();
    }
}