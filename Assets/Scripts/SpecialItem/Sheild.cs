using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : SpecialItem
{
    public override void Initialize(Player inputPlayer){
        base.Initialize(inputPlayer);
        inputPlayer.BeInvinsible();   
    }
    public void OnDestroy(){
        currentPlayer.NotBeInvincible();
    }
}
