using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableBook : MonoBehaviour
{
    private Vector2 pos;
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
    }

    private void OnMouseUp()
    {
        //print(slot);
        isDragged = false;
        if(slot != null)
        {
            transform.SetParent(slot.transform);
            pos = transform.parent.position;
        }
        transform.position = pos;
    }

    private void setParent(GameObject obj)
    {
        transform.SetParent(obj.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isDragged)
        {
            if (collision.gameObject.GetComponent<BookSlots>())
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
