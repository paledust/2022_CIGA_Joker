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
    private float SpeedMultiplier = 1;
    private bool Inverse = false;
[Header("Bomb")]
    [SerializeField] private float bombBlinkTime = 3;
    [SerializeField] private float bombBlinkFreq = 5;
[Header("Item")]
    [SerializeField] private int coinAmount = 3;
    public int bombAmount = 3;
    [SerializeField] private SpecialItem currentItem;
    [SerializeField] private Animation m_itemAmountAnimation;
    [SerializeField] private TextMesh m_itemAmountText;
[Header("Physics")]
    [SerializeField] private CircleCollider2D m_collider;
[Header("Poisned")]
    [SerializeField] private float RecoverDelay = 3;
    [SerializeField] private float PoisonDropCoinRate = 5;
[Header("Damage Transfer")]
    public bool CanTransferDamage = false;
[Header("Rock Paper Sissor")]
    [SerializeField] private RPS_SO rpsData;
    [SerializeField] private SpriteRenderer rpsRenderer;
    [SerializeField] private Animation hitFeedback;
    public RPS_CHOISE rpsChoise = RPS_CHOISE.ROCK;
    public int CoinAmount{get{return coinAmount;}}
    public bool invincible{get; private set;} = false;
    private PlayerInput input; 
    private Vector2 direction;
    private Vector3 facingDirection = Vector3.up;
    private Rigidbody2D m_rigid;
    private float horizontalInput;
    private float verticalInput;
    private bool Poisoned = false;
    private float posionTimer = 0;
    IEnumerator coroutineRecovered;
