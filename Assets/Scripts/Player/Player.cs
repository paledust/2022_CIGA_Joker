using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public int PlayerIndex = 1;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float detectRange = 0.2f;
    [SerializeField] public int coinAmount = 3;
    [SerializeField] public int bombAmount = 3;
    [SerializeField] private CircleCollider2D m_collider;
    private Vector2 direction;
    private Vector3 facingDirection = Vector3.up;
    private Rigidbody2D m_rigid;
    private Ray2D ray;
    private RaycastHit2D hit;
    void Awake(){
        m_collider = GetComponent<CircleCollider2D>();
        m_rigid = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate(){
        m_rigid.position += direction * moveSpeed * Time.fixedDeltaTime;
    }
    void OnMove(InputValue value){
        direction = value.Get<Vector2>();
        if(direction != Vector2.zero) facingDirection = direction;
    }
    void OnPutInCoin(){
        //To Do:检测有无可以放入的对象
        InteractableObject interactableObj = DetectInteractable(new Ray2D(transform.position+facingDirection*(m_collider.radius+0.01f), facingDirection));
        if(interactableObj != null){
            interactableObj.OnInteract(INTERACTABLE_TYPE.PUT_IN_COIN, this);
        }
    }
    void OnPutInBomb(){
        //To Do:检测有无可以放入的对象
        InteractableObject interactableObj = DetectInteractable(new Ray2D(transform.position+facingDirection*(m_collider.radius+0.01f), facingDirection));
        if(interactableObj != null){
            interactableObj.OnInteract(INTERACTABLE_TYPE.PUT_IN_BOMB, this);
        }
    }
    void OnTakeOutStuff(InputValue value){
        //To Do:检测有无可以拿去的对象
        InteractableObject interactableObj = DetectInteractable(new Ray2D(transform.position+facingDirection*(m_collider.radius+0.01f), facingDirection));
        if(interactableObj != null){
            interactableObj.OnInteract(INTERACTABLE_TYPE.TAKE_OUT_STUFF, this);
        }
    }
    void OnAttack(InputValue value){
        //To Do:检测有无可以攻击的玩家
        Player player = DetectPlayer(new Ray2D(transform.position+facingDirection*(m_collider.radius+0.01f), facingDirection));
        if(player != null){
            //To Do:开始猜拳?????
             
        }
    }
    InteractableObject DetectInteractable(Ray2D ray){
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, detectRange);
        if(hit.collider!=null && hit.collider.GetComponent<InteractableObject>()){
            return hit.collider.GetComponent<InteractableObject>();
        }
        return null;
    }
    Player DetectPlayer(Ray2D ray){
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, detectRange);
        if(hit.collider!=null && hit.collider.GetComponent<Player>()){
            return hit.collider.GetComponent<Player>();
        }
        return null;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position+facingDirection*(m_collider.radius+0.01f), facingDirection*detectRange);
    }
}
