using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScrew : UsableObject
{
    public float correctRotation;
    public bool correct = false;
    private float initialRotation;
    private float diff;
    public override void Action()
    {
        transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);
    }

    private void OnMouseDown()
    {
        Action();
    }

    public override void OpenAnimation()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!correct)
        {
            if (initialRotation < transform.eulerAngles.z)
            {
                diff = 360 - (Mathf.Abs(transform.eulerAngles.z - initialRotation));
            }
            else
            {
                diff = transform.eulerAngles.z - initialRotation;
            }
            if (Mathf.Round(Mathf.Abs(diff)) == correctRotation)
            {
                correct = true;
            }
            else
            {
                correct = false;
            }
        }
        
    }

    new private void Start()
    {
        base.Start();
        initialRotation = transform.eulerAngles.z;
    }
}
