using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectObject : MonoBehaviour
{
    public string inspectText;
    private GameManager gm;
    public GameObject viewObject;
    public bool wasInspected;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown () {
        if(inspectText != null) {
            gm.SetText(inspectText);
                wasInspected = true;
            }
        if(viewObject != null) {
            viewObject.GetComponent<DetailView>().Open();
        }
    }
}
