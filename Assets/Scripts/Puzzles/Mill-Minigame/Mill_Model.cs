using System;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public interface IMillModel
{
    Dictionary<int, BoardNode> GameBoard { get; set; }
    void InitializeBoard();
    void UpdateField(int key, int state);
    bool IsNeighbor(int key1, int key2);
    List<int> GetFieldsByState(int playerID);
    bool CheckForMills(int key, int currentState);
}

public class BoardNode
{
    // (0 for empty (default), 1 for player 1 (white), 2 for player 2 (black))
    public int state = 0;
    public List<int> neighbors;

    public BoardNode(List<int> neighbors)
    {
        this.neighbors = neighbors;
    }

    public void SetState(int state)
    {
        this.state = state;
    }
}

public class MillModel : IMillModel
{
    // Dictionary containing BoardNode (with state) and its key/ID
    public Dictionary<int, BoardNode> gameBoard = new Dictionary<int, BoardNode>();    
    private Dictionary<int, List<int[]>> millsByNode;
    public Dictionary<int, BoardNode> GameBoard
    {
        get { return gameBoard; }
        set
        {
            gameBoard = value;
        }
    }
    private List<int>[] neighborNodes;

    // all possible mills, each containing 3 node IDs
    static readonly int[][] mills =
    {
        new[] {0, 1, 2},
        new[] {3, 4, 5},
        new[] {6, 7, 8},
        new[] {9, 10, 11},
        new[] {12, 13, 14},
        new[] {15, 16, 17},
        new[] {18, 19, 20},
        new[] {21, 22, 23},
        new[] {21, 9, 0},
        new[] {18, 10, 3},
        new[] {15, 11, 6},
        new[] {22, 19, 16},
        new[] {7, 4, 1},
        new[] {17, 12, 8},
        new[] {20, 13, 5},
        new[] {23, 14, 2},
    };

    // update field and check for any mills surrounding the updated field
    public void UpdateField(int key, int state)
    {
        GameBoard[key].SetState(state);
        CheckForMills(key, state);
    }

    // create gameboard model
    public void InitializeBoard()
    {
        // assign neighbor nodes to each node (only direct neighbors to model movement paths)
        neighborNodes = new List<int>[24];
        neighborNodes[0] = new List<int> {1, 9};
        neighborNodes[1] = new List<int> {0, 2, 4};
        neighborNodes[2] = new List<int> {1, 14};
        neighborNodes[3] = new List<int> {10, 4};
        neighborNodes[4] = new List<int> {3, 7, 1, 5};
        neighborNodes[5] = new List<int> {4, 13};
        neighborNodes[6] = new List<int> {11, 7};
        neighborNodes[7] = new List<int> {6, 4, 8};
        neighborNodes[8] = new List<int> {7, 12};
        neighborNodes[9] = new List<int> {21, 0, 10};
        neighborNodes[10] = new List<int> {9, 11, 18, 3};
        neighborNodes[11] = new List<int> {10, 15, 6};
        neighborNodes[12] = new List<int> {17, 8, 13};
        neighborNodes[13] = new List<int> {12, 20, 5, 14};
        neighborNodes[14] = new List<int> {23, 13, 2};
        neighborNodes[15] = new List<int> {11, 16};
        neighborNodes[16] = new List<int> {15, 19, 17};
        neighborNodes[17] = new List<int> {16, 12};
        neighborNodes[18] = new List<int> {20, 29};
        neighborNodes[19] = new List<int> {18, 22, 20, 16};
        neighborNodes[20] = new List<int> {19, 13};
        neighborNodes[21] = new List<int> {9, 22};
        neighborNodes[22] = new List<int> {21, 19, 23};
        neighborNodes[23] = new List<int> {22, 14};

        // lookup table containing a node and its possible mills
        millsByNode = new Dictionary<int, List<int[]>>();
        foreach(var mill in mills)
        {
            foreach (var node in mill) {
                if(!millsByNode.TryGetValue(node, out var list))
                {
                    list = new List<int[]>();
                    millsByNode[node] = list;
                }
                list.Add(mill);
            }
        }

        // fill board with nodes
        for (int i = 0; i < 24; i++)
        {
            GameBoard.Add(i, new BoardNode(neighborNodes[i]));
        }
    }

    public bool CheckForMills(int key, int currentState)
    {
        if(currentState == 0)
        {
            return false;
        }
        // get all possible mills
        var possibleMills = millsByNode[key];

        foreach(var mill in possibleMills)
        {
            if(IsMill(mill))
            {
                Debug.Log("mill formed");
                return true;
                // TODO notify controller
                // controller sends mill to view
            }
        }
        return false;

        // check if all nodes of a mill have the same state
        bool IsMill(int[] mill)
        {
            foreach(int node in mill)
            {
                if(GameBoard[node].state != currentState)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public bool IsNeighbor(int key1, int key2)
    {
        foreach(var node in neighborNodes[key1])
        {
            if(node == key2)
            {
                return true;
            }
        }
        return false;
    }

    public List<int> GetFieldsByState(int state)
    {
        List<int> list = new List<int>();
        foreach(var point in GameBoard)
        {
            if(point.Value.state == state)
            {
                list.Add(point.Key);
            }
        }
        return list;
    }
}