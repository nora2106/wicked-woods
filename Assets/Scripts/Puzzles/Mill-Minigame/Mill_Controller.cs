using System;
using System.Diagnostics;
using UnityEditor;

public interface IMillController
{
    public void StartGame();
}

public class MillController : IMillController
{
    private readonly IMillModel model;
    private readonly IMillView view;
    private string gamePhase = "setup";
    private PointClickedEventArgs selectedField = null;

    public MillController(IMillModel model, IMillView view)
    {
        this.model = model;
        this.view = view;

        // subscribe to the view board change event
        view.OnBoardChanged += HandlePlayerInput;
    }

    // sync model board when view board is updated
    public void HandlePlayerInput(object sender, PointClickedEventArgs e)
    {
        // setup phase
        // place stone on empty field
        int newState = 0;
        if (gamePhase == "setup" && e.State == 0)
        {
            newState = 1;
        }

        // move phase
        if (gamePhase == "move")
        {
            // click on full field -> select it and choose another field 
            if (e.State == 1)
            {
                selectedField = e;
                newState = 1;
            }
            // selected field -> check if another field is neighbor
            else if (e.State == 0 && selectedField != null && model.IsNeighbor(selectedField.Key, e.Key))
            {
                newState = 1;
                model.UpdateField(selectedField.Key, 0);
                view.UpdateField(selectedField.Key, 0);
                selectedField = null;
            }
        }
        model.UpdateField(e.Key, newState);
        view.UpdateField(e.Key, newState);
        
        if (model.GetFieldsByState(1).Count == 9)
        {
            gamePhase = "move";
        }
    }

    public void StartGame()
    {
        // start game
    }
}