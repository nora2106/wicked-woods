using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorIcon : MonoBehaviour
{
    public Sprite inspectIcon; 
    public Sprite moveIcon;
    public Sprite grabIcon;
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
    }

    public void ShowInspect() {
        gameObject.GetComponent<SpriteRenderer>().sprite = inspectIcon;
    }

    public void ShowGrab() {
        gameObject.GetComponent<SpriteRenderer>().sprite = grabIcon;
    }

    public void Reset() {
        gameObject.GetComponent<SpriteRenderer>().sprite = moveIcon;
    }
}
