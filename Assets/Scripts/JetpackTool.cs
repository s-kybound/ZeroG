using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackTool : MonoBehaviour
{
    // Default values of 100, 5, 2 give a delta-V of ~50m/s.
    public Rigidbody player;
    [Header("Gameplay Settings")]
    public float fuelCapacity = 300f;
    public float nozzleFuelConsumptionRate = 5f;
    public float thrustStrength = 10f;

    private float currentFuel;

    // Start is called before the first frame update
    void Start()
    {
        currentFuel = fuelCapacity;
    }
    // Update is called once per frame
    void Update()
    {
        // Check if there is sufficient fuel for one burst of all nozzles
        if (currentFuel > 3 * nozzleFuelConsumptionRate * Time.deltaTime)
        {
        Thrust();
        }
    }

    void Thrust()
    {
        // Forward and backward velocity
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) != (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            currentFuel = Mathf.Max(0, currentFuel - nozzleFuelConsumptionRate * Time.deltaTime);
            bool forward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            player.AddForce(thrustStrength * (forward ? transform.forward : -transform.forward));
        }
        // Upwards and downwards velocity
        if (Input.GetKey(KeyCode.Space) != Input.GetKey(KeyCode.C))
        {
            currentFuel = Mathf.Max(0, currentFuel - nozzleFuelConsumptionRate * Time.deltaTime);
            // todo   
            bool up = Input.GetKey(KeyCode.Space);
            player.AddForce(thrustStrength * (up ? transform.up : -transform.up));
        }
        // Left and right velocity
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) != (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
        {
            currentFuel = Mathf.Max(0, currentFuel - nozzleFuelConsumptionRate * Time.deltaTime);   
            // todo
            bool right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
            player.AddForce(thrustStrength * (right ? transform.right : -transform.right));
        }
    }

    public void addFuel(float addition)
    {
        currentFuel = Mathf.Min(fuelCapacity, currentFuel + addition);
    }

    public float jetpackbarFraction()
    {
        return currentFuel / fuelCapacity;
    }
    public float getVelocity()
    {
        return player.velocity.magnitude;
    }
}
