using System;
using System.Diagnostics;
using UnityEditor;

public interface IMillController
{
    public void StartGame();
    public void SyncViewToModel();
    
}

public class MillController : IMillController
{
    private readonly IMillModel model;
    private readonly IMillView view;

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
       model.UpdateField(e.Key, e.State);
    }

    public void StartGame()
    {
        // start game
    }

    // sync view board data with model data
    public void SyncViewToModel()
    {
        view.GameBoard = model.GameBoard;
    }
}