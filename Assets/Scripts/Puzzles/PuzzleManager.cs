using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PuzzleManager : MonoBehaviour, ISetup
{
    public bool solved;
    public string id;
    public bool resets;
    protected GameManager gm;
    public UnityEvent OnPuzzleSolved;
    public bool CloseAfterSolve;

    public void Setup()
    {
        gm = GameManager.Instance;
        if(gm.save.data.solvedPuzzles.Contains(id)) {
            solved = true;
        }
    }

    //reset puzzle on every activation only if it is checked
    private void OnEnable()
    {
        if(resets)
        {
            ResetPuzzle();
        }
        if(solved)
        {
            DisablePuzzle();
        }
    }

    //reset puzzle to standard
    public abstract void ResetPuzzle();

    public abstract bool CheckIfSolved();

    public void CheckProgress()
    {
        if(CheckIfSolved())
        {
            solved = true;
            if (!gm.save.data.solvedPuzzles.Contains(id))
            {
                gm.save.data.solvedPuzzles.Add(id);
            }
            OnPuzzleSolved.Invoke();
            Success();
        }
    }

    public abstract void Success();

    public abstract void DisablePuzzle();
}
