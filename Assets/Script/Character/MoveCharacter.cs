using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCharacter : BaseCharacter
{
    // �˶��Ͷ���
    private Rigidbody2D rb;
    private Animator anim;
    // �ƶ����
    public float movePower = 10f;
    public int direction = 1;
    bool isRunning = false;
    // ��Ծ���
    public float jumpPower = 15f;
    public int maxJumps = 2; // �����Ծ����
    private int jumpsRemaining; // ʣ�����Ծ����
    bool isJumping = false;
    private bool isGrounded = false;
    public LayerMask Ground;
    public Transform foot;

    private bool ExecuteDie = false;
    // �������
    public float climbSpeed = 5f; // �����ٶ�
    public bool isClimbing = false; // �Ƿ�����������
    // �������
    public float flyPower = 5f;
    public bool isFlying = false;
    // һЩ����
    private float localScaleNum;
    // ״̬���
    private bool canMove;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        RestorePhysics();
        localScaleNum = transform.localScale.x;
        jumpsRemaining = maxJumps;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            Restart();
        if(health <= 0) alive = false;
        if (alive)
        {
            if(!canMove) return;
            if (Input.GetKeyDown(KeyCode.H))
            {
                UpdateFly();
                Restart();
            }
            if (isFlying)
            {
                Fly();
            }
            if (!isFlying)
                MoveUpdate();
        }
        else
        {
            if (!ExecuteDie)
                Die();
        }
    }
    void MoveUpdate()
    {
        IsGrounded();
        Climb();
        Jump();
        Run();
    }
    void Restart()
    {
        anim.SetTrigger("idle");
        alive = true;
    }
    void IsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(foot.position, 0.1f, Ground);
    }
    public void ReversalDir() {
        direction = -direction;
        transform.localScale = new Vector3(direction * localScaleNum, localScaleNum, localScaleNum);
    }
    void Run()
    {
        Vector3 moveVelocity = Vector3.zero;
        anim.SetBool("isRun", false);

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            direction = -1;
            moveVelocity = Vector3.left;

            transform.localScale = new Vector3(direction * localScaleNum, localScaleNum, localScaleNum);
            if (!anim.GetBool("isJump") && !isClimbing)
                anim.SetBool("isRun", true);

        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            direction = 1;
            moveVelocity = Vector3.right;

            transform.localScale = new Vector3(direction * localScaleNum, localScaleNum, localScaleNum);
            if (!anim.GetBool("isJump") && !isClimbing)
                anim.SetBool("isRun", true);

        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }
    void Jump()
    {
        if (isGrounded)
        {
            jumpsRemaining = maxJumps;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (jumpsRemaining > 0)
            {
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
        }
    }

    void Fly()
    {
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
        transform.localScale = new Vector3(direction * localScaleNum, localScaleNum, localScaleNum);
        transform.position += moveVelocity * movePower * Time.deltaTime;
        transform.position += flyVelocity * flyPower * Time.deltaTime;
    }
    public void UpdateFly()
    {
        rb.gravityScale = isFlying ? 5 : 0;
        isFlying = !isFlying;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0; // ������������ֹ�������
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = 5; // �ָ�����
        }
    }
    void Climb()
    {
        if (isClimbing)
        {
            anim.SetBool("isJump", true);
            float verticalInput = Input.GetAxis("Vertical");
            if (Mathf.Abs(verticalInput) > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed);
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
    void RestorePhysics()
    {
        rb.gravityScale = 5;       // �ָ�����
        rb.isKinematic = false;    // �ָ�Ϊ����ѧ����
    }
    public override void Die()
    {
        if (ExecuteDie) return;

        anim.SetTrigger("die");
        alive = false;
        ExecuteDie = true;

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        rb.isKinematic = true;
        if (CanResurrect()) {
            UpdateResurrectNum(-1);
            Invoke("RestartLevel", 1f);            
        }
        
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
