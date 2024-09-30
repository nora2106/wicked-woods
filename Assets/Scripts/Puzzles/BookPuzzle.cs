using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BookPuzzle : PuzzleManager
{
    public Text displayText;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    public override bool CheckIfSolved() {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Bookslot")) {
            if(go.GetComponent<BookSlots>().correct == false) {
                return false;
            }
        }
        return true;
    }

    public override void Success()
    {
        print("Heureka! DU hast es geschafft!");
    }
}
