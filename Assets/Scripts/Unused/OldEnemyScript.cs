using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// With thanks to Dave / GameDevelopment on inspiration for structure of page
public class OldEnemyScript : MonoBehaviour
{
    private GameObject player, grapple, bullet;
    private Rigidbody selfVel;
    private LineRenderer line;
    private Humanoid selfHealth;
    private Vector3 relvel, spawn, startPoint, endPoint;
    private Quaternion spawnOrientation;
    private SlayerLeaderboard slayer;
    private bool respawning = false;
    private bool hasGoal = false;
    private float gunCooldown, currentGunCooldown, currentGrappleCooldown;
    public Transform gunTip;
    public GameObject bulletProjectile;
    public GameObject grappleProjectile;
    public float attackDistance = 70f;
    public float grappleDistance = 110f;
    public float interceptDistance = 300f;
    public float thrustStrength = 9f;
    public float roundsPerSecond = 2;
    public float grappleCooldown = 2f;
    public float bulletSpeed = 60f;
    public float grappleSpeed = 30f;
    public float distanceFromStart = 300f;
    public float patrolDistance = 50f;
    public float respawnTime = 4f;
 
    void Awake()
    {
        player = GameObject.Find("Player");
        selfVel = GetComponent<Rigidbody>();
        selfHealth = GetComponent<Humanoid>();
        line = GetComponent<LineRenderer>();
        slayer = GameObject.Find("Leaderboard").GetComponent<SlayerLeaderboard>();
    }

    void Start()
    {
        gunCooldown = 1f / roundsPerSecond;
        currentGunCooldown = gunCooldown;
        currentGrappleCooldown = grappleCooldown;
        spawn = transform.position;
        spawnOrientation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(selfHealth.amIDead())
            Die();
        else
        {
            selfHealth.Regen();
            float playerDist = Vector3.Distance(player.transform.position, transform.position);
            if (playerDist <= attackDistance)
                Attack();
            else if (playerDist <= grappleDistance)
                Grapple();
            else if (playerDist <= interceptDistance)
                Intercept();
            else
                Patrol();
            currentGunCooldown = Mathf.Min(gunCooldown + 1, currentGunCooldown + Time.deltaTime);
            currentGrappleCooldown = Mathf.Min(grappleCooldown + 1, currentGrappleCooldown + Time.deltaTime);
        }
    }

    void Patrol()
    {
        // Move randomly, keep within a specific range from start
        if ((spawn - transform.position).magnitude > distanceFromStart)
        {
            // Burn towards start, create a random new endpoint to travel to
            hasGoal = true;
            startPoint = transform.position;
            endPoint = spawn + new Vector3(UnityEngine.Random.Range(-patrolDistance, patrolDistance), UnityEngine.Random.Range(-patrolDistance, patrolDistance), UnityEngine.Random.Range(-patrolDistance, patrolDistance));
            transform.LookAt(endPoint);
            selfVel.AddForce(transform.forward * thrustStrength * Time.deltaTime);
        }
        else
        {
            if (!hasGoal)
            {
                startPoint = transform.position;
                endPoint = transform.position + new Vector3(UnityEngine.Random.Range(-patrolDistance, patrolDistance), UnityEngine.Random.Range(-patrolDistance, patrolDistance), UnityEngine.Random.Range(-patrolDistance, patrolDistance));
                hasGoal = true;
            }
            else
            {
                // Check if within halfway of the journey to the new point
                if ((transform.position - endPoint).magnitude < ((startPoint - endPoint).magnitude / 2))
                {
                    if (selfVel.velocity.magnitude < 0.01f)
                    {
                        // Job done, end goal
                        hasGoal = false;
                    }
                    else
                    {
                        // Look in the retrograde direction and burn
                        transform.LookAt(transform.position - selfVel.velocity);
                        selfVel.AddForce(transform.forward * thrustStrength * Time.deltaTime);
                    }
                }
                else
                {
                    transform.LookAt(endPoint);
                    selfVel.AddForce(transform.forward * thrustStrength * Time.deltaTime);
                }
            }
        }
    }

    void Intercept()
    {
        hasGoal = false;
        // Velocity calculating code taken from below, see Grapple() for more detail on these calculations.
        // Here we intend to bleed off lateral velocity and then burn towards the player.
        relvel = player.GetComponent<Rigidbody>().velocity - selfVel.velocity;
        Vector3 towardsVector = (transform.position - player.transform.position).normalized;
        // Find the magnitude with dot product
        float towardsVectorMagnitude =  Vector3.Dot(relvel, towardsVector);
        towardsVector = towardsVectorMagnitude * towardsVector;
        // Now we find the lateral vector. relvel is comprised of only these 2 vectors so we can find the last simply by subtracting
        Vector3 lateralVector = relvel - towardsVector;
        // If lateral Velocity is significant, we eliminate it first
        if (lateralVector.magnitude > 2f)
            transform.LookAt(transform.position + lateralVector);
        // Else we aim directly at the player and burn
        else
            transform.LookAt(player.transform);
        selfVel.AddForce(transform.forward * thrustStrength * Time.deltaTime);
    }

