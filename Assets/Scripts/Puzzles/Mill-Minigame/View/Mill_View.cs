using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PointClickedEventArgs : EventArgs
{
    public int Key { get; }
    public FieldState State { get; }

    public PointClickedEventArgs(int key, FieldState state)
    {
        Key = key;
        State = state;
    }
}

[Serializable]
public struct MillViewConfig
{
    public GameObject pointPrefab;
    public float spacing;
    public GameObject whiteChipPrefab;
    public GameObject blackChipPrefab;
    public Transform playerPos;
    public Transform enemyPos;
    public TextMeshProUGUI displayText;
}

public interface IMillView
{
    Dictionary<int, Vector2> GameBoard { get; }
    event EventHandler<PointClickedEventArgs> OnBoardChanged;
    void InitializeBoard(MillViewConfig config);
    void UpdateField(int key, FieldState state);
    void UpdateBoard(IMillModel model);
    void DisplayText (string text);
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
    private GameObject blackChipPrefab;
    private GameObject whiteChipPrefab;
    private Transform playerStonePos;
    private Transform enemyStonePos;
    private TextMeshProUGUI displayText;
    public event EventHandler<PointClickedEventArgs> OnBoardChanged = (sender, e) => { };

    // update field visually
    public void UpdateField(int key, FieldState state)
    {
        boardPoints[key].SetState(state);
    }

    public void InitializeBoard(MillViewConfig config)
    {
        if (GameBoard.Count == 0)
        {
            return;
        }

        whiteChipPrefab = config.whiteChipPrefab;
        blackChipPrefab = config.blackChipPrefab;
        playerStonePos = config.playerPos;
        enemyStonePos = config.enemyPos;
        displayText = config.displayText;
        boardPoints = new BoardPoint[GameBoard.Count];

        // create board points based on board positions and assign physical position
        for (int i = 0; i < GameBoard.Count; i++)
        {
            var obj = Instantiate(config.pointPrefab, GetWorldPosition(GameBoard[i], config.spacing), Quaternion.identity);
            obj.GetComponent<BoardPoint>().Init(i, this);
            boardPoints[i] = obj.GetComponent<BoardPoint>();
        }

        // create available stones as children of position objects
        for (int i = 0; i < 9; i++)
        {
            Vector3 pos1 = playerStonePos.position + Vector3.down * (i * .7f);
            var obj1 = Instantiate(whiteChipPrefab, pos1, Quaternion.identity);
            obj1.transform.parent = playerStonePos;

            Vector3 pos2 = enemyStonePos.position + Vector3.down * (i * .7f);
            var obj2 = Instantiate(blackChipPrefab, pos2, Quaternion.identity);
            obj2.transform.parent = enemyStonePos;
        }
    }

    public void UpdateBoard(IMillModel model)
    {
        for (int i = 0; i < GameBoard.Count; i++)
        {
            if (boardPoints[i].state != model.GameBoard[i].state)
            {
                UpdateBoardPoint(boardPoints[i], model.GameBoard[i].state);
            }
        }

        if (playerStonePos.childCount > model.AvailableStones[FieldState.Player])
        {
            Destroy(playerStonePos.GetChild(playerStonePos.childCount - 1).gameObject);
        }

        if (enemyStonePos.childCount > model.AvailableStones[FieldState.Enemy])
        {
            Destroy(enemyStonePos.GetChild(enemyStonePos.childCount - 1).gameObject);
        }
    }

    // TODO add stone movement and remove animation
    private void UpdateBoardPoint(BoardPoint field, FieldState newState)
    {
        // create new stone if field is empty
        if (field.state == FieldState.Empty)
        {
            GameObject prefab = whiteChipPrefab;
            if (newState == FieldState.Enemy)
            {
                prefab = blackChipPrefab;
            }

            var obj = Instantiate(prefab, field.transform.position, Quaternion.identity);
            obj.transform.parent = field.transform;
        }
        // remove existing stone
        else if (field.transform.childCount > 0 && newState == FieldState.Empty)
        {
            Destroy(field.transform.GetChild(0).gameObject);
        }
        field.state = newState;
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
        Vector3 origin = new(-boardWidth / 2f, -boardHeight / 2f, 0);
        Vector3 position = new(origin.x + (pos.x * spacing), origin.y + (pos.y * spacing), 0);
        return position;
    }

    public void DisplayText(string text)
    {
        displayText.text = text;
    }
}