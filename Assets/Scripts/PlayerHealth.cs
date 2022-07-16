using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Manages life functions for the player.
    public Humanoid humanoid;
    public GrappleRotator grappleRotator;
    public GrapplingTool grapplingTool;
    public JetpackTool jetpackTool;
    public PistolTool pistolTool;
    public PlayerCamera playerCamera;
    public Rigidbody player;

    [Header("Gameplay Settings")]
    public float respawnTime = 4f;
    public float collisionDamageMinimumVelocity = 15f;
    private Vector3 spawnPoint;
    private Quaternion spawnOrientation;
    private bool respawning = false;
    private bool deathToCollision = false;
    private SlayerLeaderboard slayer;

    void Awake()
    {
        humanoid = GetComponent<Humanoid>();
        slayer = GameObject.Find("Leaderboard").GetComponent<SlayerLeaderboard>();
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        spawnOrientation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (humanoid.getHealth() <= 0)
        {
            Die();
        }
        else
        {
            humanoid.Regen();
        }
    }

    void Die()
    {
        if (!respawning)
        {
            // If you died to a collision, you lose one kill instead.
            if (deathToCollision)
            {
                deathToCollision = false;
                slayer.playerUp(-1);
            }
            else
            {
                slayer.enemyUp(1);
            }
            respawning = true;
            enabled = false;
            // todo: find a better way to do the following
            grapplingTool.endGrapple();
            grappleRotator.enabled = false;
            grapplingTool.enabled = false;
            jetpackTool.enabled = false;
            pistolTool.enabled = false;
            playerCamera.enabled = false;
            player.constraints = RigidbodyConstraints.None;
            Invoke("Respawn", respawnTime);
        }
    }

    void Respawn()
    {
        respawning = false;
        jetpackTool.addFuel(jetpackTool.fuelCapacity);
        humanoid.addHealth(humanoid.maxHealth);
        grappleRotator.enabled = true;
        grapplingTool.enabled = true;
        jetpackTool.enabled = true;
        pistolTool.enabled = true;
        playerCamera.enabled = true;
        transform.position = spawnPoint;
        transform.rotation = spawnOrientation;
        player.constraints = RigidbodyConstraints.FreezeRotation;
        player.velocity = Vector3.zero;
        enabled = true;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Ensure that minimum velocity is met, and colliding body is of either ground or default, so as to not double count damage. Also, make sure player was alive at collision hit;
        if (collision.relativeVelocity.magnitude >= collisionDamageMinimumVelocity && (collision.collider.gameObject.layer == 0 || collision.collider.gameObject.layer == 7) && !humanoid.amIDead())
        {
            if (humanoid.Damage(0.5f * collision.relativeVelocity.magnitude) <= 0)
                deathToCollision = true;
        }
    }
}
