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
        canMove = false;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        gm = GameManager.Instance;
        SetPosition(gm.save.data.playerPosition);
    }

    public void Setup()
    {
       
    }

    private void Update()
    {
        if (canMove && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.z = transform.position.z;
            }
            agent.SetDestination(target);
            gm.save.data.playerPosition = transform.position;
        }

        else
        {
            target = transform.position;
        }
    }

    public void SetPosition(Vector2 pos)
    {
        agent.enabled = false;
        gameObject.transform.position = pos;
        target = pos;
        agent.enabled = true;
        canMove = true;
    }
}
    
