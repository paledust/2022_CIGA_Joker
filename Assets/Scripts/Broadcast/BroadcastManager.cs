using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BroadcastManager : MonoBehaviour
{
    private float broadcastTimer = 0f;
    private bool nextBroadcastType = true; // true : 下次广播为金币数量 false : 通知有新的被加入
    [SerializeField] private Text countdownText;
    [SerializeField] private Text broadcastText;
    private string historyBroadcast;
    private string newBroadcast;
    private void FixedUpdate() {
        if (!GameManager.Instance.gameRunning)   return;
        broadcastTimer += Time.fixedDeltaTime;
        countdownText.text = $"{((int)(10 - broadcastTimer))}秒后广播为{(nextBroadcastType ? "双方金币数量" : "新物品的加入")}";
        if (broadcastTimer > 10.0f)
        {
            historyBroadcast += newBroadcast;
            int time = ((int)GameManager.Instance.gameTimer);
            if (nextBroadcastType)
            {
                newBroadcast = $"[{time / 60:D2} : {time % 60 :D2}]\n"
                             + $"玩家1金币/炸弹数量：{GameManager.player1.coinAmount}/{GameManager.player1.bombAmount}\n"
                             + $"玩家2金币/炸弹数量：{GameManager.player2.coinAmount}/{GameManager.player2.bombAmount}\n";
            }
            else 
            {
                EventHandler.Call_OnPutCoins();
                BoxManager boxManager = GameManager.boxManager;
                newBroadcast = $"[{time / 60:D2} : {time % 60:D2}]\n有{boxManager.PutNum}个盒子新放入了一枚糖果！！！！快来拿啊！！！\n";
            }
            broadcastText.text = "<color=#8C8C8C>" + historyBroadcast + "</color>" + newBroadcast;
            nextBroadcastType = !nextBroadcastType;
            broadcastTimer = 0;
        }
    }
}