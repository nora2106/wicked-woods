using System;
using UnityEditor;

public interface IMillController
{
    public void StartGame();
    
}

public class MillController : IMillController
{
    private readonly IMillModel model;
    private readonly IMillView view;

    public MillController(IMillModel model, IMillView view)
    {
        this.model = model;
        this.view = view;
    }

    public void StartGame()
    {
        // start game
    }

    // sync view board data with model data
    private void SyncGameBoard()
    {
        // view.gameBoard = model.GameBoard;
    }
}