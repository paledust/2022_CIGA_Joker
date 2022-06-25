using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TrickOrTreat/SpecialItem_SO")]
public class SpecialItem_SO : ScriptableObject
{
    [SerializeField] List<SpecialItem_Data> specialItemdatas;
    public GameObject GetSpecialItemPrefab(SPECIAL_ITEM_TYPE itemType){
        return specialItemdatas.Find(x=>x.itemType==itemType).itemPrefab;
    }
}
[System.Serializable]
public class SpecialItem_Data{
    public SPECIAL_ITEM_TYPE itemType;
    public GameObject itemPrefab;
}
