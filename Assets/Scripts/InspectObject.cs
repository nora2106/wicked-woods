using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectObject : MonoBehaviour
{
    public string inspectText;
    protected Text displayText;
    private GameObject cursor;
    public bool wasInspected;

    // Start is called before the first frame update
    void Start()
    {
        displayText = GameObject.Find("ActionText").GetComponent<Text>(); 
        cursor = GameObject.Find("CursorIcon");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.name == gameObject.name)
            {
                Inspect();
            }
        }
    }

    private void OnMouseOver()
    {
        cursor.GetComponent<CursorIcon>().ShowInspect();
    }

    private void OnMouseExit()
    {
        cursor.GetComponent<CursorIcon>().Reset();
    }

    public void Inspect() {
        if(inspectText != null) {
            displayText.text = inspectText;
            StartCoroutine(ResetText(2));
            wasInspected = true;
        }
    }
    
    //todo: move resetting text to text component itself
    IEnumerator ResetText(float duration)
    {
        yield return new WaitForSeconds(duration);

        if(displayText.text == inspectText)
        {
            displayText.text = "";
        }
    }
}
