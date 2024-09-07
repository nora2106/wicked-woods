using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSlots : MonoBehaviour
{
    public GameObject currentBook;
    public GameObject correctBook;
    public bool correct;

    // Start is called before the first frame update
    void Start()
    {
        currentBook = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount > 1)
        {
            foreach (Transform child in transform.parent)
            {
                if (child.childCount == 0)
                {
                    currentBook.transform.SetParent(child);
                    currentBook.transform.position = child.transform.position;
                    currentBook = gameObject.transform.GetChild(0).gameObject;
                }
            }
        }

        if (currentBook == correctBook)
        {
            correct = true;
        }
        else
        {
            correct = false;
        }
    }
}
