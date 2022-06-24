using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private Collider2D m_collider;
    void Awake()=>m_collider = GetComponent<Collider2D>();
    public virtual void OnInteract(INTERACTABLE_TYPE interactableType, Player interactPlayer){}
    public void DisableTrigger(){m_collider.enabled = false;}
    public void EnableTrigger(){m_collider.enabled = true;}
}
