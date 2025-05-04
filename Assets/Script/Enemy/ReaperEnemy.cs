using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperEnemy : RandomMove
{
    private Animator animator;

    private int damage = 20; // �˺�

    private bool haveWeapon = false; // �Ƿ�������
    // ������ȴʱ����ر���
    private float attackCooldown = 1f; // �������ʱ��
    private float lastAttackTime = -1f; // �ϴι���ʱ��
    // ׷�����
    public bool isChasing = false; // �Ƿ�����׷�����
    public bool playerInRange = false;
    public GameObject player; // ���λ��
    private Vector3 playerPosition; // ���λ��
    private float chaseSpeed = 3f; // ׷���ٶ�
    private float distanceToPlayer; // ���˺����֮��ľ���

    private bool isDie = false; // �Ƿ�����


    void Start()
    {
        // ��ȡAnimator���
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject.");
        }
        initialPosition = transform.position;
        StartMove();
        // player = GameObject.FindGameObjectWithTag("Player");
        health = 50; // ��ʼ��Ѫ��

        damagePopupSystem = FindObjectOfType<DamagePopupSystem>();
    }

    void Update()
    {
        if(isDie) return; // ���������ִ�к�������
        if (player != null)
        {
            playerPosition = player.transform.position; // ��ȡ���λ��
            playerPosition.y += 0.6f; // ����y��λ��
            float X = playerPosition.x - transform.position.x;
            if (Mathf.Abs(X) > 0.7f)
            {
                if (X < 0)
                    playerPosition.x += 0.7f; // ����x��λ��
                else
                    playerPosition.x -= 0.7f; // ����x��λ��
            } else
            {
                playerPosition.x = transform.position.x; // ����x��λ��
            }

            distanceToPlayer = Vector2.Distance(transform.position, playerPosition); // ������˺����֮��ľ���
        }
        ChasePlayer(); // ����Ƿ�׷�����

        if (isChasing)
        {
            MoveToPlayer();
            if (!haveWeapon && distanceToPlayer < 5f) // ������˺����֮��ľ���С��1.5f
            {
                HaveWeapon();
            }
            else if (haveWeapon && distanceToPlayer > 7f) // ������˺����֮��ľ���С��1.5f
            {
                RemoveWeapon();
            }
            if (distanceToPlayer < 1f) // ������˺����֮��ľ���С��1.5f
            {
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    Attack();
                }
            }
        }
        else
        {
            Tick();
        }
    }

    void MoveToPlayer()
    {
        if (playerPosition == null) return;

        // ���㵽��ҵķ���
        Vector2 direction = (playerPosition - transform.position).normalized;

        // �ƶ����˳������
        transform.position = Vector2.MoveTowards(
            transform.position,
            playerPosition,
            chaseSpeed * Time.deltaTime
        );

        // ���³���
        UpdateFacingDirection(playerPosition);
    }

    void ChasePlayer()
    {
        IsInRange(playerPosition.x, playerPosition.y); // �������Ƿ��ڹ�����Χ��
        if (playerInRange && !isChasing)
        {
            StartChasing();
        }
        else if (!playerInRange && isChasing)
        {
            StopChasing();
        }
    }

    void StartChasing()
    {
        isChasing = true;

        // ֹͣ����ƶ�
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        isMoving = false;
    }

    // ֹͣ׷�𣬻ָ�����ƶ�
    void StopChasing()
    {
        isChasing = false;

        // ���¿�ʼ����ƶ�
        StartMove();
    }


    void AnimTest()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                Attack();
                Debug.Log("Attack");
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            HaveWeapon();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            RemoveWeapon();
        }
    }

    void Attack()
    {
        animator.SetTrigger("attack");
    }
    void Idle()
    {
        animator.SetTrigger("idle");
    }
    void HaveWeapon()
    {
        if (haveWeapon)
        {
            return;
        }
        animator.SetBool("haveWeapon", true);
        haveWeapon = true;
        Invoke("Idle", 1f);
    }
    void RemoveWeapon()
    {
        if (!haveWeapon)
        {
            return;
        }
        animator.SetBool("haveWeapon", false);
        haveWeapon = false;
        Invoke("Idle", 0.5f);
    }

    void IsInRange(float x, float y)
    {
        if (x >= initialPosition.x - xOffset && x <= initialPosition.x + xOffset &&
           y >= initialPosition.y - yOffset && y <= initialPosition.y + yOffset)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        Hurt();
    }
    void Hurt()
    {
        animator.SetTrigger("hurt");
    }
    public override void Die()
    {
        // animator.SetTrigger("death");
        isDie = true;
        Destroy(gameObject, 0.5f);
    }
}
