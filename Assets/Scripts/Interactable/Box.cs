using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : InteractableObject
{
    [SerializeField] private int ContainCoin;
    [SerializeField] private bool[] ContainBomb = new bool[2];
    [SerializeField] private Animation feedbackAnimation;
    private bool PlayingFeedback = false;
    public override void OnInteract(INTERACTABLE_TYPE interactableType, Player currentPlayer)
    {
        base.OnInteract(interactableType, currentPlayer);
        if(PlayingFeedback) return;

        int playerIndex = currentPlayer.PlayerIndex;

        switch(interactableType){
            case INTERACTABLE_TYPE.PUT_IN_COIN:
                if(currentPlayer.coinAmount>0){
                    currentPlayer.coinAmount --;
                    ContainCoin ++;
                    StartCoroutine(CoroutinePlayingFeedback());
                    Debug.Log("有玩家放入金币");
                }
                break;
            case INTERACTABLE_TYPE.PUT_IN_BOMB:
                if(currentPlayer.bombAmount>0){
                    if(!ContainBomb[playerIndex]){
                        ContainBomb[playerIndex] = true;
                        currentPlayer.bombAmount --;
                        StartCoroutine(CoroutinePlayingFeedback());
                    }

                    Debug.Log($"玩家{playerIndex+1}放入一枚炸弹");
                }
                break;
            case INTERACTABLE_TYPE.TAKE_OUT_STUFF:
                if(ContainBomb[1-playerIndex]){
                    currentPlayer.coinAmount = currentPlayer.coinAmount/2;
                    ContainBomb[1-playerIndex] = false;

                    //To Do: 添加player被炸的feedback
                    Debug.Log($"玩家{playerIndex+1}被炸弹炸了");
                }
                else if(ContainCoin>0){
                    currentPlayer.coinAmount += ContainCoin;
                    ContainCoin --;
                    Debug.Log("有玩家拿起金币");
                }

                //WARNING: 目前盒子空的情况下，拿起来也是有反馈
                StartCoroutine(CoroutinePlayingFeedback());
                break;
        }
    }
    IEnumerator CoroutinePlayingFeedback(){
        PlayingFeedback = true;
        feedbackAnimation.Play();
        yield return new WaitForSeconds(feedbackAnimation.clip.length);
        PlayingFeedback = false;
    }
}
