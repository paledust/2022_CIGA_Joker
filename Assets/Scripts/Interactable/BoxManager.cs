using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    private List<Box> allBoxes;
    public int putNum = 3;
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
            int t = Random.Range(0, i);
            (allBoxes[i], allBoxes[t]) = (allBoxes[t], allBoxes[i]);
        }

        for (int i = 0; i < System.Math.Max(putNum, allBoxes.Count); i++)
        {
            allBoxes[i].PutInCoin(1);
        }
    }
    
}
