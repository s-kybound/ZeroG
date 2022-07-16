using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReticle : MonoBehaviour
{
    private Humanoid humanoid;
    public Text reticle;
    void Awake()
    {
        humanoid = GameObject.Find("Player").GetComponent<Humanoid>();
    }

    // Update is called once per frame
    void Update()
    {
        if(humanoid.amIDead())
            reticle.text = "DEAD!";
        else
            reticle.text = ">+<";        
    }
}
