using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BroadcastManager : MonoBehaviour
{
    private float broadcastTimer = 0f;
    // private bool nextBroadcastType = true; // true : 下次广播为金币数量 false : 通知有新的被加入
    // [SerializeField] private Text countdownText;
    // [SerializeField] private Text broadcastText;
    // private string historyBroadcast;
    // private string newBroadcast;
    private void FixedUpdate() {
        if (!GameManager.Instance.gameRunning)   return;
        broadcastTimer += Time.fixedDeltaTime;
        if (broadcastTimer > 5.0f)
        {
            // historyBroadcast += newBroadcast;
            // int time = ((int)GameManager.Instance.gameTimer);
            // BoxManager boxManager = GameManager.boxManager;
            // newBroadcast = $"[{time / 60:D2} : {time % 60:D2}]\n有{boxManager.PutNum}个盒子新放入了新的糖果！！！！快来拿啊！！！\n";
            // broadcastText.text = "<color=#8C8C8C>" + historyBroadcast + "</color>" + newBroadcast;
            EventHandler.Call_OnPutCoins();
            broadcastTimer = 0;
        }
    }
}