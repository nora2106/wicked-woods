using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorIcon : MonoBehaviour
{
    public Sprite inspectIcon; 
    public Sprite moveIcon;
    public Sprite grabIcon;
    public Sprite useIcon;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = moveIcon;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            if(hit.collider.gameObject.GetComponent<InspectObject>() || hit.collider.gameObject.tag == "inspect")
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = inspectIcon;
            }
            else if(hit.collider.gameObject.GetComponent<UsableObject>()) {
                gameObject.GetComponent<SpriteRenderer>().sprite = useIcon;
            }
            else if(hit.collider.gameObject.tag == "grab")
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

    public void ShowGrab() {
        gameObject.GetComponent<SpriteRenderer>().sprite = grabIcon;
    }

    public void Reset() {
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
    }
}
