using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableBook : MonoBehaviour
{
    public Vector2 pos;
    private bool isDragged;
    private GameObject slot;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isDragged)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
    }

    private void OnMouseDown()
    {
        pos = transform.position;
        isDragged = true;
        transform.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    private void OnMouseUp()
    {
        transform.GetComponent<SpriteRenderer>().sortingOrder = 2;
        isDragged = false;
        if(slot != null)
        {
            Vector2 newPos = slot.transform.position;
            Transform newParent = slot.transform.parent;
            
            slot.transform.SetParent(transform.parent);
            slot.transform.position = pos;
            slot.GetComponent<DraggableBook>().pos = pos;
            
            transform.SetParent(newParent);
            pos = newPos;
        }
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isDragged)
        {
            if (collision.gameObject.GetComponent<DraggableBook>())
            {
                slot = collision.gameObject;
            }
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(isDragged && collision.gameObject == slot)
        {
            slot = null;
        }
    }
}
