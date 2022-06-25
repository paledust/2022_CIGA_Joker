using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    private List<Box> allBoxes;
    public int putNum = 3;
    // private List<Box> emptyBoxes;
    private void Start() {
        allBoxes = new List<Box>(GetComponentsInChildren<Box>());
        EventHandler.E_OnPutCoins += PutCoins; //最好写在OnEnable以及OnDisable里面，方便注册以及注销delegate。否则在切换场景或者游戏结束时容易出现内存泄漏。
    }

    private void PutCoins() 
    {
        for (int i = allBoxes.Count - 1; i > 0; i--)
        {
            int t = Random.Range(0, i);
            (allBoxes[i], allBoxes[t]) = (allBoxes[t], allBoxes[i]);
        }

        for (int i = 0; i < System.Math.Max(putNum, allBoxes.Count); i++)
        {
            allBoxes[i].PutInCoin(1);
        }
    }
    
}
