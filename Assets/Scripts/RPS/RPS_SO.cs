using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TrickOrTreat/RPS_SO")]
public class RPS_SO : ScriptableObject
{
    [SerializeField] private List<RPSData> RPSData;
    public Sprite GetRPSSprite(RPS_CHOISE rpsChoise){
        return RPSData.Find(x=>x.rpsChoise == rpsChoise).rpsSprite;
    }
}
[System.Serializable]
public class RPSData{
    public RPS_CHOISE rpsChoise;
    public Sprite rpsSprite;
}
