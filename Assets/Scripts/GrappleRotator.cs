using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleRotator : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public GrapplingTool grapple;
    [Header("Appearance")]
    public float rotationSpeed = 5f;
    private Quaternion targetOrientation;
    
    // Update is called once per frame
    void Update()
    {
        if (!grapple.Grappling())
        {
            targetOrientation = transform.parent.rotation;
        }
        else
        {
            targetOrientation = Quaternion.LookRotation(grapple.GetGrapplePoint() - transform.position);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetOrientation, Time.deltaTime * rotationSpeed);
    }
}
