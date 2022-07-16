using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Follows DanisTutorials' grappling gun tutorial, but deviates heavily on the grapple projectile implementation.
public class GrapplingTool : MonoBehaviour
{
    private float cooldown = 5f;
    private LineRenderer line;
    private Vector3 target, currentGrapple;
    private SpringJoint joint;
    private GameObject grapple;
    public GameObject GrappleProjectile;
    [Header("Gameplay Settings")]
    public float cooldownTimer = 2f;
    public float speed = 50f;
    public Transform grappleTip, player;
    [Header("Physics Settings")]
    public float spring = 4.5f;
    public float damper = 4f;
    public float massScale = 4.5f;

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
            if (grapple)
            {
                Destroy(grapple);
                destroyRope();
            }
            shootGrapple();
            cooldown = 0f;
        }
        else if (!Input.GetMouseButton(1))
        {
            if (joint)
                endGrapple();
            if (grapple)
            {
                Destroy(grapple);
                destroyRope();
            }
        }
        cooldown = Mathf.Min(cooldownTimer + 1, cooldown + Time.deltaTime);
    }

    //LateUpdate only executes after all Update() processes for the frame are done
    void LateUpdate()
    {
        rope();
    }

    void shootGrapple()
    {
        grapple = Instantiate(GrappleProjectile, grappleTip.position, transform.rotation);
        grapple.GetComponentInChildren<Rigidbody>().velocity = player.GetComponent<Rigidbody>().velocity + transform.forward * speed;
        grapple.GetComponentInChildren<GrappleProjectile>().setPlayer(player.GetComponent<Rigidbody>());
        line.positionCount = 2;
    }

    public void startGrapple(Vector3 hitTarget)
    {
        target = hitTarget;
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

    void rope()
    {
        // Draws the line so that we can see the grapple
        if (!joint)
            return;
        line.SetPosition(0, grappleTip.position);
        line.SetPosition(1, target);
    }

    public void rope(Vector3 bulletpos)
    {
        line.SetPosition(0, grappleTip.position);
        line.SetPosition(1, bulletpos);
    }

    public void destroyRope()
    {
        line.positionCount = 0;
    }

    public void endGrapple()
    {
        // Destroys a grapple instance
        line.positionCount = 0;
        if (joint)
            Destroy(joint);
    }

    public bool Grappling()
    {
        return joint != null;
    }

    public float grapplebarFraction()
    {
        return cooldown / cooldownTimer;
    }

    public Vector3 GetGrapplePoint()
    {
        return target;
    }
}