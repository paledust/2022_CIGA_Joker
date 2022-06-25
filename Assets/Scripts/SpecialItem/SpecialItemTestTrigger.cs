using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItemTestTrigger : MonoBehaviour
{
    [SerializeField] private SPECIAL_ITEM_TYPE itemtype;
    [SerializeField] private SpecialItem_SO itemData;
    void OnTriggerEnter2D(Collider2D other){
        Player player = other.GetComponent<Player>();
        if(player!=null){
            GameObject.Instantiate(itemData.GetSpecialItemPrefab(itemtype), player.transform).GetComponent<SpecialItem>().Initialize(player);
            Destroy(gameObject);
        }
    }
}
