using UnityEngine;

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
        // TODO implement real selected visual effect
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.8f, 0, 0);
        if(state == FieldState.Selected)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.8f, 0, 0.6f);
        }
    }

    // register click
    void OnMouseDown()
    {
        // notify view about click
        view.HandleBoardInteraction(this);
    }
}