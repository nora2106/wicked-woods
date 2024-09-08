using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSlots : MonoBehaviour
{
    public GameObject correctBook;
    public bool correct;
    public GameObject puzzle;


    // Start is called before the first frame update
    void Start()
    {
        puzzle = transform.parent.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.GetChild(0).gameObject == correctBook)
        {
            puzzle.GetComponent<BookPuzzle>().Success();
            correct = true;
        }
        else
        {
            correct = false;
        }
    }
}
