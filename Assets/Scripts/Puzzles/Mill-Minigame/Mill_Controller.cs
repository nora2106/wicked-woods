using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public interface IMillController
{
    public void StartGame();
}

public class MillController : IMillController
{
    private readonly IMillModel model;
    private readonly IMillView view;
    private readonly IMillRules rules;

    /* current phase (default: setup) */
    private string gamePhase;
    private int remainingStones = 18;
    private bool playerTurn;
    private PointClickedEventArgs selectedField = null;

    public MillController(IMillModel model, IMillView view, IMillRules rules)
    {
        this.model = model;
        this.view = view;
        this.rules = rules;

        /* subscribe to the view board change event */
        view.OnBoardChanged += HandlePlayerInput;
    }

    /// <summary>
    /// Handle player input and call action depending on the game phase.
    /// </summary>
    /// <param name="sender">The object triggering the Event.</param>
    /// <param name="e">The Event.</param>
    public void HandlePlayerInput(object sender, PointClickedEventArgs e)
    {
        if (!playerTurn)
        {
            return;
        }
        
        rules.PlaceOrMoveStone(model, e.Key);
        UpdateView();
        SwitchTurn();

        // place stone
        // if (gamePhase == "setup" && e.State == 0)
        // {
        //     ExecuteMove(e.Key, 1);
        //     remainingStones--;
        // }

        // if (gamePhase == "move")
        // {
        //     // click on full field -> select it and choose another field 
        //     if (e.State == 1)
        //     {
        //         selectedField = e;
        //         view.UpdateField(selectedField.Key, 3);
        //         UnityEngine.Debug.Log("click on any empty neighboring field to move selected stone.");
        //     }
        //     // selected field -> check if another field is neighbor 
        //     else if (e.State == 0 && selectedField != null && model.IsNeighbor(selectedField.Key, e.Key))
        //     {
        //         model.UpdateField(selectedField.Key, 0);
        //         view.UpdateField(selectedField.Key, 0);
        //         selectedField = null;
        //         ExecuteMove(e.Key, 1);
        //     }
        // }

        // // remove stone from field if enemy stone and not in a mill
        // else if (gamePhase == "remove" && e.State == 2 && !model.CheckForMills(e.Key, e.State))
        // {
        //     gamePhase = "move";
        //     if (remainingStones > 0)
        //     {
        //         gamePhase = "setup";
        //     }
        //     ExecuteMove(e.Key, 0);
        //     return;
        // }

        // // change gamephase if all stones are placed
        // if (remainingStones == 0)
        // {
        //     gamePhase = "move";
        // }
    }

    /// <summary>
    /// Switches which player's turn it is.
    /// </summary>
    private void SwitchTurn()
    {
        if (playerTurn)
        {
            playerTurn = false;
            EnemyMove();
        }
        playerTurn = true;

    }

    /// <summary>
    /// Calculate enemy move.
    /// </summary>
    private void EnemyMove()
    {
        rules.ExecuteEnemyMove(model);
        UpdateView();
        SwitchTurn();
        // int newFieldKey = 0;
        // int newState = 2;

        // if (gamePhase == "setup")
        // {
        //     newFieldKey = GetRandomField(0);
        //     remainingStones--;
        //     if (remainingStones == 0)
        //     {
        //         gamePhase = "move";
        //     }
        // }
        // else if (gamePhase == "move")
        // {
        //     for(int i = 0; i < model.GameBoard.Count; i++)
        //     {
        //         List<int> neighbors = model.GameBoard[i].neighbors;
        //         for(int j = 0; j < neighbors.Count; j++)
        //         {
        //             // if(model.GameBoard[neighbors[j]].state == 0)
        //             // {
        //             //     model.UpdateField(i, 0);
        //             //     view.UpdateField(i, 0);
        //             //     newFieldKey = neighbors[j];
        //             //     newState = 2;
        //             // }
        //         }
        //     }
        //     // TODO pathfinding algorithm
        // }

        // ExecuteMove(newFieldKey, newState);
    }

    // Aligns the view board to the model board.
    private void UpdateView()
    {
        view.UpdateBoard(model.GameBoard);
    }

    /// <summary>
    /// Either place new stone, move existing stone or remove stone.
    /// </summary>
    /// <param name="key">Key of the changed field.</param>
    /// <param name="newState">New state of the changed field.</param>
    public void ExecuteMove(int key, int newState)
    {
        // TODO end game if one player has only 2 stones
        // if (gamePhase == "")
        // {
        //     return;
        // }
        // model.UpdateField(key, newState);
        // view.UpdateField(key, newState);
        
        // if (model.CheckForMills(key, newState))
        // {
        //     if(playerTurn)
        //     {
        //         gamePhase = "remove";
        //         UnityEngine.Debug.Log("click on any enemy stone to remove.");
        //     }
        //     else
        //     {
        //         EnemyRemove();
        //     }
        // }
        // SwitchTurn();
    }

    

    /// <summary>
    /// Remove player stone after the enemy forms a mill.
    /// </summary>
    private void EnemyRemove()
    {
        // for(int i = 0; i < model.GameBoard.Count; i++)
        // {
        //     if(model.GameBoard[i].state == 1 && !model.CheckForMills(i, 1))
        //     {
        //         ExecuteMove(i, 0);
        //     }
        // }
    }

    /// <summary>
    /// Gets a random board key that matches the required state.
    /// </summary>
    /// <param name="state"> Required field state. </param>
    // public int GetRandomField(int state)
    // {
    //     int newField;
    //     System.Random rnd = new System.Random();

    //     void CheckIfEmpty(int field)
    //     {
    //         if (model.GameBoard[field].state == state)
    //         {
    //             newField = field;
    //         }
    //         else
    //         {
    //             CheckIfEmpty(rnd.Next(0, model.GameBoard.Count));
    //         }
    //     }

    //     CheckIfEmpty(rnd.Next(0, model.GameBoard.Count));
    //     return newField;
    // }

    public void StartGame()
    {
        // start game
        gamePhase = "setup";
        playerTurn = true;
    }

    public void StopGame()
    {
        gamePhase = "";
    }
}