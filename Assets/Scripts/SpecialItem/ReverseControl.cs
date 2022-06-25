using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseControl : SpecialItem
{
    private Player opponent;
    public override void Initialize(Player inputPlayer)
    {
        base.Initialize(inputPlayer);
        opponent = (inputPlayer == GameManager.player1?GameManager.player2:GameManager.player1);
        opponent.InverseControl();
    }
    void OnDestroy(){
        opponent.UnInverseControl();
    }
}
