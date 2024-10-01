using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class MoveCharacter : MonoBehaviour, ISetup
{
    private Vector3 target;
    public bool canMove;
    private NavMeshAgent agent;
    private GameManager gm;

    public void Start()
    {

    }

    public void Setup()
    {
        gm = GameManager.Instance;
        if (gm.gameData != null && gm.gameData.playerPosition != Vector3.zero)
        {
            transform.position = gm.gameData.playerPosition;
        }
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        canMove = true;
    }

    private void Update()
    {
        if(canMove && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.z = transform.position.z;
            }
            agent.SetDestination(target);
            gm.gameData.playerPosition = transform.position;
        }

        else
        {
            target = transform.position;
        }
    }
}
    
