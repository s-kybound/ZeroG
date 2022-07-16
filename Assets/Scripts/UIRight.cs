using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRight : MonoBehaviour
{
    private GrapplingTool grapplingTool;
    private JetpackTool jetpackTool;
    public Text guiRight;
    
    // Awake
    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        grapplingTool = player.GetComponentInChildren<GrapplingTool>();
        jetpackTool = player.GetComponentInChildren<JetpackTool>();
    }
    // Update is called once per frame
    void Update()
    {
        string grapple;
        grapple = "|";
        // Grapple
        for (int x = 0; x < 10; x++)
        {
            if (x < (10 - (int)(grapplingTool.grapplebarFraction() * 10)))
                grapple += " ";
            else
                grapple += "-";
        }
        grapple += "|";
        guiRight.text = "GRAPPLE\n" + grapple + "\nVELOCITY: " + jetpackTool.getVelocity().ToString("F2") + "m/s";
    }
}
