using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinHead : SpecialItem
{
    void OnTriggerEnter2D(Collider2D other){
        Player player = other.GetComponent<Player>();
        if(player!=null && player!=currentPlayer){
            
        }
    }
}
