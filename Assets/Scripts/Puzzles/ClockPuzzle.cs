using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ClockPuzzle : MonoBehaviour
{
    public GameObject clockPrefab;
    // all clocks per type, including main clocks
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
            Debug.LogWarning("Error: Infinite while loop.");
        }
    }

    private void CalcClockTimes()
    {
        int attempts = 0;
        int maxAttempts = 10000;

        for (int i = 0; i < allClocks.Count; i++)
        {
            while (allClocks[i].Any(c => c.time == 0) && attempts < maxAttempts)
            {
                for (int j = 0; j < allClocks[i].Count; j++)
                {
                    if(allClocks[i][j].time == 0)
                    {
                        int newTime = 0;
                        if (j < (allClocks[i].Count - 1) && allClocks[i][j + 1].time != 0)
                        {
                            newTime = (allClocks[i][j + 1].time + 720 + 60) % 720;
                        }

                        else if (j > 0 && allClocks[i][j - 1].time != 0)
                        {
                            newTime = (allClocks[i][j - 1].time + 720 + 60) % 720;
                        }
                        allClocks[i][j].time = newTime;
                    }
                }
                attempts++;
            }
            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Error: Infinite while loop.");
            }
        }
    }

    public void UpdateAllClocks(Clock changedClock)
    {
        List<Clock> currClocks = allClocks[changedClock.typeID - 1];
        for (int i = 0; i < currClocks.Count; i++)
        {
            if(i < (currClocks.Count - 1) && currClocks[i].time != (currClocks[i + 1].time + 720 + 60) % 720)
            {
                currClocks[i].time = (currClocks[i + 1].time + 720 + 60) % 720;
            }
            else if (i > 0 && currClocks[i].time != (currClocks[i - 1].time + 720 + 60) % 720)
            {
                currClocks[i].time = (currClocks[i - 1].time + 720 + 60) % 720;
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
