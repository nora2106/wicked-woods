using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveEventArgs : EventArgs
{
}
public interface IMillView
{
    Dictionary<int, BoardPosition> GameBoard {get; set;}
    event EventHandler<PlayerMoveEventArgs> OnMove;
    void InitializeBoard();
    void UpdateBoardPosition(int key);
}

public class MillView : IMillView
{
    public Dictionary<int, BoardPosition> gameBoard;
    private readonly BoardPoint[] boardPoints = new BoardPoint[23];

    public event EventHandler<PlayerMoveEventArgs> OnMove;

    public Dictionary<int, BoardPosition> GameBoard
    {
        get
        {
            return gameBoard;
        }
        set
        {
            gameBoard = value;
        }
    }

    public void InitializeBoard()
    {
        if(GameBoard.Count == 0 || GameBoard.Count != boardPoints.Length)
        {
            return;
        }

        // create board points based on board positions
        for(int i = 0; i < GameBoard.Count; i++)
        {
            boardPoints[i] = new BoardPoint(boardPoints[i].x, boardPoints[i].y, boardPoints[i].key);
        }
    }

    // update state of certain point on gameboard
    // notify controller about update
    public void UpdateBoardPosition(int key)
    {
        GameBoard[key].SetState(boardPoints[key].state);

        // notify controller about update
    }
}