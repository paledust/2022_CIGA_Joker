using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : InteractableObject
{
    [SerializeField] private int ContainCoin;
    [SerializeField] private bool ContainBomb;
[Header("Animation")]
    [SerializeField] private Animation feedbackAnimation;
    [SerializeField] private Animator feedbackAnimator;
    [SerializeField] private Animator bombAnimator;
    [SerializeField] private Animator vfxAnimator;
    public bool IsEmpty{get{return ContainCoin==0 && !ContainBomb;}}
    private string vfxTrigger = "Play";
    private bool PlayingFeedback = false;
    public override void OnInteract(INTERACTABLE_TYPE interactableType, Player currentPlayer)
    {
        base.OnInteract(interactableType, currentPlayer);
        if(PlayingFeedback) return;

        int playerIndex = currentPlayer.PlayerIndex;

        switch(interactableType){
            case INTERACTABLE_TYPE.PUT_IN_COIN:
                if(ContainBomb){
                    BombTest(currentPlayer);
                }
                else if(currentPlayer.CoinAmount>0){
                    currentPlayer.MinusOneCoin();
                    ContainCoin ++;
                    StartCoroutine(CoroutinePlayingFeedback());
                    
                    Debug.Log("有玩家放入金币");
                }
                break;
            case INTERACTABLE_TYPE.PUT_IN_BOMB:
                if(ContainBomb){
                    BombTest(currentPlayer);
                }
                else if(currentPlayer.bombAmount>0){
                    ContainBomb = true;
                    currentPlayer.bombAmount --;
                    StartCoroutine(CoroutinePlayingFeedback());

                    Debug.Log($"玩家{playerIndex+1}放入一枚炸弹");
                }
                break;
            case INTERACTABLE_TYPE.TAKE_OUT_STUFF:
                if(ContainBomb){
                    BombTest(currentPlayer);
                }
                else if(ContainCoin>0){
                    currentPlayer.GetCoins(ContainCoin);
                    ContainCoin -= ContainCoin;
                    Debug.Log("有玩家拿起金币");
                }

                //WARNING: 目前盒子空的情况下，拿起来也是有反馈
                StartCoroutine(CoroutinePlayingFeedback());
                break;
        }
    }
    public void PutInCoin(int amount)=>ContainCoin += amount;
    IEnumerator CoroutinePlayingFeedback(){
        PlayingFeedback = true;
        feedbackAnimation.Play();
        feedbackAnimator.Play("Box_Open", 0, 0);
        yield return new WaitForSeconds(feedbackAnimation.clip.length);
        PlayingFeedback = false;
    }
    void BombTest(Player currentPlayer){
        vfxAnimator.SetTrigger(vfxTrigger);
        bombAnimator.SetTrigger("Bomb");
        currentPlayer.DamageTest();
        ContainBomb = false;

        //To Do: 添加player被炸的feedback
        Debug.Log($"玩家{currentPlayer.PlayerIndex+1}被炸弹炸了");
    }
}
