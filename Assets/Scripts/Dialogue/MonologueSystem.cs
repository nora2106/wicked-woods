using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonologueSystem : MonoBehaviour
{
    public Typewriter typewriter;
    public GameObject panel;
    public float resetDuration = 3;
    private string msg;
    
    void Start()
    {
        panel.SetActive(false);
        typewriter = gameObject.GetComponentInChildren<Typewriter>();
    }

    void Update()
    {
        //wait for text to finish 
        if (typewriter.gameObject.GetComponent<Text>().text == msg)
        {
            StartCoroutine("Reset");

        }

        if(msg != "" && Input.GetKeyDown(KeyCode.Return))
        {
            typewriter.Skip();
        }
    }

    private void OnMouseDown()
    {
        typewriter.Skip();
    }

    public void setText(string t)
    {
        if(typewriter == null)
        {
            typewriter = gameObject.GetComponentInChildren<Typewriter>();
        }
        typewriter.Reset();
        StopCoroutine("Reset");
        panel.SetActive(true);
        msg = t;
        typewriter.Write(t);
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(resetDuration);
        typewriter.Reset();
        msg = "";
        panel.SetActive(false);
    }
}
