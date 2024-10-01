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
        if(gm.gameData.solvedPuzzles.Contains(id)) {
            solved = true;
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
            Success();
        }
    }

    public abstract void Success();
}