    void Grapple()
    {
        hasGoal = false;
        // Fire grapples at player
        relvel = player.GetComponent<Rigidbody>().velocity - selfVel.velocity;
        // Split relative velocity into 2 components - lateral component and component towards enemy.
        // Towards vector is obtained by (relvel.pointing to me vector)pointing to me vector where pointing to me vector is a unit vector
        // Initially create towardsVector as that unit vector, we will find the true magnitude soon.
        Vector3 towardsVector = (transform.position - player.transform.position).normalized;
        // Find the magnitude with dot product
        float towardsVectorMagnitude =  Vector3.Dot(relvel, towardsVector);
        towardsVector = towardsVectorMagnitude * towardsVector;
        // Now we find the lateral vector. relvel is comprised of only these 2 vectors so we can find the last simply by subtracting
        Vector3 lateralVector = relvel - towardsVector;
        // Our grapple will be shot with a vector with the same component of lateralVelocity, as well as a towardsVector in the opposite direction(if object is moving TOWARDS us), and of different magnitude.
        // To find the magnitude of this reverse vector, since lateral and towards vectors are perpendicular, we can use pythagoras theorem,
        // hypothenuse^2 = breadth^2 + length^2, as the triangle formed by our final vector and these 2 other vectors is a right-hand triangle.
        // Knowing the length of hypothenuse (grapple velocity) as well as length of either length/breadth (lateralVector), we solve to find the other length(magnitude).
        // using breadth = square root(hypothenuse^2 - length^2)
        float awayVectorMagnitude = Mathf.Sqrt(grappleSpeed * grappleSpeed - lateralVector.sqrMagnitude);
        Vector3 awayVector = awayVectorMagnitude * (player.transform.position - transform.position).normalized;
        Vector3 grappleVector = awayVector + lateralVector;
        // If below is true, there is no hope for the grapple to reach the player. Burn towards player instead.
        if (towardsVectorMagnitude < 0 && towardsVector.magnitude > awayVector.magnitude)
        {
            transform.LookAt(player.transform);
            selfVel.AddForce(transform.forward * thrustStrength * Time.deltaTime);
        }
        else
        {
            transform.LookAt(transform.position + grappleVector);
            if (currentGrappleCooldown >= grappleCooldown)
            {
                if (grapple)
                    return;
                grapple = Instantiate(grappleProjectile, gunTip.position, transform.rotation);
                grapple.GetComponentInChildren<Rigidbody>().velocity = selfVel.velocity + transform.forward * grappleSpeed;
                grapple.GetComponentInChildren<EnemyGrapple>().setPlayer(selfVel);
                line.positionCount = 2;
                currentGrappleCooldown = 0f;
            }
        }
        
    }

    void Attack()
    {
        hasGoal = false;
        // Match velocity and shoot
        relvel = player.GetComponent<Rigidbody>().velocity - selfVel.velocity;
        selfVel.AddForce(relvel.normalized * Mathf.Min(thrustStrength, relvel.magnitude) * Time.deltaTime);
        transform.LookAt(player.transform);
        if (currentGunCooldown >= gunCooldown)
        {
            bullet = Instantiate(bulletProjectile, gunTip.position, transform.rotation);
            bullet.GetComponentInChildren<Rigidbody>().velocity = selfVel.velocity + transform.forward * bulletSpeed;
            currentGunCooldown = 0f;
        }
    }

    void Die()
    {
        if (!respawning)
        {
            slayer.playerUp(1);
            respawning = true;
            enabled = false;
            Invoke("Respawn", respawnTime);
        }
    }

    void Respawn()
    {
        respawning = false;
        selfHealth.addHealth(selfHealth.maxHealth);
        // Randomize spawnpoint
        transform.position = spawn + new Vector3(UnityEngine.Random.Range(-patrolDistance, patrolDistance), UnityEngine.Random.Range(-patrolDistance, patrolDistance), UnityEngine.Random.Range(-patrolDistance, patrolDistance));
        transform.rotation = spawnOrientation;
        selfVel.velocity = Vector3.zero;
        enabled = true;
    }

    public void rope(Vector3 bulletpos)
    {
        line.SetPosition(0, gunTip.position);
        line.SetPosition(1, bulletpos);
    }

    public void destroyRope()
    {
        line.positionCount = 0;
    }
}
