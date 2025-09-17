using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    // clock time in minutes
    public int time;
    // clock color type (1-3)
    public int typeID;
    // defines if clock can be edited
    public bool editable = false;
    public bool isMain = false;
    TextMeshProUGUI text;

    void Start()
    {
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        if(isMain)
        {
            text.color = Color.red;
        }
    }

    private void Update()
    {
        text.text = formatTime(time);
    }

    private void OnMouseDown()
    {
        if(editable)
        {
            time += 15;
            if (time != 720)
            {
                time = time % 720;
            }

            gameObject.GetComponentInParent<ClockPuzzle>().UpdateAllClocks(this);
        }
    }

    public string formatTime(int time)
    {
        int hour = time / 60;
        int minutes = time - (hour * 60);
        string timeString = hour.ToString("00") + ":" + minutes.ToString("00");
        return timeString;
    }
}
