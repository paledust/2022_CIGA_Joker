using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    private List<Box> allBoxes;
    private int num = 6;
    public int PutNum {
        get => Mathf.Min(num, allBoxes.Count);
    }
    // private List<Box> emptyBoxes;
    private void OnEnable() {
        EventHandler.E_OnPutCoins += PutCoins; 
    }

    private void OnDisable() {
        EventHandler.E_OnPutCoins -= PutCoins; 
    }
    
    private void Start() {
        allBoxes = new List<Box>(GetComponentsInChildren<Box>());
    }

    private void PutCoins() 
    {
        for (int i = allBoxes.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i);
            (allBoxes[i], allBoxes[rand]) = (allBoxes[rand], allBoxes[i]);
        }
        Debug.Log($"有{PutNum}个盒子新放入了一枚糖果！！！！快来拿啊！！！");
        for (int i = 0; i < PutNum; i++)
        {
            allBoxes[i].PutInCoin(1);
        }
    }
    
}
