using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectObject : MonoBehaviour
{
    public string inspectText;
    protected Text displayText;
    public GameObject viewObject;
    public bool wasInspected;

    // Start is called before the first frame update
    void Start()
    {
        displayText = GameObject.FindWithTag("Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown () {
        if(inspectText != null) {
                displayText.text = inspectText;
                wasInspected = true;
            }
        if(viewObject != null) {
            viewObject.GetComponent<DetailView>().Open();
        }
    }
}
