using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public int PlayerIndex = 1;
[Header("Basic Player Control")]
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float detectRange = 0.2f;
    [SerializeField] private PlayerSprite_SO playerSpriteData;
    [SerializeField] private SpriteRenderer playerRender;
[Header("Item")]
    public int coinAmount = 3;
    public int bombAmount = 3;
    [SerializeField] private Animation m_itemAmountAnimation;
    [SerializeField] private TextMesh m_itemAmountText;
[Header("Physics")]
    [SerializeField] private CircleCollider2D m_collider;
[Header("Rock Paper Sissor")]
    [SerializeField] private RPS_SO rpsData;
    [SerializeField] private SpriteRenderer rpsRenderer;
    public RPS_CHOISE rpsChoise = RPS_CHOISE.ROCK;
    private PlayerInput input; 
    private Vector2 direction;
    private Vector3 facingDirection = Vector3.up;
    private Rigidbody2D m_rigid;
    private Ray2D ray;
    private RaycastHit2D hit;
    private float horizontalInput;
    private float verticalInput;
#region UNITY事件
    void Awake(){
        m_collider  = GetComponent<CircleCollider2D>();
        m_rigid     = GetComponent<Rigidbody2D>();
        input       = GetComponent<PlayerInput>();
    }
    void FixedUpdate(){
        m_rigid.position += direction * moveSpeed * Time.fixedDeltaTime;
    }
    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position+facingDirection*(m_collider.radius+0.01f), facingDirection*detectRange);
    }
#endregion

#region 玩家操作
    void OnHorizontal(InputValue value){
        //侦测到有按键输入
        horizontalInput = value.Get<float>();
        if(horizontalInput != 0){
            direction = Vector2.zero;
            direction.x = horizontalInput;
            playerRender.sprite = playerSpriteData.getFacingSprite(FACING_DIRECTION.RIGHT);
            playerRender.flipX  = direction.x<0;
        }
        else{
            direction.x = 0;
            if(verticalInput!=0){
                direction.y = verticalInput;
                playerRender.sprite = playerSpriteData.getFacingSprite(direction.y>0?FACING_DIRECTION.UP:FACING_DIRECTION.DOWN);
            }
        }
    }
    void OnVertical(InputValue value){
        verticalInput = value.Get<float>();
        if(verticalInput != 0){
            direction = Vector2.zero;
            direction.y = verticalInput;   
            playerRender.sprite = playerSpriteData.getFacingSprite(direction.y>0?FACING_DIRECTION.UP:FACING_DIRECTION.DOWN);
        }
        else{
            direction.y = 0;
            if(horizontalInput!=0){
                direction.x = horizontalInput;
                playerRender.sprite = playerSpriteData.getFacingSprite(FACING_DIRECTION.RIGHT);
                playerRender.flipX  = direction.x<0;
            }
        }
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
            EventHandler.Call_OnEnterRPSMode();
            player.EnterRPSMode();
            EnterRPSMode();
        }
    }
    void OnRock(){
        rpsChoise = RPS_CHOISE.ROCK;
        rpsRenderer.sprite = rpsData.GetRPSSprite(rpsChoise);
    }
    void OnPaper(){
        rpsChoise = RPS_CHOISE.PAPER;
        rpsRenderer.sprite = rpsData.GetRPSSprite(rpsChoise);
    }
    void OnSissor(){
        rpsChoise = RPS_CHOISE.SISSOR;
        rpsRenderer.sprite = rpsData.GetRPSSprite(rpsChoise);
    }
#endregion

// 进入猜拳模式(Rock, Paper, Sissor)
    public void EnterRPSMode(){
        //停止角色的移动
        direction = Vector2.zero;

        rpsChoise = RPS_CHOISE.ROCK;
        rpsRenderer.gameObject.SetActive(true);
        rpsRenderer.sprite = rpsData.GetRPSSprite(rpsChoise);
        input.SwitchCurrentActionMap("RPS");
    }
// 退出猜拳模式(Rock, Paper, Sissor)
    public void ExitRPSMode(){
        input.SwitchCurrentActionMap("Player");
        rpsRenderer.gameObject.SetActive(false);
    }
    public void PauseInput()=>input.enabled = false;
    public void ResumeInput()=>input.enabled = true;
    public void GetBombed(){

    }
    public void MinusOneCoin(){if(coinAmount > 0) coinAmount --;}
    public void GetCoins(int amount){
        coinAmount += amount;
        m_itemAmountText.text = $"+{amount}";
        m_itemAmountAnimation.Play();
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
}
