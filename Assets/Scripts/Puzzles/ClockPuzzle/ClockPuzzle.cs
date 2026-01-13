using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ClockPuzzle : MonoBehaviour
{
    public List<Clock> clocks1 = new List<Clock>();
    public List<Clock> clocks2 = new List<Clock>();
    public List<Clock> clocks3 = new List<Clock>();
    public List<List<Clock>> allClocks;
    
    // only main clocks
    public List<Clock> mainClocks = new List<Clock>();
    System.Random rnd = new System.Random();
    public int startTime;
    public TextMeshProUGUI realTime;
    public int globalTime;

    void Start()
    {
        InitializeClocks();
        globalTime = startTime;
        int hour = globalTime / 60;
        int minutes = globalTime - (hour * 60);
        realTime.text = hour.ToString("00") + ":" + minutes.ToString("00");
        CalcMainClockTimes();
        CalcClockTimes();
    }

    int AddClockTimes(List<Clock> clocks)
    {
        int sum = 0;
        clocks.ForEach(clock =>
        {
            sum += clock.time;
        });
        if(sum == 720)
        {
            return 720;
        }
        return sum % 720;
    }

    private void InitializeClocks()
    {
        allClocks = new List<List<Clock>> { clocks1, clocks2, clocks3 };
        foreach (Transform t in gameObject.transform)
        {
            if (t.gameObject.GetComponent<Clock>())
            {
                Clock clock = t.gameObject.GetComponent<Clock>();
                allClocks[clock.typeID - 1].Add(clock);
                if (clock.isMain)
                {
                    mainClocks.Add(clock);
                }
            }
        }
    }

    private void CalcMainClockTimes()
    {
        int attempts = 0;
        int maxAttempts = 10000;

        while (AddClockTimes(mainClocks) != globalTime && attempts < maxAttempts)
        {
            for (int i = 0; i < mainClocks.Count; i++)
            {
                mainClocks[i].time = GenerateRandomTime();
            }
            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Error: Infinite while loop in CalcMainClockTimes.");
        }
    }

    private void CalcClockTimes()
    {
        for (int i = 0; i < allClocks.Count; i++)
        {
            Clock mainClock = mainClocks.Find(c => c.typeID == allClocks[i][0].typeID);
            if(mainClock == null)
            {
                return;
            }

            int index = allClocks[i].IndexOf(mainClock);

            // change all clocks after main clock
            for (int j = index + 1; j < allClocks[i].Count; j++)
            {
                allClocks[i][j].time = (allClocks[i][j - 1].time + 720 + 60) % 720;
            }

            // change remaining clocks before main clock
            if (index > 0)
            {
                for (int j = index - 1; j >= 0; j--)
                {
                    allClocks[i][j].time = (allClocks[i][j + 1].time + 720 - 60) % 720;
                }
            }
        }
    }

    public void UpdateAllClocks(Clock changedClock)
    {
        List<Clock> currClocks = allClocks[changedClock.typeID - 1];
        int index = currClocks.IndexOf(changedClock);

        // change all clocks after changed clock
        for (int i = index + 1; i < currClocks.Count; i++)
        {
            currClocks[i].time = (currClocks[i - 1].time + 720 + 60) % 720;
        }

        // change remaining clocks before changed clock
        if(index > 0)
        {
            for (int i = index - 1; i >= 0; i--)
            {
                //@todo potentially delay increase to make it more complicated (e.g. 60, 60*2, 60*3,...)
                currClocks[i].time = (currClocks[i + 1].time + 720 + 60) % 720;
            }
        }

        globalTime = AddClockTimes(mainClocks);
        int hour = globalTime / 60;
        int minutes = globalTime - (hour * 60);
        realTime.text = hour.ToString("00") + ":" + minutes.ToString("00");
    }

    // generate a random time in minutes between 15 (00:15) and 720 (24:00)
    public int GenerateRandomTime(int x = 15, int y = 721)
    {
        int randomNum = rnd.Next(x, y);
        if (randomNum % 15 == 0)
        {
            return randomNum;
        }
        return GenerateRandomTime();
    }
}
