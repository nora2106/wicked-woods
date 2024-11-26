using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//redundant atm
public class SwitchView : MonoBehaviour
{
    public Camera mainCam;
    public Camera secondaryCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam.enabled = true;
        secondaryCam.enabled = false;
    }

    void SwitchCamera()
    {
        mainCam.enabled = true;
        secondaryCam.enabled = false;
    }
}
