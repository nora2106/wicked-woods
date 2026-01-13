using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPlank : MonoBehaviour
{
    public bool complete;
    public GameObject priorPlank;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckScrews())
        {
            complete = true;
            transform.parent.gameObject.GetComponent<PuzzleManager>().CheckProgress();
            Destroy(gameObject);
            //add animation
        }
    }
    bool CheckScrews()
    {
        if(priorPlank == null)
        {
            foreach (Transform t in transform)
            {
                if (t.gameObject.GetComponent<RotateScrew>().correct == false)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}
