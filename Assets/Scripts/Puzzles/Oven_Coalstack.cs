using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenCoalstack : MonoBehaviour
{
    public List<GameObject> coals;
    private int currentOrder = 0;
    public OvenPuzzle puzzle;
    private int initialCount;

    void Start()
    {
        puzzle = gameObject.GetComponent<OvenPuzzle>();
        if(!puzzle.solved)
        {
            foreach (Transform t in transform)
            {
                coals.Add(t.gameObject);
            }
            initialCount = coals.Count;
        }
    }

    public void ResetToStart()
    {
        if (coals.Count > 0)
        {
            coals.ForEach(coal =>
            {
                coal.GetComponent<OvenCoal>().ResetCoal();
            });
            currentOrder = 0;
        }
    }

    public void CheckOrder(int designatedOrder)
    {
        currentOrder++;
        if(currentOrder != designatedOrder)
        {
            Lose();
        }
        if(currentOrder == initialCount)
        {
            puzzle.solved = true;
        }
        gameObject.GetComponent<OvenPuzzle>().CheckProgress();
    }

    public void Lose()
    {
        print("lose");
        // go up in flames

        // deactivate coal moving
        coals.ForEach(coal =>
        {
            coal.GetComponent<Collider2D>().enabled = false;
        });
    }
}
