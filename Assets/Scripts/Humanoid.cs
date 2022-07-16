using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public float maxHealth = 100f;
    public float regenPerSecond = 1f;
    private float health;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (health <= 0)
            dead = true;
        else
            dead = false;
    }

    public float getHealth()
    {
        return health;
    }

    public float Damage(float dmg)
    {
        health = Mathf.Max(-1f, health - dmg);
        return health;
    }

    public float healthbarFraction()
    {
        return health / maxHealth;
    }

    public void Regen()
    {
        health = Mathf.Min(100, health + regenPerSecond * Time.deltaTime);
    }

    public float addHealth(float addedHealth)
    {
        health = Mathf.Max(maxHealth, health + addedHealth);
        return health;
    }
    public bool amIDead()
    {
        return dead;
    }
}
