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
    private int remainingStones = 18;
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
            remainingStones--;
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

        else if(gamePhase == "remove" && e.State == 2)
        {
            newState = 0;
            gamePhase = "move";
            if(remainingStones > 0)
            {
                gamePhase = "setup";
            }
        }

        // update model and view boards
        model.UpdateField(e.Key, newState);
        view.UpdateField(e.Key, newState);

        // change game phase if mill was made
        if(model.CheckForMills(e.Key, newState))
        {
            gamePhase = "remove";
        }
        // change gamephase if all stones are placed
        else if (remainingStones == 0)
        {
            gamePhase = "move";
        }
        EnemyMove();
    }

    public void EnemyMove()
    {
        if(gamePhase == "setup")
        {
            remainingStones--;
        }
        else if(gamePhase == "move")
        {
            // pathfinding algorithm
        }
    }

    public void StartGame()
    {
        // start game
        gamePhase = "setup";
    }
}