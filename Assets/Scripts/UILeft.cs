using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeft : MonoBehaviour
{
    private Humanoid playerHealth;
    private JetpackTool jetpackTool;
    public Text guiLeft;
    
    // Awake
    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        playerHealth = player.GetComponent<Humanoid>();
        jetpackTool = player.GetComponentInChildren<JetpackTool>();
    }
    // Update is called once per frame
    void Update()
    {
        string health, fuel;
        health = fuel = "|";
        // Health
        for (int x = 0; x < 10; x++)
        {
            if (x < (int)(playerHealth.healthbarFraction() * 10))
                health += "-";
            else
                health += " ";
        }
        health += "|";
        // Fuel
        for (int x = 0; x < 10; x++)
        {
            if (x < (int)(jetpackTool.jetpackbarFraction() * 10))
                fuel += "-";
            else
                fuel += " ";
        }
        fuel += "|";
        guiLeft.text = "HEALTH\n" + health + "\nFUEL\n" + fuel;
    }
}
