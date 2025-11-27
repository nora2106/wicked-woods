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
                if (gm.pendingCommand != null)
                {
                    clickIndex++;
                    if (gm.movementTargetPos != Vector2.zero)
                    {
                        target = gm.movementTargetPos;
                    }
                }
                target.z = transform.position.z;
                if(transform.localScale.x > 0)
                {
                    if (target.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    }
                }
                else if(target.x < transform.position.x)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }

                if (target == transform.position)
                {
                    EndMovement();
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
                if (clickIndex > 1)
                {
                    gm.pendingCommand = null;
                }
                EndMovement();
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

    private void EndMovement()
    {
        isMoving = false;
        if (gm.pendingCommand != null)
        {
            gm.ExecutePendingInteraction();
        }
        clickIndex = 0;
    }
}
    
