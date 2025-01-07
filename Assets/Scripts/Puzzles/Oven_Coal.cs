using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class OvenCoal : MonoBehaviour
{
    public int order;
    private GameManager gm;
    private Vector2 pos;
    private bool dragging; 
    public LocalizedString text;
    void Start()
    {
        gm = GameManager.Instance;
        pos = transform.position;
    }

    private void OnMouseDrag()
    {
        if(gm.selectedItemID == "firehook")
        {
            dragging = true;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
        else
        {
            gm.SetText(text.GetLocalizedString());
        }
    }


    private void OnMouseUp()
    {
        if (dragging)
        {
            if(Vector2.Distance(pos, transform.position) < 2)
            {
                dragging = false;
                transform.position = pos;
            }
            else
            {
                dragging = false;
                transform.parent.GetComponent<OvenCoalstack>().CheckOrder(order);
                gameObject.SetActive(false);
            }
        }
    }

    public void ResetCoal()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<Collider2D>().enabled = true;
        transform.position = pos;
    }
}
