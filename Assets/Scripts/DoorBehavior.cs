using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorBehavior : UsableObject
{
    public Sprite newSprite;
    private Text displayText;
    private static System.Timers.Timer aTimer;
    public string clickText = "This door is locked.";

    public void Start()
    {
        displayText = GameObject.Find("ActionText").GetComponent<Text>();
    }

    override public void Action()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        clickText = "Level done!";
        SceneManager.LoadScene("FirstScene", LoadSceneMode.Single);
    }

    override public void ClickAction()
    {
        displayText.text = clickText;
        StartCoroutine(ResetText(2));
    }

    IEnumerator ResetText(float duration)
    {
        yield return new WaitForSeconds(duration);

        if(displayText.text == clickText)
        {
            displayText.text = "";
        }
    }

    IEnumerator LoadNext(float duration)
    {
        yield return new WaitForSeconds(duration);

        SceneManager.LoadScene("FirstScene", LoadSceneMode.Single);
    }

    private static void SetTimer(int time)
    {
        aTimer = new System.Timers.Timer(time);
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }
}
