using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 target;
    void Start()
    {
        target = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
        }
        agent.SetDestination(target);
        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
