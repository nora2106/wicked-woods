using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Clock : MonoBehaviour
{
    public int id;
    // clock time in minutes
    public int time;
    public List<Clock> neighbors = new List<Clock>();
    ClockPuzzleSimulation simulation;
    System.Random rnd = new System.Random();
    TextMeshPro text;
    public LineRenderer lineRenderer;

    void Start()
    {
        text = gameObject.GetComponent<TextMeshPro>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    private void Update()
    {
        text.text = time.ToString();
    }

    private void OnMouseDown()
    {
        text.faceColor = Color.red;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        neighbors.ForEach(n =>
        {
            n.text.faceColor = Color.red;
        });
    }

    public Clock(int ID, ClockPuzzleSimulation parentSim, int initialTime = 0)
    {
        id = ID;
        simulation = parentSim;
        if (initialTime != 0)
        {
            time = initialTime;
        }
    }

    public void Setup(ClockPuzzleSimulation parentSim, int initialTime = 0)
    {
        simulation = parentSim;
        if (initialTime != 0)
        {
            time = initialTime;
        }
        lineRenderer.positionCount = 1;

        lineRenderer.SetPosition(0, transform.position);
    }

    // add clock as neighbor
    public void addNeighbor(Clock newNeighbor)
    {
        neighbors.Add(newNeighbor);
        UpdateLine();
    }

    public void UpdateLine()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, neighbors[0].transform.position);
        lineRenderer.SetPosition(1, transform.position);

        //if (lineRenderer.positionCount > 3)
        //{
        //    lineRenderer.SetPosition(0, neighbors[1].transform.position);
        //    lineRenderer.SetPosition(1, transform.position);
        //    lineRenderer.SetPosition(2, neighbors[2].transform.position);
        //}
    }

    // add new neighbor with a time that completes the sum
    public void FillSumWithNewNeighbor()
    {
        if(neighbors.Count == 0)
        {
            Debug.Log(time + " has no neighbors");
            return;
        }

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

        if (simulation.addedClocks.Any(c => c.time == requiredTime) && simulation.loops > 5)
        {
            Debug.Log(time + " requires " + requiredTime);
            //simulation.AddClock(this, requiredTime);
            //simulation.addedClocks.ForEach(clock => simulation.clocks.Add(clock));
            //simulation.reachedGoal = true;
            //return;
        }

        if (requiredTime == time)
        {
            // add 2 clocks instead of 1
            void calcTime()
            {
                int time1 = simulation.generateRandomTime(15, (requiredTime + 1));
                int time2 = requiredTime - time1;
                if(time2 <= 0 || time1 <= 0)
                {
                    calcTime();
                    return;
                }
                simulation.AddClock(this, time1);
                simulation.AddClock(this, time2);
            }
            calcTime();
            return;

        }
        simulation.AddClock(this, requiredTime);
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
}
