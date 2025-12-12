using Unity;
using UnityEngine;

public class BoardPoint : MonoBehaviour {
    public int x;
    public int y;
    public int key;
    public int state;

    public BoardPoint(int x, int y, int key)
    {
        this.x = x;
        this.y = y;
        this.key = key;
        state = 0;
    }

    // set gameobject position according to pos values
    public void SetGOPosition()
    {
    }

    void OnMouseDown()
    {
        // register click
    }
}