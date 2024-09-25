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

    bool CheckSlots() {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Bookslot")) {
            if(go.GetComponent<BookSlots>().correct == false) {
                return false;
            }
        }
        return true;
    }

    public void Success() {
        if(CheckSlots()) {
        displayText.text = "Hallelujah! Du hast gewonnen!";
        }
    }
}
