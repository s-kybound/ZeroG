using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SlayerLeaderboard : MonoBehaviour
{
    public Text topText;
    private int playerCount, enemyCount;
    // Start is called before the first frame update
    void Awake()
    {
        playerCount = enemyCount = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        topText.text = "SLAYER\n3 KILLS TO WIN\n";
        if (playerCount >= 3 && enemyCount >= 3)
        {
            topText.text += "TIE";
            Invoke("returnMenu",3f);
        }
        else if (playerCount >= 3)
        {
            topText.text += "PLAYER WINS";
            Invoke("returnMenu",3f);
        }
        else if (enemyCount >= 3)
        {
            topText.text += "AI WINS";
            Invoke("returnMenu",3f);
        }
        else
        {
            topText.text += playerCount + " - PLAYER |      AI     - " + enemyCount;
        }   
    }

    void returnMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
    public void playerUp(int add)
    {
        playerCount += add;
    }
    public void enemyUp(int add)
    {
        enemyCount += add;
    }
    

}
