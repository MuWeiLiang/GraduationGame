using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimplePlayerController : MonoBehaviour
{
    public float movePower = 10f;
    public float jumpPower = 15f; //Set Gravity Scale in Rigidbody2D Component to 5
    public int maxJumps = 2; // 最大跳跃次数
    private int jumpsRemaining; // 剩余的跳跃次数
    public LayerMask Ground;
    public Transform foot;

    private Rigidbody2D rb;
    private Animator anim;
    private int direction = 1;
    bool isJumping = false;
    private bool isGrounded = false;
    public bool alive = true;
    private float localScaleNum;
    private bool isDie = false;
    public float climbSpeed = 5f; // 爬梯速度
    public bool isClimbing = false; // 是否正在爬梯子


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        RestorePhysics();
        localScaleNum = transform.localScale.x;
        jumpsRemaining = maxJumps;
    }

    private void Update()
    {
        Restart();
        if (alive)
        {
            IsGrounded();
            Hurt();
            Die();
            Attack();
            Climb();
            Jump();
            Run();
        }
        else
        {
            if (!isDie)
            {
                Die();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0; // 禁用重力，防止玩家下落
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = 5; // 恢复重力
        }
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
            if(!isJumping && anim.GetBool("isJump"))
            {
                anim.SetBool("isJump", false);
            }
        }
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetTrigger("attack");
        }
    }
    void Hurt()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetTrigger("hurt");
            if (direction == 1)
                rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
            else
                rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
        }
    }
    public void Die(bool flag = false)
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) || !alive || flag)
        {
            anim.SetTrigger("die");
            alive = false;
            isDie = true;

            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.isKinematic = true;
            Invoke("RestartLevel", 1f);
        }
    }
    void Restart()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            anim.SetTrigger("idle");
            alive = true;
        }
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void RestorePhysics()
    {
        rb.gravityScale = 5;       // 恢复重力
        rb.isKinematic = false;    // 恢复为动力学刚体
    }
}