using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public float speed;
    public float maxHeight;
    private Vector3 target;
    private new SpriteRenderer renderer;
    public bool canMove = true;

    void Start()
    {
        target = transform.position;
        renderer = GetComponent<SpriteRenderer>();
        var floor = GameObject.Find("Floor");
        //maxHeight = (renderer.bounds.size.y + floor.transform.position.y - 0.2f) * -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.z = transform.position.z;

                if (target.y > maxHeight)
                {
                    target.y = maxHeight;
                }

                if (target.x > transform.position.x)
                {
                    transform.localScale = new Vector2(0.8f, transform.localScale.y);
                }
                else
                {
                    transform.localScale = new Vector2(-0.8f, transform.localScale.y);
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }

        else
        {
            target = transform.position;
        }
    }
}
    
