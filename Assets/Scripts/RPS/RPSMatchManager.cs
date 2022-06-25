using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPSMatchManager : MonoBehaviour
{
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
            yield return new WaitForSeconds(1f);
            count ++;
        }

    //To Do:宣布比赛结果
        RPS_CHOISE player1Choise = GameManager.player1.rpsChoise;
        RPS_CHOISE player2Choise = GameManager.player2.rpsChoise;
        
        if(player1Choise == player2Choise){
            Debug.Log("平局");
        }
        else if(player2Choise == counterChoise(player1Choise)){
            Debug.Log("玩家2胜出");
            GameManager.player2.coinAmount += GameManager.player1.coinAmount;
            GameManager.player1.coinAmount = 0;
        }
        else{
            Debug.Log("玩家1胜出");
            GameManager.player1.coinAmount += GameManager.player2.coinAmount;
            GameManager.player2.coinAmount = 0;
        }

    //结束玩家的RPS MODE
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
