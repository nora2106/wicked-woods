using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//changes cursor icon based on possible interaction
public class CursorIcon : MonoBehaviour
{
    public Sprite inspectIcon; 
    public Sprite moveIcon;
    public Sprite grabIcon;
    public Sprite useIcon;
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = moveIcon;
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null && !EventSystem.current.IsPointerOverGameObject())
        {
            if (hit.collider.gameObject.GetComponent<ObjectInteraction>() || hit.collider.gameObject.tag == "inspect")
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = inspectIcon;
            }
            else if(hit.collider.gameObject.GetComponent<UsableObject>()) {
                gameObject.GetComponent<SpriteRenderer>().sprite = inspectIcon;
            }
            else if (hit.collider.gameObject.GetComponent<InventoryItem>() || hit.collider.gameObject.GetComponent<ItemObject>())
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = grabIcon;
            }
            else
            {
                Reset();
            }
        }
        else if(hit.collider != null && EventSystem.current.IsPointerOverGameObject())
        {
            if (hit.collider.gameObject.GetComponent<InventoryItem>() || hit.collider.gameObject.GetComponent<ItemObject>())
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = grabIcon;
            }
            else
            {
                Reset();
            }
        }
        else
        {
            Reset();
        }
    }

    public void SetUse() {
        gameObject.GetComponent<SpriteRenderer>().sprite = inspectIcon;
    }

    public void SetGrab() {
        gameObject.GetComponent<SpriteRenderer>().sprite = grabIcon;
    }

    public void Reset() {
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
    }
}
