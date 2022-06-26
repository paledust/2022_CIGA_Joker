using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : InteractableObject, IRPSable
{
    [SerializeField] private int ContainCoin;
    [SerializeField] private bool ContainBomb;
[Header("Animation")]
    [SerializeField] private Animation feedbackAnimation;
    [SerializeField] private Animator feedbackAnimator;
    [SerializeField] private Animator bombAnimator;
    [SerializeField] private Animator vfxAnimator;
[Header("RPS Mini Game")]
    [SerializeField] private SpriteRenderer m_match_renderer;
    [SerializeField] private SpriteRenderer m_rpschoise_renderer;
    [SerializeField] private RPS_CHOISE rpsChoise = RPS_CHOISE.ROCK;
    [SerializeField] private RPS_SO rpsData;
    [SerializeField] private GameObject rpsObj;
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
        currentPlayer.DamageTest(2);
        ContainBomb = false;

        //To Do: 添加player被炸的feedback
        Debug.Log($"玩家{currentPlayer.PlayerIndex+1}被炸弹炸了");
    }
    public Sprite GetMatchSprite(){return m_match_renderer.sprite;}
    public void ShowRPSResult(){
        rpsChoise = (RPS_CHOISE)Random.Range(0,3);
        m_rpschoise_renderer.GetComponent<Animator>().enabled = false;
        m_rpschoise_renderer.sprite = rpsData.GetRPSSprite(rpsChoise);
    }
    public RPS_CHOISE GetRPSChoice(){return rpsChoise;}
    public void EnterRPSMode(){
        rpsObj.SetActive(true);
        m_rpschoise_renderer.GetComponent<Animator>().enabled = true;
        m_rpschoise_renderer.GetComponent<Animator>().Play("RPS", 0, Random.Range(0f,1f));
    }
    public void ExitRPSMode(){rpsObj.SetActive(false);}
    public int GetCoins(){return ContainCoin;}
    public void HandleWinning(int opponentCoinAmount){ContainCoin ++;}
}
