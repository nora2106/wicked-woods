using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ClockPuzzleSimulation : MonoBehaviour
{
    public GameObject clockPrefab;
    public List<Clock> clocks = new List<Clock>();
    public List<Clock> addedClocks = new List<Clock>();
    System.Random rnd = new System.Random();
    bool first = true;
    public bool reachedGoal = false;
    public int loops = 0;

    void Start()
    {
        //GenerateClocks();
        Debug.Log(AddTimes(450, 450));
    }

    int AddTimes(int time1, int time2, int time3 = 0)
    {
        return (time1 + time2 + time3) % 720;
    }

    // generate a random time in minutes between 15 (00:15) and 720 (24:00)
    public int generateRandomTime(int x = 15, int y = 721)
    {
        int randomNum = rnd.Next(x, y);
        if (randomNum % 15 == 0)
        {
            return randomNum;
        }
        return generateRandomTime();
    }

    void GenerateClocks()
    {
        GameObject clockObject = Instantiate(clockPrefab, transform.position, Quaternion.identity);
        Clock firstClock = clockObject.GetComponent<Clock>();
        firstClock.Setup(this, 720);
        clocks.Add(firstClock);

        GameObject clockObjec2t = Instantiate(clockPrefab, new Vector3(transform.position.x - 3 , transform.position.y, 0), Quaternion.identity);
        Clock clock2 = clockObjec2t.GetComponent<Clock>();
        clock2.Setup(this, 450);
        clocks.Add(clock2);
        firstClock.addNeighbor(clock2);
        clock2.addNeighbor(firstClock);
        
        firstClock.FillSumWithNewNeighbor();
        clock2.FillSumWithNewNeighbor();

        while (!reachedGoal && loops < 20)
        {
            loops += 1;
            clocks.ForEach(clock =>
            {
                if (!clock.checkIfTimeWorks())
                {
                    clock.FillSumWithNewNeighbor();
                }
            });
            addedClocks.ForEach(clock => clocks.Add(clock));
            addedClocks = new List<Clock>();
        }
    }

    public void AddClock(Clock baseClock, int time = 0)
    {
        Vector3 basePos = baseClock.gameObject.transform.position;
        float newX = basePos.x + 0.5f;
        float newY = basePos.y + 0.5f;
        if (first)
        {
            newX = basePos.x - 0.5f;
        }
        first = !first;
        
        GameObject clockObject = Instantiate(clockPrefab, new Vector3(newX, newY), Quaternion.identity);
        Clock newClock = clockObject.GetComponent<Clock>();
        newClock.Setup(this, time);
        newClock.addNeighbor(baseClock);
        baseClock.addNeighbor(newClock);
        addedClocks.Add(newClock);
    }

    private bool allClocksCorrect()
    {
        bool clocksChecked = true;
        clocks.ForEach(clock =>
        {
            if (!clock.checkIfTimeWorks())
            {
                clocksChecked = false;
            }
        });
        addedClocks.ForEach(clock =>
        {
            if (!clock.checkIfTimeWorks())
            {
                clocksChecked = false;
            }
        });
        return clocksChecked;
    }
}
