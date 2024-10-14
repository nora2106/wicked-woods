using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonologueSystem : MonoBehaviour
{
    public Text text;
    public GameObject panel;
    public float resetDuration = 5;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setText(string t)
    {
        StopCoroutine("Reset");
        ShowMonologue();
        text.text = t;
        //wait for text to finish 
        StartCoroutine("Reset");
    }

    public void ShowMonologue()
    {
        panel.SetActive(true);
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(resetDuration);
        text.text = "";
        panel.SetActive(false);
    }
}
