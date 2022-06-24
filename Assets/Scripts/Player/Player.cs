using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 1;
    [SerializeField] private float detectRange = 0.2f;
    private Vector2 direction;
    private Vector3 facingDirection = Vector3.up;
    private Rigidbody2D m_rigid;
    private Ray2D ray;
    void Awake(){
        m_rigid = GetComponent<Rigidbody2D>();
    }
    void Update(){
    }
    void FixedUpdate(){
        m_rigid.position += direction * MoveSpeed * Time.fixedDeltaTime;
    }
    void OnMove(InputValue value){
        direction = value.Get<Vector2>();
        if(direction != Vector2.zero) facingDirection = direction;
    }
    void OnPutIn(InputValue value){
        //To Do:检测有无可以放入的对象
        Debug.Log("Put In Stuff");
    }
    void OnTakeOut(InputValue value){
        //To Do:检测有无可以拿去的对象
        Debug.Log("Take Out Stuff");
    }
    void OnAttack(InputValue value){
        //To Do:检测有无可以攻击的对象
        Debug.Log("Attack");
    }
    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, facingDirection*detectRange);
    }
}
