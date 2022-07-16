using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrappling : MonoBehaviour
{
    private float cooldown = 5f;
    private LineRenderer line;
    private Vector3 target, currentGrapple;
    private SpringJoint joint;
    [Header("Gameplay Settings")]
    public float cooldownTimer = 2f;
    public LayerMask grappleable;
    public Transform grappleTip, camera, player;
    public float maxDistance = 100f;
    [Header("Physics Settings")]
    public float spring = 4.5f;
    public float damper = 7f;
    public float massScale = 4.5f;
    [Header("Misc")]
    public float grappleSpeed = 8f;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        /* Controls - Right Click to fire grapple. Release to untether.
        You can only fire another grapple if the cooldown has been reached.*/
        if (Input.GetMouseButtonDown(1) && cooldown > cooldownTimer)
        {
            // Sanity Check - if previous grapple exists, destroy old grapple.
            if (joint)
                endGrapple();
            Debug.Log("Grapple Fired");
            startGrapple();
            cooldown = 0f;
        }
        else if (Input.GetMouseButtonUp(1) && joint)
        {
            Debug.Log("Grapple destroyed");
            endGrapple();
        }
        cooldown = Mathf.Min(cooldownTimer + 1, cooldown + Time.deltaTime);
    }

    //LateUpdate only execures after all Update() processes for the frame are done
    void LateUpdate()
    {
        rope();
    }

    void startGrapple()
    {
        RaycastHit hitTarget;
        // Checks if 1. raycast hits a valid object and returns the information 
        if (Physics.Raycast(camera.position, camera.forward, out hitTarget, maxDistance, grappleable)) {
            target = hitTarget.point;
            // Creates a connection between the grapple end and our body
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = target;
            // Simulates elasticity of rope
            float lineLength = Vector3.Distance(player.position, target);
            joint.maxDistance = lineLength * 0.6f;
            joint.minDistance = lineLength * 0.0001f;
            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = massScale;
            line.positionCount = 2;
            currentGrapple = grappleTip.position;
        }
        else
        {
            Debug.Log("Grapple Failed");
        }
    }

    void rope()
    {
        // Draws the line so that we can see the grapple
        if (!joint)
            return;
        currentGrapple = Vector3.Lerp(currentGrapple, target, Time.deltaTime * grappleSpeed);
        line.SetPosition(0, grappleTip.position);
        line.SetPosition(1, currentGrapple);
    }

    void endGrapple()
    {
        // Destroys a grapple instance
        line.positionCount = 0;
        Destroy(joint);
    }

    public bool Grappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return target;
    }
}
