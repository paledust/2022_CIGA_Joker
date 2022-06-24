using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : InteractableObject
{
    [SerializeField] private int ContainCoin;
    [SerializeField] private int ContainBomb;
    public override void OnInteract(INTERACTABLE_TYPE interactableType)
    {
        base.OnInteract(interactableType);
        switch(interactableType){
            case INTERACTABLE_TYPE.PUT_IN:
                
                break;
            case INTERACTABLE_TYPE.TAKE_OUT:

                break;
        }
    }
}
