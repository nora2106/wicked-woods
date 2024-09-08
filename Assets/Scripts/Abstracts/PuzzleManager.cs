using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleManager : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPuzzle() {
        gameObject.SetActive(true);
        player.GetComponent<MoveCharacter>().canMove = false;
    }

    public void ClosePuzzle() {
        gameObject.SetActive(false);
        player.GetComponent<MoveCharacter>().canMove = true;
    }
}
