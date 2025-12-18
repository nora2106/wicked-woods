using Unity;
using UnityEngine;
using System;

public class BoardPoint : MonoBehaviour {
    public int x;
    public int y;
    public int key;
    public int state = 0;
    public MillView view;

    public void Init(int x, int y, int key, MillView view)
    {
        this.x = x;
        this.y = y;
        this.key = key;
        this.view = view;
    }
    void OnMouseDown()
    {
        // register click
        state = 1;
        view.HandleBoardStateChange(key, state);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}