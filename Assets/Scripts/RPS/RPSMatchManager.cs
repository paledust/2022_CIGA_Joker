using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RPSMatchManager : MonoBehaviour
{
    [SerializeField] private Text m_text;
    [SerializeField] private SpriteRenderer win_sprite;
    [SerializeField] private SpriteRenderer lose_sprite;
[Header("Winning Feedback")]
    [SerializeField] private GameObject win_result_obj;
    [SerializeField] private Animator win_result_Animator;
    [SerializeField] private string win_result_Trigger_String = "Win";
[Header("Tie Feedback")]
    [SerializeField] private Animation tie_animation;
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
        win_result_obj.SetActive(true);
        if(player1Choise == player2Choise){
            tie_animation.Play();
            win_sprite.sprite = GameManager.player2.GetStateSprite(PLAYER_SPRITE_STATE.RIGHT);
            lose_sprite.sprite = GameManager.player1.GetStateSprite(PLAYER_SPRITE_STATE.RIGHT);
            yield return new WaitForSeconds(tie_animation.clip.length);
        }
        else if(player2Choise == counterChoise(player1Choise)){
            //"玩家2获胜";
            if(!GameManager.player1.invincible && GameManager.player1.CoinAmount!=0){
                GameManager.player2.GetCoins(GameManager.player1.CoinAmount);
                GameManager.player1.LoseRPSTest();
            }

            win_result_Animator.SetTrigger(win_result_Trigger_String);
            win_sprite.sprite = GameManager.player2.GetStateSprite(PLAYER_SPRITE_STATE.RIGHT);
            lose_sprite.sprite = GameManager.player1.GetStateSprite(PLAYER_SPRITE_STATE.SAD);
            yield return new WaitForSeconds(2f);
            StartCoroutine(coroutineBlinkLoser());
            yield return new WaitForSeconds(0.8f);
        }
        else{
            //"玩家1获胜";
            if(!GameManager.player2.invincible && GameManager.player2.CoinAmount!=0){
                GameManager.player1.GetCoins(GameManager.player2.CoinAmount);
                GameManager.player2.LoseRPSTest();
            }

            win_result_Animator.SetTrigger(win_result_Trigger_String);
            win_sprite.sprite = GameManager.player1.GetStateSprite(PLAYER_SPRITE_STATE.RIGHT);
            lose_sprite.sprite = GameManager.player2.GetStateSprite(PLAYER_SPRITE_STATE.SAD);
            yield return new WaitForSeconds(2f);
            StartCoroutine(coroutineBlinkLoser());
            yield return new WaitForSeconds(0.8f);
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
    IEnumerator coroutineBlinkLoser(){
        for(float t=0;t<1;t+=Time.deltaTime/1.5f){
            lose_sprite.enabled = Mathf.Sin(t*5*2*Mathf.PI*1.5f)>0;
            yield return null;
        }
        lose_sprite.enabled = true;
    }
}
