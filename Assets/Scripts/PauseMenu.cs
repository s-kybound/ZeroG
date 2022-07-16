using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool paused;
    private GameObject player;
    public GameObject pauseMenu;
    void Awake()
    {
        player = GameObject.Find("Player");
        paused = false;
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            if (paused)
                Pause();
            else
                Resume();
        }
    }

    void Pause()
    {
        bool on = false;
        player.GetComponent<PlayerHealth>().enabled = on;
        player.GetComponent<PlayerCamera>().enabled = on;
        player.GetComponent<Humanoid>().enabled = on;
        player.GetComponentInChildren<GrappleRotator>().enabled = on;
        player.GetComponentInChildren<GrapplingTool>().enabled = on;
        player.GetComponentInChildren<GrappleRotator>().enabled = on;
        player.GetComponentInChildren<PistolTool>().enabled = on;
        player.GetComponentInChildren<JetpackTool>().enabled = on;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    void Resume()
    {
        bool on = true;
        player.GetComponent<PlayerHealth>().enabled = on;
        player.GetComponent<PlayerCamera>().enabled = on;
        player.GetComponent<Humanoid>().enabled = on;
        player.GetComponentInChildren<GrappleRotator>().enabled = on;
        player.GetComponentInChildren<GrapplingTool>().enabled = on;
        player.GetComponentInChildren<GrappleRotator>().enabled = on;
        player.GetComponentInChildren<PistolTool>().enabled = on;
        player.GetComponentInChildren<JetpackTool>().enabled = on;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
}
