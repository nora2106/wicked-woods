using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;
using UnityEngine.UIElements;

public interface IMillModel
{
    // Dictionary containing the BoardValue (position on board) and the state 
    // (0 for empty, 1 for player 1 (white), 2 for player 2 (black))
    Dictionary<int, BoardPosition> GameBoard { get; set; }
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

    public Dictionary<int, BoardPosition> GameBoard
    {
        get { return GameBoard; }
        set
        {
            GameBoard = new Dictionary<int, BoardPosition>();
            for(int i = 0; i < positions.Length; i++)
            {
                GameBoard.Add(i, positions[i]);
            }
        }
    }

    public int[][] GetNeighborIDs(BoardPosition pos, Dictionary<int, BoardPosition> board)
    {
        // middle points
        // 2 possible mills (horizontal, vertical)
        int[][] rows = new int[2][];
        // edge points
        // 3 possible mills (horizontal. vertical, diagonal)
        if(pos.y != 3  || pos.x != 3)
        {
            rows = new int[3][];
        }
        
        foreach(KeyValuePair<int, BoardPosition> pair in board)
            {
                // possible mill 1
                int count1 = 0;
                // find vertical neighbors
                // required: same x coordinate + difference between y coords not > 1
                if(pair.Value.x == pos.x && Math.Abs(pair.Value.y - pos.y) <= 1)
                {
                    rows[1][count1] = pair.Key;
                    count1++;
                }

                // possible mill 2
                int count2 = 0;
                // find horizontal neighbors
                // required: same y coordinate + difference between x coords not > 1
                if(pair.Value.y == pos.y && Math.Abs(pair.Value.x - pos.x) <= 1)
                {
                    rows[2][count2] = pair.Key;
                    count2++;
                }

                if(pos.y != 3  || pos.x != 3)
                {
                    // possible mill 3
                    int count3 = 0;
                    // find diagonal neighbors
                    if(pair.Value.y == pos.y && pair.Value.x == pos.x && Math.Abs(pair.Value.x - pos.x) <= 1)
                    {
                        rows[3][count3] = pair.Key;
                        count3++;
                    }
                }
        }
        return rows;
    }
}