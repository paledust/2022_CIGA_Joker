using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.gameTimer = 0;
        // GameManager.Instance.gameRunning = true;
        GameManager.Instance.SwitchingScene("Level-0");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}