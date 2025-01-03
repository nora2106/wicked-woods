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
    public bool isMoving;
    public GameObject interactionObject;
    private int clickIndex = 0;

    public void Start()
    {
        canMove = false;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        gm = GameManager.Instance;
        SetPosition(new Vector2(0, 0));
        //SetPosition(gm.save.data.playerPosition);
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
                if (interactionObject)
                {
                    clickIndex++;
                }
            }
            agent.SetDestination(target);
            gm.save.data.playerPosition = transform.position;
            if (agent.velocity != Vector3.zero && !isMoving)
            {
                isMoving = true;
            }
            else if (agent.velocity == Vector3.zero && isMoving)
            {
                isMoving = false;
                if(clickIndex > 1)
                {
                    interactionObject = null;
                }

                if(interactionObject)
                {
                    // TODO consider other objects without openobject (extra script or condition)
                    interactionObject.GetComponent<ActionAfterMovement>().ActionAfterMovement();
                    interactionObject = null;
                }
                clickIndex = 0;
            }
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
    
