using System;
using System.Collections.Generic;
using UnityEngine;

public class PointClickedEventArgs : EventArgs
{
    public int Key {get;}
    public int State {get;}

    public PointClickedEventArgs(int key, int state)
    {
        Key = key;
        State = state;
    }
}
public interface IMillView
{
    Dictionary<int, BoardPosition> GameBoard { get; set; }
	event EventHandler<PointClickedEventArgs> OnBoardChanged;    
    void InitializeBoard(GameObject pointPrefab, float spacing);
}

public class MillView : MonoBehaviour, IMillView
{
    public Dictionary<int, BoardPosition> gameBoard;
	public event EventHandler<PointClickedEventArgs> OnBoardChanged = (sender, e) =>
    {
        
    };
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

    public void InitializeBoard(GameObject pointPrefab, float spacing)
    {
        GameBoard = gameBoard;

        if(GameBoard.Count == 0)
        {
            return;
        }
        // create board points based on board positions and assign physical position
        for (int i = 0; i < GameBoard.Count; i++)
        {
            var obj = Instantiate(pointPrefab, GetPosition(GameBoard[i].x, GameBoard[i].y, spacing), Quaternion.identity);
            obj.GetComponent<BoardPoint>().Init(GameBoard[i].x, GameBoard[i].y, i, this);
        }
    }

    public void HandleBoardStateChange(int key, int state)
    {
        GameBoard[key].SetState(state);
        var eventArgs = new PointClickedEventArgs(key, state);
        OnBoardChanged(this, eventArgs);
    }

    private Vector3 GetPosition(int x, int y, float spacing)
    {
        int rows = 6;
        int cols = 6;
        float boardWidth = cols * spacing;
        float boardHeight = rows * spacing;
        Vector3 origin = new Vector3(-boardWidth / 2f, -boardHeight / 2f, 0);
        Vector3 position = new Vector3(origin.x + (x * spacing), origin.y + (y * spacing), 0);
        return position;
    }
}