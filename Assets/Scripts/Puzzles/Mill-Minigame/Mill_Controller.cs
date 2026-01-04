using System;
using System.Diagnostics;
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

    /* current phase (default: setup) */
    private string gamePhase;
    private int remainingStones = 18;
    private bool playerTurn;
    private PointClickedEventArgs selectedField = null;

    public MillController(IMillModel model, IMillView view)
    {
        this.model = model;
        this.view = view;

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
        int newState = 0;

        // place stone
        if (gamePhase == "setup" && e.State == 0)
        {
            newState = 1;
            remainingStones--;
        }

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

        // remove stone from field if enemy stone and not in a mill
        else if (gamePhase == "remove" && e.State == 2 && !model.CheckForMills(e.Key, e.State))
        {
            newState = 0;
            gamePhase = "move";
            if (remainingStones > 0)
            {
                gamePhase = "setup";
            }
            ExecuteMove(e.Key, newState);
            return;
        }

        ExecuteMove(e.Key, newState);

        // change game phase if mill was made
        if (model.CheckForMills(e.Key, newState))
        {
            gamePhase = "remove";
        }
        // change gamephase if all stones are placed
        if (remainingStones == 0)
        {
            gamePhase = "move";
        }
        EnemyMove();
    }

    /// <summary>
    /// Either place new stone, move existing stone or remove stone.
    /// </summary>
    /// <param name="key">Key of the changed field.</param>
    /// <param name="newState">New state of the changed field.</param>
    public void ExecuteMove(int key, int newState)
    {
        model.UpdateField(key, newState);
        view.UpdateField(key, newState);
    }

    /// <summary>
    /// Calculate enemy move.
    /// </summary>
    public void EnemyMove()
    {
        int newFieldKey = 0;
        int newState = 2;

        if (gamePhase == "setup")
        {
            newFieldKey = GetRandomField(0);
            remainingStones--;
            if (remainingStones == 0)
            {
                gamePhase = "move";
            }
        }
        else if (gamePhase == "move")
        {
            // not working (causing stack overflow)
            void CheckNeighbors(int key)
            {
                model.GameBoard[key].neighbors.ForEach((node) =>
                {
                    if(model.GameBoard[node].state == 0)
                    {
                        ExecuteMove(key, 0);
                        newFieldKey = node; 
                        return;
                    }
                });
                CheckNeighbors(GetRandomField(2));
            }
            CheckNeighbors(GetRandomField(2));

            // TODO pathfinding algorithm
        }
        // mill formed - remove random player stone
        if (model.CheckForMills(newFieldKey, newState))
        {
            newFieldKey = GetRandomField(1);
            newState = 0;
        }
        
        ExecuteMove(newFieldKey, newState);
        playerTurn = true;
    }

    /// <summary>
    /// Gets a random board key that matches the required state.
    /// </summary>
    /// <param name="state"> Required field state. </param>
    public int GetRandomField(int state)
    {
        int newField;
        System.Random rnd = new System.Random();

        void CheckIfEmpty(int field)
        {
            if (model.GameBoard[field].state == state)
            {
                newField = field;
            }
            else
            {
                CheckIfEmpty(rnd.Next(0, model.GameBoard.Count));
            }
        }

        CheckIfEmpty(rnd.Next(0, model.GameBoard.Count));
        return newField;
    }

    public void StartGame()
    {
        // start game
        gamePhase = "setup";
        playerTurn = true;
    }
}