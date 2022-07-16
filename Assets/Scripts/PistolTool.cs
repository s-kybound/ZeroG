using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolTool : MonoBehaviour
{
    public Rigidbody player;
    public GameObject bulletProjectile;
    public Transform barrelTip;
    [Header("Gameplay Settings")]
    public int roundsPerSecond = 4;
    public float speed = 100f;

    private float cooldown, currentCooldown;
    // Start is called before the first frame update
    void Start()
    {
        cooldown = 1f / (float)roundsPerSecond;
        currentCooldown = cooldown;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && currentCooldown >= cooldown)
        {
            GameObject bullet = Instantiate(bulletProjectile, barrelTip.position, transform.rotation);
            bullet.GetComponentInChildren<Rigidbody>().velocity = player.velocity + transform.forward * speed;
            currentCooldown = 0f;
        }
        currentCooldown = Mathf.Min(cooldown + 1, currentCooldown + Time.deltaTime);
    }
}
