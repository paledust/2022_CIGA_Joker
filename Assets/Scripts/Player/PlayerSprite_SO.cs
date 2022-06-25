using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TrickOrTreat/PlayerSprite_SO")]
public class PlayerSprite_SO : ScriptableObject
{
    [SerializeField] private List<PlayerSpriteData> spriteData;
    public Sprite getFacingSprite(FACING_DIRECTION direction){
        return spriteData.Find(x=>x.direction == direction).sprite;
    }
}
[System.Serializable]
public class PlayerSpriteData{
    public Sprite sprite;
    public FACING_DIRECTION direction;
}
