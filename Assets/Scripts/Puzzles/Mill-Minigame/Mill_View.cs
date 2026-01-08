using System;
using System.Collections.Generic;
using UnityEngine;

public class PointClickedEventArgs : EventArgs
{
    public int Key {get;}
    public FieldState State {get;}

    public PointClickedEventArgs(int key, FieldState state)
    {
        Key = key;
        State = state;
    }
}

public interface IMillView
{
    Dictionary<int, Vector2> GameBoard { get;}
	event EventHandler<PointClickedEventArgs> OnBoardChanged;    
    void InitializeBoard(GameObject pointPrefab, float spacing);
    void UpdateField(int key, FieldState state);
    void UpdateBoard(Dictionary<int, BoardNode> board);
}

public class MillView : MonoBehaviour, IMillView
{
    // create view gameboard containing ID and position
    // starting at bottom left: from left to right, row for row ending at top right
    public Dictionary<int, Vector2> GameBoard => new()
    {
        {0, new Vector2(0, 0)},
        {1, new Vector2(0, 3)},
        {2, new Vector2(0, 6)},
        {3, new Vector2(1, 1)},
        {4, new Vector2(1, 3)},
        {5, new Vector2(1, 5)},
        {6, new Vector2(2, 2)},
        {7, new Vector2(2, 3)},
        {8, new Vector2(2, 4)},
        {9, new Vector2(3, 0)},
        {10, new Vector2(3, 1)},
        {11, new Vector2(3, 2)},
        {12, new Vector2(3, 4)},
        {13, new Vector2(3, 5)},
        {14, new Vector2(3, 6)},
        {15, new Vector2(4, 2)},
        {16, new Vector2(4, 3)},
        {17, new Vector2(4, 4)},
        {18, new Vector2(5, 1)},
        {19, new Vector2(5, 3)},
        {20, new Vector2(5, 5)},
        {21, new Vector2(6, 0)},
        {22, new Vector2(6, 3)},
        {23, new Vector2(6, 6)},
    };
    public BoardPoint[] boardPoints;
	public event EventHandler<PointClickedEventArgs> OnBoardChanged = (sender, e) => {};
    public string gamemode;
    // game modes: setup phase, move phase, select
    public string Gamemode{get{return gamemode;}set{Gamemode = value;}}
    public BoardPoint selectedPoint;

    // update field visually
    public void UpdateField(int key, FieldState state)
    {
        boardPoints[key].SetState(state);
    }

    public void InitializeBoard(GameObject pointPrefab, float spacing)
    {
        if(GameBoard.Count == 0)
        {
            return;
        }
        boardPoints = new BoardPoint[GameBoard.Count];
        // create board points based on board positions and assign physical position
        for (int i = 0; i < GameBoard.Count; i++)
        {
            var obj = Instantiate(pointPrefab, GetWorldPosition(GameBoard[i], spacing), Quaternion.identity);
            obj.GetComponent<BoardPoint>().Init(i, this);
            boardPoints[i] = obj.GetComponent<BoardPoint>();
        }
    }

    public void UpdateBoard(Dictionary<int, BoardNode> board)
    {
        for (int i = 0; i < GameBoard.Count; i++)
        {
            boardPoints[i].SetState(board[i].state);
        }
    }

    public void HandleBoardInteraction(BoardPoint sender)
    {
        // notify controller about click
        var eventArgs = new PointClickedEventArgs(sender.key, sender.state);
        OnBoardChanged(this, eventArgs);
    }

    private Vector3 GetWorldPosition(Vector2 pos, float spacing)
    {
        int rows = 6;
        int cols = 6;
        float boardWidth = cols * spacing;
        float boardHeight = rows * spacing;
        Vector3 origin = new Vector3(-boardWidth / 2f, -boardHeight / 2f, 0);
        Vector3 position = new Vector3(origin.x + (pos.x * spacing), origin.y + (pos.y * spacing), 0);
        return position;
    }
}