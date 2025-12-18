using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public interface IMillModel
{
    Dictionary<int, BoardPosition> GameBoard { get; set; }
    void InitializeBoard();
    void UpdateField(int key, int state);
    
}

public class BoardPosition
{
    public int x;
    public int y;
    // (0 for empty (default), 1 for player 1 (white), 2 for player 2 (black))
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
    // all points in the format (x,y)
    public BoardPosition[] positions = {new(0,0), new(3,0), new(6, 0), new(1,1), new(3,1), new(5,1), new(2,2), new(4,2),
    new(0,3), new(1,3), new(2,3), new(3, 2), new(4,3), new(5,3), new(6,3), new(2,4), new(3,4), new(4,4), new(1,5), new(3,5),
    new(5,5), new(0,6), new(3,6), new(6,6)};

    // Dictionary containing the BoardValue (position on board) and the current state 
    public Dictionary<int, BoardPosition> gameBoard;

    public Dictionary<int, BoardPosition> GameBoard
    {
        get { return gameBoard; }
        set
        {
            gameBoard = value;
        }
    }

    // return point by key
    private BoardPosition GetPositionByKey(int key)
    {
        return GameBoard[key];
    }

    // update field and check for any mills surrounding the updated field
    public void UpdateField(int key, int state)
    {
        GameBoard[key].SetState(state);
        CheckForMills(key, state);
    }

    // create gameboard and add points
    public void InitializeBoard()
    {
        GameBoard = new Dictionary<int, BoardPosition>();
        for (int i = 0; i < positions.Length; i++)
        {
            GameBoard.Add(i, positions[i]);
        }
    }

    public void CheckForMills(int key, int currentState)
    {
        // Debug.Log(GetPositionByKey(key).state);
        List<int>[] neighborRows = GetNeighbors(key);
        for(int i = 0; i < neighborRows.Length; i++)
        {
            var row = neighborRows[i];
            // Debug.Log(GetPositionByKey(row[0]).state);
            // Debug.Log(GetPositionByKey(row[1]).state);
            Debug.Log("row" + i + ": " + row[0]);
            Debug.Log("row" + i + ": " + row[1]);
            // every field in a row has 2 neighbors
            if(GetPositionByKey(row[0]).state == currentState && GetPositionByKey(row[1]).state == currentState)
            {
                Debug.Log("Mill formed");
            }
        }
    }

    // return rows of keys from points adjacent to selected point
    // every row is a possible mill with selected point
    private List<int>[] GetNeighbors(int key)
    {
        var board = GameBoard;
        BoardPosition pos = GetPositionByKey(key);
        List<int>[] rows = new List<int>[2];
        List<int> row1 = new List<int>();
        List<int> row2 = new List<int>();
        List<int> row3 = new List<int>();

        foreach (KeyValuePair<int, BoardPosition> pair in board)
        {
            // find vertical neighbors (same x coordinate)
            if (pair.Value.x == pos.x && pair.Key != key)
            {
                if(pos.x != 3)
                {
                    row1.Add(pair.Key);
                }
                // middle row contains 2 separate rows
                else if(Math.Abs(pair.Value.y - pos.y) <= 2)
                {
                    row1.Add(pair.Key);
                }
            }

            // find horizontal neighbors (same y coordinate)
            if (pair.Value.y == pos.y && pair.Key != key)
            {
                if(pos.y != 3)
                {
                    row2.Add(pair.Key);
                }
                // middle row contains 2 separate rows
                else if(Math.Abs(pair.Value.x - pos.x) <= 2)
                {
                    row2.Add(pair.Key);
                }
            }
            
            // TODO: fix diagonal neighbor generation
            // find diagonal neighbors
            if (pos.y != 3 && pos.x != 3)
            {
                if (Math.Abs(pair.Value.x - pos.x) <= 2 && Math.Abs(pair.Value.y - pos.y) <= 2 && pair.Key != key)
                {
                    row3.Add(pair.Key);
                }
            }
            
            if(row3.Count > 0)
            {
                rows = new List<int>[3];
                rows[2] = row3;
            }

            rows[0] = row1;
            rows[1] = row2;
        }
        return rows;
    }
}