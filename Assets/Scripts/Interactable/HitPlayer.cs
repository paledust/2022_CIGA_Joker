using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    [SerializeField] private float effectTime = 5;
    void OnTriggerEnter2D(Collider2D other){
        Player player = other.GetComponent<Player>();
        if(player!=null){
            player.DamageTest();
            StartCoroutine(coroutineInversePlayerControl(player));
        }
    }
    IEnumerator coroutineInversePlayerControl(Player player){
        player.InverseControl();
        yield return new WaitForSeconds(effectTime);
        player.UnInverseControl();
    }
}