#region UNITY事件
    void Awake(){
        m_collider  = GetComponent<CircleCollider2D>();
        m_rigid     = GetComponent<Rigidbody2D>();
        input       = GetComponent<PlayerInput>();
    }
    void Update(){
        if(Poisoned){
            posionTimer += Time.deltaTime * PoisonDropCoinRate;
            if(posionTimer >= 1){
                posionTimer = 0;
                DamageTest();
            }
        }
    }
    void FixedUpdate(){
        m_rigid.position += direction * moveSpeed * Time.fixedDeltaTime * SpeedMultiplier * (Inverse?-1:1);
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
            playerRender.sprite = playerSpriteData.getFacingSprite(PLAYER_SPRITE_STATE.RIGHT);
            playerRender.flipX  = direction.x<0;

            facingDirection = direction;
        }
        else{
            direction.x = 0;
            if(verticalInput!=0){
                direction.y = verticalInput;
                playerRender.sprite = playerSpriteData.getFacingSprite(direction.y>0?PLAYER_SPRITE_STATE.UP:PLAYER_SPRITE_STATE.DOWN);

                facingDirection = direction;
            }
        }
    }
    void OnVertical(InputValue value){
        verticalInput = value.Get<float>();
        if(verticalInput != 0){
            direction = Vector2.zero;
            direction.y = verticalInput;   
            playerRender.sprite = playerSpriteData.getFacingSprite(direction.y>0?PLAYER_SPRITE_STATE.UP:PLAYER_SPRITE_STATE.DOWN);

            facingDirection = direction;
        }
        else{
            direction.y = 0;
            if(horizontalInput!=0){
                direction.x = horizontalInput;
                playerRender.sprite = playerSpriteData.getFacingSprite(PLAYER_SPRITE_STATE.RIGHT);
                playerRender.flipX  = direction.x<0;

                facingDirection = direction;
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
        //To Do:检测有无可以划拳的玩家
        Ray2D ray = new Ray2D(transform.position+facingDirection*(m_collider.radius+0.01f), facingDirection);

        Player player = DetectPlayer(ray);
        IRPSable obj = DetectRPSable(ray);
        if(player != null){
            //To Do:开始猜拳?????
            EventHandler.Call_OnEnterRPSMode();
            player.EnterRPSMode();
            EnterRPSMode();
        }
        else if(obj!=null){
            EventHandler.Call_OnEnterRPSMode_PVE(this, obj);
            EnterRPSMode();
            obj.EnterRPSMode();
        }
    }
    void OnRock(){
        rpsChoise = RPS_CHOISE.ROCK;
        hitFeedback.Play();
    }
    void OnPaper(){
        rpsChoise = RPS_CHOISE.PAPER;
        hitFeedback.Play();
    }
    void OnSissor(){
        rpsChoise = RPS_CHOISE.SISSOR;
        hitFeedback.Play();
    }
#endregion
    public void ShowRPSResult(){
        rpsRenderer.GetComponent<Animator>().enabled = false;
        rpsRenderer.sprite = rpsData.GetRPSSprite(rpsChoise);
    }
    public Sprite GetStateSprite(PLAYER_SPRITE_STATE spriteState){
        return playerSpriteData.getFacingSprite(spriteState);
    }
// 进入猜拳模式(Rock, Paper, Sissor)
    public void EnterRPSMode(){
        //停止角色的移动
        direction = Vector2.zero;

        rpsChoise = RPS_CHOISE.ROCK;
        rpsRenderer.gameObject.SetActive(true);
        rpsRenderer.GetComponent<Animator>().enabled = true;
        rpsRenderer.GetComponent<Animator>().Play("RPS", 0, Random.Range(0f,1f));
        input.SwitchCurrentActionMap("RPS");
    }
// 退出猜拳模式(Rock, Paper, Sissor)
    public void ExitRPSMode(){
        input.SwitchCurrentActionMap("Player");
        rpsRenderer.gameObject.SetActive(false);
    }
    public void PauseInput()=>input.enabled = false;
    public void ResumeInput()=>input.enabled = true;
    public void BeInvinsible(){invincible = true;}
    public void NotBeInvincible(){invincible = false;}
    public void InverseControl(){Inverse = true;}
    public void UnInverseControl(){Inverse = false;}
    public void OnGetSpecialItem(SpecialItem item){
        if(currentItem!=null){
            Destroy(currentItem.gameObject);
            currentItem = null;
        }

        currentItem = item;
    }
    public void GetPoisoned(){
        if(invincible) {
            Debug.Log($"玩家{PlayerIndex+1}当前无敌");
            return;
        }
        DamageTest();
        Poisoned = true;
        float alpha = playerRender.color.a;
        Color color = Color.green;
        color.a = alpha;

        playerRender.color = color;
        if(coroutineRecovered!=null){
            StopCoroutine(coroutineRecovered);
        }
    }
    public void Recovered(){
        if(Poisoned){
            coroutineRecovered = CoroutineRecover();
            StartCoroutine(coroutineRecovered);
        }
    }
    public void DamageTest(){
        if(CanTransferDamage){
            Debug.Log($"玩家{PlayerIndex+1}转移伤害给了另一名玩家");
            EventHandler.Call_OnTransferDamage(1, this);
            return;
        }
        LoseCoin();
    }
    public void LoseRPSTest(){
        if(CanTransferDamage){
            Debug.Log($"玩家{PlayerIndex+1}转移伤害给了另一名玩家");
            EventHandler.Call_OnTransferDamage(coinAmount, this);
            return;  
        }
        LooseAllCoins();
    }
    public void LoseCoin(){
        if(invincible){
            Debug.Log($"玩家{PlayerIndex+1}当前无敌");
            return;
        }
        MinusOneCoin();
        StartCoroutine(coroutineBlink());
    }
    public void LoseCoins(int damage){
        if(invincible){
            Debug.Log($"玩家{PlayerIndex+1}当前无敌");
            return;
        }
        coinAmount -= damage;
        coinAmount = Mathf.Max(0, coinAmount);
        StartCoroutine(coroutineBlink());
    }
    public void LooseAllCoins(){
        LoseCoins(coinAmount);
    }
    public void MinusOneCoin(){
        if(coinAmount > 0) coinAmount --;
    }
    public void ChangeSpeedScale(float increaseScale){
        SpeedMultiplier = increaseScale;
    }
    IEnumerator CoroutineRecover(){
        yield return new WaitForSeconds(RecoverDelay);
        playerRender.color = Color.white;
        Poisoned = false;
    }
    IEnumerator coroutineBlink(){
        for(float t=0;t<1;t+=Time.deltaTime/bombBlinkTime){
            playerRender.enabled = Mathf.Sin(t*bombBlinkFreq*2*Mathf.PI*bombBlinkTime)>0;
            yield return null;
        }
        playerRender.enabled = true;
    }

    public void GetCoins(int amount){
        coinAmount += amount;
        m_itemAmountText.text = $"+{amount}";
        m_itemAmountAnimation.Play();
    }
    IRPSable DetectRPSable(Ray2D ray){
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, detectRange);
        if(hit.collider!=null && hit.collider.GetComponent<IRPSable>()!=null){
            return hit.collider.GetComponent<IRPSable>();
        }
        return null;        
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
