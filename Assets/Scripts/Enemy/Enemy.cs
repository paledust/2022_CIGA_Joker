using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    private Vector2 moveDir;
    private float timer = 3f;
    private Rigidbody2D rb;
    [SerializeField] private Transform transformLeftUp, transformRightDown;
    private Vector2 posLeftUp, posRightDown;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        posLeftUp = transformLeftUp.position;
        posRightDown = transformRightDown.position;
    }
    private void FixedUpdate() {
        timer += Time.fixedDeltaTime;
        if (timer > 3f)
        {            
            moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rb.velocity = moveDir.normalized * moveSpeed;
            
            timer = 0f;
        }
        
        if (!JudgeLeft())
        {
            rb.velocity = new Vector2(Mathf.Abs(rb.velocity.x), rb.velocity.y);
        }
        if (!JudgeRight())
        {
            rb.velocity = new Vector2(-Mathf.Abs(rb.velocity.x), rb.velocity.y);
        }
        if (!JudgeUp())
        {
            rb.velocity = new Vector2(rb.velocity.x, -Mathf.Abs(rb.velocity.y));
        }
        if (!JudgeDown())
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(rb.velocity.y));
        }
        if (Mathf.Abs(rb.velocity.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
        }
    }
    private bool JudgeLeft()
    {
        return transform.position.x > posLeftUp.x;
    }
    private bool JudgeRight()
    {
        return transform.position.x < posRightDown.x;
    }
    private bool JudgeUp()
    {
        return transform.position.y < posLeftUp.y;
    }
    private bool JudgeDown()
    {
        return transform.position.y > posRightDown.y;
    }
}
