using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferDamage : SpecialItem
{
    private Player opponent;
    public override void Initialize(Player inputPlayer)
    {
        base.Initialize(inputPlayer);
        opponent = (inputPlayer == GameManager.player1?GameManager.player2:GameManager.player1);
        currentPlayer.CanTransferDamage = true;

        EventHandler.E_OnTransferDamage += TrySendDamage;
    }
    void OnDestroy(){
        EventHandler.E_OnTransferDamage -= TrySendDamage;
    }
    void TrySendDamage(int damage, Player callPlayer){
        if(callPlayer != currentPlayer) return;
        opponent.LoseCoins(damage);
    }
}
