using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItem : MonoBehaviour
{
    [SerializeField] protected SPECIAL_ITEM_TYPE itemType;
    [SerializeField] protected float LifeTime = 5;
    [SerializeField] protected Player currentPlayer;
    protected float timer = 0;
    public virtual void Initialize(Player inputPlayer){
        currentPlayer = inputPlayer;
    }
    void Update(){
        timer += Time.deltaTime;
        if(timer>LifeTime){
            Destroy(gameObject);
        }
    }
}
