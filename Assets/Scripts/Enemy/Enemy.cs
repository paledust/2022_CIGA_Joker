using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    private Vector2 moveDir;
    private bool goal;
    private Vector2 moveGoal;
    private float timer = 3f;
    private float catchCoroutineTimer = 0f;
    private Rigidbody2D rb;
    [SerializeField] private Transform transformLeftUp, transformRightDown;
    private Vector2 posLeftUp, posRightDown;
    private Coroutine catchCoroutine;
    private bool catchPlayer;
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
        
        if (goal)
        {
            moveDir = new Vector2(moveGoal.x - transform.position.x, moveGoal.y - transform.position.y);
            rb.velocity = moveDir.normalized * moveSpeed;
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
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            if (catchPlayer)    return;
            catchPlayer = true;
            Player player = other.gameObject.GetComponent<Player>();
            if (catchCoroutine != null)
            {
                StopCoroutine(catchCoroutine);
            }
            catchCoroutine = StartCoroutine(Catch(player));
        }
    }
    IEnumerator Catch(Player player)
    {
        Rigidbody2D playerRb = player.gameObject.GetComponent<Rigidbody2D>();
        CircleCollider2D playerColl = player.gameObject.GetComponent<CircleCollider2D>();
        Vector3 enemyPos = transform.position;
        Vector3 playerPos = player.transform.position;
        SpriteRenderer spriteRenderer = player.transform.Find("PlayerImage").GetComponent<SpriteRenderer>();
        player.ChangeSpeedScale(0f);
        playerColl.enabled = false;
        spriteRenderer.sortingLayerName = "default";

        catchCoroutineTimer = 0f;
        while (catchCoroutineTimer < 5f)
        {
            playerRb.velocity = rb.velocity;
            catchCoroutineTimer += Time.fixedDeltaTime;
            print(catchCoroutineTimer);
            yield return new WaitForFixedUpdate();
        }

        moveGoal = enemyPos;
        goal = true;
        while (Mathf.Sqrt(Mathf.Pow(transform.position.x - moveGoal.x, 2) + Mathf.Pow(transform.position.y - moveGoal.y, 2)) > 0.1f)
        {
            playerRb.velocity = rb.velocity;
            yield return 0;
        }
        goal = false;
        moveGoal = posLeftUp;
        playerRb.velocity = Vector2.zero;
        player.transform.position = playerPos;

        yield return new WaitForSeconds(1f);

        playerColl.enabled = true;
        player.ChangeSpeedScale(1f);
        spriteRenderer.sortingLayerName = "MidLayer";

        catchPlayer = false;
    }
    private void OnTriggerExit2D(Collider2D other) {
        
    }
}
