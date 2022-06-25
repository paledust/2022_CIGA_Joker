using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BroadcastManager : MonoBehaviour
{
    private float broadcastTimer = 0f;
    private bool f = true; // true : 下次广播为金币数量 false : 通知有新的被加入
    [SerializeField] private Text countdownText;
    [SerializeField] private Text broadcastText;
    private void FixedUpdate() {
        broadcastTimer += Time.fixedDeltaTime;
        countdownText.text = $"{((int)(10 - broadcastTimer))}秒后广播为{(f ? "双方金币数量" : "新物品加入")}";
        if (broadcastTimer > 10.0f)
        {
            if (f)
            {
                broadcastText.text = $"玩家1金币/炸弹数量为：{GameManager.player1.coinAmount}/{GameManager.player1.bombAmount}\n";
                broadcastText.text += $"玩家2金币/炸弹数量为：{GameManager.player2.coinAmount}/{GameManager.player2.bombAmount}";
            }
            else 
            {
                EventHandler.Call_OnPutCoins();
                BoxManager boxManager = GameManager.boxManager;
                broadcastText.text = $"有{boxManager.PutNum}个盒子新放入了一枚糖果！！！！快来拿啊！！！";
            }
            f = !f;
            broadcastTimer = 0;
        }
    }
}