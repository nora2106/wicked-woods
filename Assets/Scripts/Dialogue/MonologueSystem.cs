using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonologueSystem : MonoBehaviour
{
    public Typewriter typewriter;
    public GameObject panel;
    public float resetDuration = 5;
    private string msg;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);

        typewriter = gameObject.GetComponentInChildren<Typewriter>();
    }

    // Update is called once per frame
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
        StopCoroutine("Reset");
        ShowMonologue();
        msg = t;
        typewriter.Write(t);
    }

    public void ShowMonologue()
    {
        panel.SetActive(true);
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(resetDuration);
        typewriter.Reset();
        msg = "";
        panel.SetActive(false);
    }
}
