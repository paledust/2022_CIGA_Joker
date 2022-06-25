using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TrickOrTreat/PlayerSprite_SO")]
public class PlayerSprite_SO : ScriptableObject
{
    [SerializeField] private List<PlayerSpriteData> spriteData;
    public Sprite getFacingSprite(PLAYER_SPRITE_STATE state){
        return spriteData.Find(x=>x.state == state).sprite;
    }
}
[System.Serializable]
public class PlayerSpriteData{
    public Sprite sprite;
    public PLAYER_SPRITE_STATE state;
}
