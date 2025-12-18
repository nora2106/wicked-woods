using Unity;
using UnityEngine;
using System;

public class BoardPoint : MonoBehaviour {
    public int key;
    public int state = 0;
    public MillView view;

    public void Init(int key, MillView view)
    {
        this.key = key;
        this.view = view;
    }

    public void SetState(int state)
    {
        this.state = state;
        if(state == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    // register click
    void OnMouseDown()
    {
        // notify view about click
        view.HandleBoardInteraction(this);
    }
}