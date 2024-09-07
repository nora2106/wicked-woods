using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetText : MonoBehaviour
{
    private string currText;
    private bool started;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<Text>().text != "" && started == false)
        {
            StartCoroutine(Reset(4));
        }


    }

    IEnumerator Reset(float duration)
    {
        started = true;
        currText = gameObject.GetComponent<Text>().text;
        
        yield return new WaitForSeconds(duration);
        
        if(gameObject.GetComponent<Text>().text == currText)
        {
            gameObject.GetComponent<Text>().text = "";
        }
        started = false;
    }
}
