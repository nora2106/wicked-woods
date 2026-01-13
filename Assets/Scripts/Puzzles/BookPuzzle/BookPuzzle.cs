using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BookPuzzle : PuzzleManager, ISetup
{
    public override bool CheckIfSolved() {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Bookslot")) {
            if(go.GetComponent<BookSlots>().correct == false) {
                return false;
            }
        }
        return true;
    }

    public override void ResetPuzzle()
    {
    }

    public override void Success()
    {
        print("Heureka! DU hast es geschafft!");
    }

    public override void DisablePuzzle()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Bookslot"))
        {
            go.GetComponent<BookSlots>().correctBook.transform.parent = go.transform;
        }
    }
}
