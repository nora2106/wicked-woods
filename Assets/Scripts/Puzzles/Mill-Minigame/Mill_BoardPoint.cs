using Unity;
using UnityEngine;
using System;
using UnityEditorInternal;

public enum FieldState
{
    Empty,
    Player,
    Enemy,
    Selected
}

public class BoardPoint : MonoBehaviour {
    public int key;
    public FieldState state = FieldState.Empty;
    public MillView view;

    public void Init(int key, MillView view)
    {
        this.key = key;
        this.view = view;
    }

    public void SetState(FieldState state)
    {
        this.state = state;
        switch(state)
        {
            // empty field
            case FieldState.Empty:
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                break;
            // player stone    
            case FieldState.Player:
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                break;
            // enemy stone
            case FieldState.Enemy:
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f);
                break;
            // selected player stone
            case FieldState.Selected:
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0.4f, 0.4f, 0.4f, 1f);
                break;
        }
    }

    // register click
    void OnMouseDown()
    {
        // notify view about click
        view.HandleBoardInteraction(this);
    }
}