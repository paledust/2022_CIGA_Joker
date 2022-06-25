using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Please make sure "GameManager" is excuted before every custom script
public class GameManager : Singleton<GameManager>
{
    public static Camera mainCam;
    public static Player player1;
    public static Player player2;
    public static BoxManager boxManager;
    private static bool isSwitchingScene = false;
    private static bool isPaused = false;
    public float gameTimer = 0f;
    public bool gameRunning = false;
    private bool restartFlag1, restartFlag2;
    [SerializeField] private Text gameoverText;
    [SerializeField] private Text countdownText;
    [SerializeField] private Image gameoverImage;
    protected override void Awake()
    {
        base.Awake();
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
    private void FixedUpdate() {
        if (!gameRunning)   return;
        gameTimer += Time.fixedDeltaTime;
        int lastTime = ((int)(180 - gameTimer));
        countdownText.text = $"{lastTime / 60:D2} : {lastTime % 60:D2}";
        if (gameTimer > 5.0f)
        {
            GameOver();
            gameRunning = false;
            gameTimer = 0;
        }
    }
    public void GameOver()
    {
        gameoverImage.gameObject.SetActive(true);
        Debug.Log("GameOver!");
        int coin1 = player1.coinAmount;
        int coin2 = player2.coinAmount;
        string winner = coin1 == coin2 ? "平局" : coin1 > coin2 ? "玩家1获胜" : "玩家2获胜";
        gameoverText.text = $"游戏结束！\n({coin1}:{coin2})\n{winner}！";
        player1.PauseInput();
        player2.PauseInput();
    }

    public void Restart()
    {
        gameoverImage.gameObject.SetActive(false);
        gameTimer = 0;
        gameRunning = true;
        SwitchingScene("Level-0", "Level-0");
    }    
    public void Exit()
    {
        gameoverImage.gameObject.SetActive(false);
        SwitchingScene("Level-0", "Start"); 
    }
    public void SwitchingScene(string from, string to){
        if(!isSwitchingScene){
            StartCoroutine(SwitchSceneCoroutine(from, to));
        }
    }
    public void SwitchingScene(string to){
        if(!isSwitchingScene){
            StartCoroutine(SwitchSceneCoroutine(to));
        }
    }
    public void PauseTheGame(){
        if(isPaused) return;
        
        Time.timeScale = 0;
        AudioListener.pause = true;
        isPaused = true;
    }
    public void ResumeTheGame(){
        if(!isPaused) return;

        AudioListener.pause = false;
        Time.timeScale = 1;
        isPaused = false;
    }
    /// <summary>
    /// This method is good for load scene in an additive way, having a persistance scene
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    IEnumerator SwitchSceneCoroutine(string from, string to){
        isSwitchingScene = true;

        if(from != string.Empty){
            //TO DO: do something before the last scene is unloaded. e.g: call event of saving 
            yield return SceneManager.UnloadSceneAsync(from);
        }
        //TO DO: do something after the last scene is unloaded.
        yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(to));
        //TO DO: do something after the next scene is loaded. e.g: call event of loading

        isSwitchingScene = false;
    }
    /// <summary>
    /// This method is good for load one scene each time, no persistance scene
    /// </summary>
    /// <param name="to"></param>
    /// <returns></returns>
    IEnumerator SwitchSceneCoroutine(string to){
        isSwitchingScene = true;

        //TO DO: do something before the next scene is loaded. e.g: call event of saving 
        yield return SceneManager.LoadSceneAsync(to);
        //TO DO: do something after the next scene is loaded. e.g: call event of loading

        isSwitchingScene = false;
    }
}
