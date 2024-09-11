using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveCharacter : MonoBehaviour
{
    private Vector3 target;
    public bool canMove = true;
    private NavMeshAgent agent;

    void Start()
    {
        target = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if(canMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.z = transform.position.z;
            }
            agent.SetDestination(target);
        }

        else
        {
            target = transform.position;
        }
    }
}
    
