using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockPuzzleSimulation : MonoBehaviour
{
    public List<Clock> clocks = new List<Clock>();
    
    void Start()
    {
        initializeClocks();
        Debug.Log(clocks[2].time);
    }

    void initializeClocks()
    {
        Clock firstClock = new Clock(1, 5.75f);
        clocks.Add(firstClock);
        for (int i = 1; i < 6; i++)
        {
            Clock newClock = new Clock(i);
            clocks.Add(newClock);
        }

        Clock lastClock = new Clock(clocks.Count + 1, 12);
        clocks.Add(lastClock);

        clocks[1].addNeighbors(clocks[2], firstClock);
        clocks[2].addNeighbors(clocks[3], clocks[4]);
        clocks[2].addNeighbors(clocks[3], clocks[4]);
    }
}

public class Clock 
{
    public int id;
    public float time;
    private List<Clock> neighbors = new List<Clock>();
    bool fixedTime = false;

    public Clock(int ID, float initialTime = 0, bool FixedTime = false)
    {
        id = ID;
        fixedTime = FixedTime;
        if(initialTime != 0)
        {
            time = initialTime;
        }
    }

    public void addNeighbors(params Clock[] newNeighbors)
    {
        foreach(Clock neighbor in newNeighbors)
        {
            neighbors.Add(neighbor);
        }
        updateTime();
    }

    public void setTime(float newTime)
    {
        time = newTime;
    }

    public void updateTime()
    {
        neighbors.ForEach(neighbor =>
        {
            time += neighbor.time;
        });
        time = time % 48;
        Debug.Log(time);
    }

    void updateNeighborTime()
    {

    }
}
