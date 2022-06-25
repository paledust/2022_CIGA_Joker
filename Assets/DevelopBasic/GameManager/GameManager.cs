using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Please make sure "GameManager" is excuted before every custom script
public class GameManager : Singleton<GameManager>
{
    public static Camera mainCam;
    public static Player player1;
    public static Player player2;
    private static bool isSwitchingScene = false;
    private static bool isPaused = false;
    private float putCoinsTimer = 0f;
    private float gameTimer = 0f;
    protected override void Awake()
    {
        base.Awake();
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
    private void FixedUpdate() {
        putCoinsTimer += Time.fixedDeltaTime;
        gameTimer += Time.fixedDeltaTime;
        if (putCoinsTimer > 20.0f)
        {
            EventHandler.Call_OnPutCoins();
            putCoinsTimer = 0;
        }
        if (gameTimer > 180.0f)
        {
            Debug.Log("GameOver!");
            gameTimer = 0;
        }
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
