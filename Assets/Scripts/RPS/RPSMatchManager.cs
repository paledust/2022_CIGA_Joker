using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RPSMatchManager : MonoBehaviour
{
    [SerializeField] private Text m_text;
    [SerializeField] private SpriteRenderer win_sprite;
    [SerializeField] private SpriteRenderer lose_sprite;
    [SerializeField] private GameObject win_result_obj;
    [SerializeField] private Animator win_result_Animator;
    [SerializeField] private string win_result_Trigger_String = "Win";
    void OnEnable(){
        EventHandler.E_OnEnterRPSMode += StartRPSMode;
    }
    void OnDisable(){
        EventHandler.E_OnEnterRPSMode += StartRPSMode;
    }
    void StartRPSMode(){
        StartCoroutine(coroutineRPSMatch());
    }
    IEnumerator coroutineRPSMatch(){
        int count = 0;
        for(int i=0; i<4; i++){
            m_text.text = (3-count).ToString();
            yield return new WaitForSeconds(1f);
            count ++;
        }

    //To Do:宣布比赛结果
        m_text.text = "结束!";

        GameManager.player1.PauseInput();
        GameManager.player2.PauseInput();

        GameManager.player1.ShowRPSResult();
        GameManager.player2.ShowRPSResult();

        RPS_CHOISE player1Choise = GameManager.player1.rpsChoise;
        RPS_CHOISE player2Choise = GameManager.player2.rpsChoise;
        
        yield return new WaitForSeconds(1f);

        m_text.text = string.Empty;
        if(player1Choise == player2Choise){
            m_text.text = "平局";
        }
        else if(player2Choise == counterChoise(player1Choise)){
            // m_text.text = "玩家2获胜";
            if(GameManager.player1.coinAmount!=0) GameManager.player2.GetCoins(GameManager.player1.coinAmount);
            GameManager.player1.coinAmount = 0;
            win_result_obj.SetActive(true);
            win_result_Animator.SetTrigger(win_result_Trigger_String);
            win_sprite.sprite = GameManager.player2.GetWinningSprite();
            lose_sprite.sprite = GameManager.player1.GetWinningSprite();
            yield return new WaitForSeconds(2.8f);
        }
        else{
            // m_text.text = "玩家1获胜";
            if(GameManager.player2.coinAmount!=0) GameManager.player1.GetCoins(GameManager.player2.coinAmount);
            GameManager.player2.coinAmount = 0;
            win_result_obj.SetActive(true);
            win_result_Animator.SetTrigger(win_result_Trigger_String);
            win_sprite.sprite = GameManager.player1.GetWinningSprite();
            lose_sprite.sprite = GameManager.player2.GetWinningSprite();
            yield return new WaitForSeconds(2.8f);
        }

        yield return new WaitForSeconds(1.5f);

        win_result_obj.SetActive(false);
    //结束玩家的RPS MODE
        m_text.text = string.Empty;

        GameManager.player1.ResumeInput();
        GameManager.player2.ResumeInput();

        GameManager.player1.ExitRPSMode();
        GameManager.player2.ExitRPSMode();

    }
    RPS_CHOISE counterChoise(RPS_CHOISE currentChoise){
        switch (currentChoise){
            case RPS_CHOISE.PAPER:
                return RPS_CHOISE.SISSOR;
            case RPS_CHOISE.ROCK:
                return RPS_CHOISE.PAPER;
            case RPS_CHOISE.SISSOR:
                return RPS_CHOISE.ROCK;
            default:
                return RPS_CHOISE.SISSOR;
        }
    }
}
