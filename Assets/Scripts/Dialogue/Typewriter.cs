using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{
    public Text text;
    public string currMsg;
    public bool active;
    [SerializeField] private float timePerChar = 0.05f;

    void Awake()
    {
        text = GetComponent<Text>();
        text.text = "";
    }

    private void Update()
    {
        if(currMsg != "")
        {
            if(currMsg == text.text)
            {
                active = false;
            }
        }
    }

    public void Write(string msg)
    {
        text.text = "";
        currMsg = msg;
        StartCoroutine("PlayText");
        active = true;
    }

    public void Skip()
    {
        StopCoroutine("PlayText");
        text.text = currMsg;
        active = false;
    }

    public void Reset()
    {
        StopCoroutine("PlayText");
        text.text = "";
        currMsg = "";
        active = false;
    }

    IEnumerator PlayText()
    {
        active = true;
        foreach (char c in currMsg)
        {
            text.text += c;
            yield return new WaitForSeconds(timePerChar);
        }
        active = false;
    }
}
