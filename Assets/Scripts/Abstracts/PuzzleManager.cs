using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleManager : MonoBehaviour
{
    public bool solved;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
