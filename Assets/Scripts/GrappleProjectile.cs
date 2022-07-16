using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleProjectile : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public float maxDistance = 200f;
    public float tugStrength = 10f;
    public float damage = 30f;
    private Rigidbody player;
    private Vector3 spawnPoint, initVelocity;
    private bool contact = false;
    private GrapplingTool grapplingTool;

    public void setPlayer(Rigidbody select)
    {
        player = select;
        grapplingTool = player.GetComponentInChildren<GrapplingTool>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!contact)
        {
            spawnPoint = player.position;
            grapplingTool.rope(transform.position);
            // check if maxdistance has been reached
            if (Vector3.Distance(spawnPoint, transform.position) >= maxDistance)
            {
                Destroy(transform.parent.gameObject);
                grapplingTool.destroyRope();
            }
        }    
    }
    private void OnTriggerEnter(Collider other)
    {
        contact = true;
        Rigidbody target;
        // create the vector direction towards the player to tug to
        Vector3 tugVelocity = Vector3.Normalize(player.position - transform.position) * tugStrength;
        switch (other.gameObject.layer)
        {
            // if people
            case 6:
                // hurt them, drag them in reverse grapple direction
                target = other.GetComponent<Rigidbody>();
                other.GetComponent<Humanoid>().Damage(damage);
                target.AddForce(tugVelocity, ForceMode.VelocityChange);
                grapplingTool.destroyRope();
            break;
            // if ground
            case 7:
                // activate grapple
                grapplingTool.startGrapple(transform.position);
            break;
            // if item
            case 8:
                // drag item in reverse grapple direction
                target = other.GetComponent<Rigidbody>();
                target.AddForce(tugVelocity, ForceMode.VelocityChange);
                grapplingTool.destroyRope();
            break;
            default:
                grapplingTool.destroyRope();
            break;
        }
        Destroy(transform.parent.gameObject);
    }
}
