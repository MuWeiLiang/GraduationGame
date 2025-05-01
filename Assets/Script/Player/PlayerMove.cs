using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SPlayerController;

public class PlayerMove
{
    PlayerSoundEvent onPlayer;
    // 运动和动画
    private Rigidbody2D rb;
    private Animator anim;
    private Transform selfTransform;
    private PlayerBase playerBase;
    // 移动相关
    public float movePower = 8f;
    public int direction = 1;
    private bool isRunning = false;
    // 跳跃相关
    private float jumpPower = 15f;
    private int maxJumps = 2; // 最大跳跃次数
    private int jumpsRemaining; // 剩余的跳跃次数
    bool isJumping = false;
    private bool isGrounded = false;
    private LayerMask Ground;
    private Transform foot;
    // 爬梯相关
    private float climbSpeed = 5f; // 爬梯速度
    private bool isClimbing = false; // 是否正在爬梯子
    private float lastClimbTime = -0.5f;
    // 飞行相关
    private float flyPower = 5f;
    private bool isFlying = false;
    private float flyTime = 0f;
    // 一些常数
    private float localScaleNum;
    public void Initialize(Rigidbody2D RB, Animator Anim, Transform Tran, PlayerSoundEvent onPlayer, PlayerBase playerbase)
    {
        // Initialize player movement settings here
        // Debug.Log("PlayerMove initialized");
        rb = RB;
        anim = Anim;
        selfTransform = Tran;
        this.onPlayer = onPlayer;
        this.playerBase = playerbase;
        RestorePhysics();
        localScaleNum = selfTransform.localScale.x;
        jumpsRemaining = maxJumps;
        int groundLayer = LayerMask.NameToLayer("Ground");
        Ground = 1 << groundLayer;
        if (Ground == 0)
        {
            Debug.Log("Ground Layer not found! Make sure it's set in the Inspector.");
        }
        foot = selfTransform.Find("foot");
        if (foot == null)
        {
            Debug.Log("Foot not found! Make sure it's a child of the player object.");
        }
    }
    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            Restart();
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    UpdateFly();
        //    Restart();
        //}
        if (isFlying)
        {
            Fly();
            if (isFlying)
            {
                flyTime += Time.deltaTime;
                if (flyTime >= 1f)
                {
                    flyTime -= 1f;
                    playerBase.UpdateMana(-1);
                }
            }
        }
        else
        {
            MoveUpdate();
        }
    }
    void MoveUpdate()
    {
        IsGrounded();
        Climb();
        Jump();
        Run();
    }
    public void Restart()
    {
        anim.SetTrigger("idle");
    }
    void IsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(foot.position, 0.1f, Ground);
    }
    void Run()
    {
        Vector3 moveVelocity = Vector3.zero;
        anim.SetBool("isRun", false);
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            direction = -1;
            moveVelocity = Vector3.left;

            selfTransform.localScale = new Vector3(direction * localScaleNum, localScaleNum, localScaleNum);
            if (!anim.GetBool("isJump") && !isClimbing)
                anim.SetBool("isRun", true);
            if (!isRunning && !isClimbing && !isJumping)
            {
                // onPlayer?.Invoke("run");
                isRunning = true;
            }
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            direction = 1;
            moveVelocity = Vector3.right;

            selfTransform.localScale = new Vector3(direction * localScaleNum, localScaleNum, localScaleNum);
            if (!anim.GetBool("isJump") && !isClimbing)
                anim.SetBool("isRun", true);
            if (!isRunning && !isClimbing && !isJumping)
            {
                // onPlayer?.Invoke("run");
                isRunning = true;
            }

        }
        if (moveVelocity == Vector3.zero && isRunning)
        {
            isRunning = false;
            // onPlayer?.Invoke("stop");
        }
        selfTransform.position += moveVelocity * movePower * Time.deltaTime;
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isRunning = false;
            onPlayer?.Invoke("stop");
            if (isGrounded)
            {
                jumpsRemaining = maxJumps;
            }
            if (jumpsRemaining > 0)
            {
                // Debug.Log("jumpsRemaining = " + jumpsRemaining + ", isGrounded = " + isGrounded);
                onPlayer?.Invoke("jump");
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                anim.SetBool("isJump", true);
                jumpsRemaining--;
            }
        }
        if (isGrounded && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            isJumping = false;
            anim.SetBool("isJump", false);
            jumpsRemaining = maxJumps;
        }
    }

    void Fly()
    {
        if (!playerBase.IsManaEnough(1))
        {
            UpdateFly();
            return;
        }
        Vector3 moveVelocity = Vector3.zero;
        Vector3 flyVelocity = Vector3.zero;
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            direction = -1;
            moveVelocity = Vector3.left;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            direction = 1;
            moveVelocity = Vector3.right;
        }
        if (Input.GetAxis("Vertical") > 0)
            flyVelocity = Vector3.up;
        if (Input.GetAxis("Vertical") < 0)
            flyVelocity = Vector3.down;
        selfTransform.localScale = new Vector3(direction * localScaleNum, localScaleNum, localScaleNum);
        selfTransform.position += moveVelocity * movePower * Time.deltaTime;
        selfTransform.position += flyVelocity * flyPower * Time.deltaTime;
    }
    public void UpdateFly()
    {
        if (!playerBase.IsManaEnough(1) && !isFlying) return;
        rb.gravityScale = isFlying ? 5 : 0;
        isFlying = !isFlying;
        if (isFlying)
        {
            Restart();
            anim.SetBool("isJump", true);
            onPlayer?.Invoke("fly");
        }
        else
        {
            anim.SetBool("isJump", false);
            onPlayer?.Invoke("stop");
        }

    }
    public void HandleTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0; // 禁用重力，防止玩家下落
        }
    }
    public void HandleTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            if (!isFlying)
                rb.gravityScale = 5; // 恢复重力
        }
    }
    void Climb()
    {
        if (isClimbing)
        {
            isRunning = false;
            anim.SetBool("isJump", true);
            float verticalInput = Input.GetAxis("Vertical");
            if (Mathf.Abs(verticalInput) > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed);
                if (Time.time - lastClimbTime >= 0.5f)
                {
                    onPlayer?.Invoke("climb");
                    lastClimbTime = Time.time; // 更新最后一次爬梯时间
                }
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
        else
        {
            if (!isJumping && anim.GetBool("isJump"))
            {
                anim.SetBool("isJump", false);
            }
        }
    }
    public void RestorePhysics()
    {
        rb.gravityScale = 5;       // 恢复重力
        rb.isKinematic = false;    // 恢复为动力学刚体
    }

    public bool GetIsFlying()
    {
        return isFlying;
    }
    public Transform GetTransform()
    {
        return selfTransform;
    }
}
