using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleManager : MonoBehaviour, ISetup
{
    public bool solved;
    public string id;
    private GameManager gm;

    public void Setup()
    {
        gm = GameManager.Instance;
        if(gm.save.data.solvedPuzzles.Contains(id)) {
            solved = true;
            DisablePuzzle();
        }
    }

    public abstract bool CheckIfSolved();

    public void CheckProgress()
    {
        if(CheckIfSolved())
        {
            solved = true;
            gameObject.SetActive(false);
            GameManager.Instance.CloseOverlay();
            if (!gm.save.data.solvedPuzzles.Contains(id))
            {
                gm.save.data.solvedPuzzles.Add(id);
            }
            Success();
        }
    }

    public abstract void Success();

    public abstract void DisablePuzzle();
}
