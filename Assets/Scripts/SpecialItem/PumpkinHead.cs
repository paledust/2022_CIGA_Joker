using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinHead : SpecialItem
{
    [SerializeField] private float SpeedIncreaseScale  = 1.5f;
    public override void Initialize(Player inputPlayer)
    {
        base.Initialize(inputPlayer);
        currentPlayer.ChangeSpeedScale(SpeedIncreaseScale);
    }
    void OnDestroy(){{
        currentPlayer.ChangeSpeedScale(1);
    }}
    void OnTriggerEnter2D(Collider2D other){
        Player player = other.GetComponent<Player>();
        if(player!=null && player != currentPlayer){
            player.GetPoisoned();
        }
    }
    void OnTriggerExit2D(Collider2D other){
        Player player = other.GetComponent<Player>();
        if(player!=null && player != currentPlayer){
            player.Recovered();
        }
    }
}
