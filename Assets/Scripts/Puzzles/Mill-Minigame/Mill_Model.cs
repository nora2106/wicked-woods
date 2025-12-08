using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public interface IMillModel
{
    // Dictionary containing the BoardValue (position on board) and the state 
    // (0 for empty, 1 for player 1 (white), 2 for player 2 (black))
    Dictionary<int, BoardPosition> GameBoard { get; set; }
    int[][] GetNeighborIDs(BoardPosition pos, int key);
    void InitializeBoard();
    BoardPosition GetPositionByKey(int key);
}

public struct BoardPosition
{
    public int x;
    public int y;
    public int state;
    public BoardPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
        state = 0;
    }

    public void SetState(int state)
    {
        this.state = state;
    }
}

public class MillModel : IMillModel
{
    public BoardPosition[] positions = {new(0,0), new(3,0), new(6, 0), new(1,1), new(3,1), new(5,1), new(2,2), new(4,2),
    new(0,3), new(1,3), new(2,3), new(4,3), new(5,3), new(6,3), new(2,4), new(3,4), new(4,4), new(1,5), new(3,5),
    new(5,5), new(0,6), new(3,6), new(6,6)};

    public Dictionary<int, BoardPosition> gameBoard;
    public string test;

    public Dictionary<int, BoardPosition> GameBoard
    {
        get { return gameBoard; }
        set
        {
            gameBoard = value;
        }
    }

    public BoardPosition GetPositionByKey(int key)
    {
        return GameBoard[key];
    }

    public void InitializeBoard()
    {
        gameBoard = new Dictionary<int, BoardPosition>();
        for (int i = 0; i < positions.Length; i++)
        {
            gameBoard.Add(i, positions[i]);
        }
    }

    public int[][] GetNeighborIDs(BoardPosition pos, int key)
    {
        var board = GameBoard;
        // middle points: 2 possible mills (horizontal, vertical)
        int[][] rows = new int[2][];
        List<int> row1 = new List<int>();
        List<int> row2 = new List<int>();
        List<int> row3 = null;

        // edge points: 3 possible mills (horizontal. vertical, diagonal)
        if (pos.y != 3 || pos.x != 3)
        {
            row3 = new List<int>();
            rows = new int[3][];
        }

        foreach (KeyValuePair<int, BoardPosition> pair in board)
        {
            // find vertical neighbors (same x coordinate)
            if (pair.Value.x == pos.x && pair.Key != key)
            {
                if(pos.x != 3)
                {
                    UnityEngine.Debug.Log("vertical: " + pair.Value.x + "" + pair.Value.y);
                    row1.Add(pair.Key);
                }
                else if(Math.Abs(pair.Value.y - pos.y) <= 3)
                {
                    UnityEngine.Debug.Log("vertical: " + pair.Value.x + "" + pair.Value.y);
                    row1.Add(pair.Key);
                }
            }

            // find horizontal neighbors (same y coordinate)
            if (pair.Value.y == pos.y && pair.Key != key)
            {
                if(pos.y != 3)
                {
                    UnityEngine.Debug.Log("horizontal: " + pair.Value.x + "" + pair.Value.y);
                    row2.Add(pair.Key);
                }
                else if(Math.Abs(pair.Value.x - pos.x) <= 3)
                {
                    UnityEngine.Debug.Log("horizontal: " + pair.Value.x + "" + pair.Value.y);
                    row2.Add(pair.Key);
                }
            }
            
            // find diagonal neighbors
            if (pos.y != 3 && pos.x != 3)
            {
                if (Math.Abs(pair.Value.x - pos.x) <= 3 && Math.Abs(pair.Value.y - pos.y) <= 3 && pair.Key != key)
                {
                    row3.Add(pair.Key);
                    UnityEngine.Debug.Log("diagonal: " + pair.Value.x + "" + pair.Value.y);
                }
            }

        }
        return rows;
    }
}