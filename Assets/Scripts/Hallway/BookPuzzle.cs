using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPuzzle : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        OpenPuzzle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPuzzle()
    {
        player.GetComponent<MoveCharacter>().canMove = false;
    }

    public void ClosePuzzle()
    {
        player.GetComponent<MoveCharacter>().canMove = true;
    }
}
