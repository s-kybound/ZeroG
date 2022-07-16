using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrapple : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public float maxDistance = 150f;
    public float tugStrength = 10f;
    public float damage = 30f;
    private Rigidbody player;
    private Vector3 spawnPoint, initVelocity;
    private bool contact = false;
    // v needs redoing
    private EnemyScript grapplingTool;

    public void setPlayer(Rigidbody select)
    {
        player = select;
        grapplingTool = player.GetComponentInChildren<EnemyScript>();
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
        if (other.gameObject.layer == 6 && other.name == "Player")
        {
            // hurt them, drag them in reverse grapple direction
            target = other.GetComponent<Rigidbody>();
            other.GetComponent<Humanoid>().Damage(damage);
            target.AddForce(tugVelocity, ForceMode.VelocityChange);
        }
        Destroy(transform.parent.gameObject);
        grapplingTool.destroyRope();
    }
}
