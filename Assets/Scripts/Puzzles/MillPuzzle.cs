using UnityEngine;

public class MillPuzzle : PuzzleManager
{
    public MillViewConfig config;
    public int stoneCount = 9;

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

        model.AvailableStones[FieldState.Player] = stoneCount;
        model.AvailableStones[FieldState.Enemy] = stoneCount;
        view.InitializeBoard(config);
        controller.StartGame();
    }
}