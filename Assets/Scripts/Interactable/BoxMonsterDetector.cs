using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMonsterDetector : MonoBehaviour
{
[Header("Box Move Speed")]
    [SerializeField] private GameObject damagePlayer;
    [SerializeField] private Color infectedColor = Color.white;
    [SerializeField] private float speed = 1;
    [SerializeField] private Animator m_animator;
    [SerializeField] private string muteTrigger = "Muted";
    [SerializeField] private string calmTrigger = "Calm";
    [SerializeField] private Rigidbody2D m_rigid;
    [SerializeField] private bool IsInfected = false;
    [SerializeField] private List<Player> inRangePlayers;
    [SerializeField] private SpriteRenderer boxRenderer;
    private Player currentChasingPlayer;
    void Awake(){
        if(IsInfected){
            damagePlayer.SetActive(true);
            boxRenderer.color = infectedColor;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>()){
            if(IsInfected) m_animator.SetTrigger(muteTrigger);
            
            inRangePlayers.Add(other.GetComponent<Player>());
            if(currentChasingPlayer==null){
                currentChasingPlayer = other.GetComponent<Player>();
            }
            else{
                if(Vector3.Distance(currentChasingPlayer.transform.position, transform.position)>Vector3.Distance(other.transform.position, transform.position)){
                    currentChasingPlayer = other.GetComponent<Player>();
                }
            }
        }
        // else if(other.GetComponent<BoxMonsterDetector>() && IsInfected){
        //     other.GetComponent<BoxMonsterDetector>().GetInfected();
        // }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(IsInfected && other.GetComponent<Player>()){
            inRangePlayers.Remove(other.GetComponent<Player>());
            if(inRangePlayers.Count!=0){
                currentChasingPlayer = inRangePlayers[0];
            }
            else{
                currentChasingPlayer = null;
                m_animator.SetTrigger(calmTrigger);
            }
        }
    }
    // public void GetInfected(){
    //     if(!IsInfected) IsInfected = true;
    //     damagePlayer.SetActive(true);
    //     boxRenderer.color = infectedColor;
    // }
    void FixedUpdate(){
        if(IsInfected && currentChasingPlayer!=null){
            m_rigid.position += ((Vector2)currentChasingPlayer.transform.position - m_rigid.position).normalized * speed * Time.fixedDeltaTime;
        }
    }
}