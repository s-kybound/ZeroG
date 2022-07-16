using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public float maxDistance = 100f;
    public float damage = 10f;
    private Vector3 spawnPoint;

    void Start()
    {
        spawnPoint = transform.position;  
    }

    // Update is called once per frame
    void Update()
    {
            if (Vector3.Distance(spawnPoint, transform.position) >= maxDistance)
            {
                Destroy(transform.parent.gameObject);
            }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            // if people
            case 6:
                // hurt them
                other.GetComponent<Humanoid>().Damage(damage);
            break;
            default:
            break;
        }
        Destroy(transform.parent.gameObject);
    }
}