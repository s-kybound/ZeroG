using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackFuel : MonoBehaviour
{
    public float fuelToAdd = 300f;
    public float respawnTime = 60f;
    private Rigidbody self;
    private Vector3 spawnPoint;
    private Quaternion spawnOrientation;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        self = GetComponent<Rigidbody>();
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
        transform.Rotate(new Vector3(0f, 30f, 0f) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && other.name == "Player")
        {
            JetpackTool target = other.GetComponentInChildren<JetpackTool>();
            target.addFuel(fuelToAdd);
            Invoke("Respawn", respawnTime);
            this.gameObject.SetActive(false);
        }
    }

    void Respawn()
    {
        transform.position = spawnPoint;
        transform.rotation = spawnOrientation;
        self.velocity = Vector3.zero;
        this.gameObject.SetActive(true);
    }
}
