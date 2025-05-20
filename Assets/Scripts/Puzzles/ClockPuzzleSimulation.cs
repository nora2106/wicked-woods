using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class ClockPuzzleSimulation : MonoBehaviour
{
    public List<Clock> clocks = new List<Clock>();
    public List<Clock> addedClocks = new List<Clock>();
    System.Random rnd = new System.Random();

    void Start()
    {
        GenerateClocks();
    }

    // generate a random time in minutes between 15 (00:15) and 720 (24:00)
    public int generateRandomTime()
    {
        int randomNum = rnd.Next(15, 720);
        if (randomNum % 15 == 0)
        {
            return randomNum;
        }
        return generateRandomTime();
    }

    void GenerateClocks()
    {
        Clock firstClock = new Clock(1, this, 720);
        clocks.Add(firstClock);
        Clock secondClock = new Clock(2, this, generateRandomTime());
        clocks.Add(secondClock);
        firstClock.addNeighbor(secondClock);
        firstClock.FillSumWithNewNeighbor();
        //Debug.Log(firstClock.checkIfTimeWorks());

        while (!allClocksCorrect())
        {
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
        for (int i = 0; i < 20; i++)
        {
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
        clocks.ForEach(clock =>
        {
            Debug.Log(clock.time);
        });
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
        return clocksChecked;
    }
}

public class Clock 
{
    public int id;
    // clock time in minutes
    public int time;
    public List<Clock> neighbors = new List<Clock>();
    ClockPuzzleSimulation simulation;
    System.Random rnd = new System.Random();

    public Clock(int ID, ClockPuzzleSimulation parentSim, int initialTime = 0)
    {
        id = ID;
        simulation = parentSim;
        if(initialTime != 0)
        {
            time = initialTime;
        }
    }

    // add clock as neighbor
    public void addNeighbor(Clock newNeighbor)
    {
        neighbors.Add(newNeighbor);
        simulation.clocks.Add(newNeighbor);
    }

    // add new neighbor with a time that completes the sum
    public void FillSumWithNewNeighbor()
    {
        int neighborSum(int i)
        {
            int sum = i;
            neighbors.ForEach(n =>
            {
                sum += n.time;
            });
            return sum;
        }

        int generateNewTime()
        {
            int newTime = simulation.generateRandomTime();

            if (neighborSum(newTime) == time || neighborSum(newTime) % 720 == time % 720)
            {
                return newTime;
            }
            return generateNewTime();
        }

        int requiredTime = generateNewTime();

        if (requiredTime == 345)
        {
            Debug.Log("success!");
            time = time - generateNewTime();
            // connect the clocks via the neighbor system
            return;
        }
        Clock neighbor = new Clock(rnd.Next(0, 100), simulation);
        neighbor.time = requiredTime;
        neighbors.Add(neighbor);
        simulation.addedClocks.Add(neighbor);
        return;
    }

    // check if the sum of all neighbors results in the current time
    public bool checkIfTimeWorks()
    {
        int timeSum = 0;
        neighbors.ForEach(n =>
        {
            timeSum += n.time;
        });
        return timeSum % 720 == time % 720;
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
