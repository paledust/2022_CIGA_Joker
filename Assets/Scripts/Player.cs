using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 1;
    private Vector3 direction;
    void Update(){
        transform.position += direction * MoveSpeed * Time.deltaTime;
    }
    void OnMove(InputValue value){
        direction = value.Get<Vector2>();
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
}
