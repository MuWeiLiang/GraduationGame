using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1 : BaseEnemy
{
    private int maxHealth = 300;
    private int maxMana = 100;
    public int AttackDamage = 20;
    [Header("�ƶ�����")]
    [SerializeField] private float moveSpeed = 3f; // �ƶ��ٶ�
    [SerializeField] private float stoppingDistance = 2f; // ֹͣ����

    private float baseMoveSpeed = 3f; // �����ƶ��ٶ�
    private float bonusMoveSpeed = 0f; // �����ƶ��ٶ�
    public float DistanceToPlayer { get; private set; } // ��Ҿ���

    private float detectionRange = 50f; // ���޾���
    private float attackDistance = 2f; // ��������
    private float minSafeDistance = 1f; // ��ȫ����
    private bool attackCooldown = false; // ������ȴ
    private bool isAttacking = false;
    private bool canAttack = true;
    private float YOffset = 0.3f; // Y��ƫ����
    public Transform playerTarget; // ���Ŀ��
    private Rigidbody2D rb; // �������
    private Animator animator; // ���ڿ��ƶ���
    private SPlayerController playerController; // ״̬������

    void Start()
    {
        // ���Ҵ���"Player"��ǩ�����
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTarget = player.transform;
            playerController = player.GetComponent<SPlayerController>();
        }
        else
        {
            Debug.LogWarning("������û���ҵ�����'Player'��ǩ�Ķ���");
        }

        // ��ȡ�������
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = maxHealth; // ����Ѫ��
        mana = maxMana; // ��������
    }

    void Update()
    {
        moveSpeed = baseMoveSpeed + bonusMoveSpeed; // �����ƶ��ٶ�
        // Debug.Log("moveSpeed: " + moveSpeed);
        if (playerTarget == null) return;
        if (playerController.IsCloaked()) return;
        Vector3 direction = playerTarget.position - transform.position;
        direction.y += 0.5f; // ����y��߶� boss
        direction.y += 1f; // ����y��߶� ���
        // Debug.Log("direction " + direction);
        float distanceToPlayer = direction.magnitude;
        DistanceToPlayer = distanceToPlayer; // ������Ҿ���
        if (distanceToPlayer > detectionRange)
        {
            // �����ҳ�����ⷶΧ��ֹͣ�ƶ�
            rb.velocity = Vector3.zero;
            return;
        }
        if (isAttacking) return;
        // ƽ��ת�����
        if (direction.x * transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        if (distanceToPlayer <= attackDistance) 
        {
            if (distanceToPlayer < minSafeDistance)
            {
                direction.x = 1f;
                MoveToTarget(direction);
            } 
            if(Mathf.Abs(direction.y) >= YOffset)
            {
                direction.x = 0;
                MoveToTarget(direction);
            }
            Attack();         
        } 
        else
        {
            ResetAnimation();
            MoveToTarget(direction);
        }       
    }

    void MoveToTarget(Vector3 direction)
    {        
        if (true)
        {
            Vector3 movement = direction.normalized * moveSpeed * Time.deltaTime;
            transform.position += movement;
        }
    }
    void Attack()
    {
        if (attackCooldown)
            return;
        attackCooldown = true;
        // Debug.Log("��������");
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        Invoke("ResetIsAttack", 0.667f); // ������������ʱ��
        StartCoroutine(AttackCooldown());
    }
    private void ResetIsAttack()
    {
        isAttacking = false;
    }
    IEnumerator AttackCooldown()
    {
        // �ȴ��������ȴʱ��
        yield return new WaitForSeconds(1f);

        // ������ȴ״̬
        attackCooldown = false;
    }
    void ResetAnimation()
    {
        animator.SetBool("isAttacking", false);
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!canAttack) return;
    //    int direction = transform.localScale.x > 0 ? 1 : -1;
    //    if (collision.CompareTag("Player"))
    //    {
    //        collision.GetComponent<SPlayerController>().GetHurt(direction, AttackDamage);
    //        canAttack = false;
    //        Invoke(nameof(ResetAttack), 0.667f);
    //    }
    //}
    //private void ResetAttack()
    //{
    //    canAttack = true;
    //}
    public void SetBaseMoveSpeed(float speed)
    {
        baseMoveSpeed = speed;
    }
    public void SetBonusMoveSpeed(float speed)
    {
        bonusMoveSpeed = speed;
    }

    public bool IsManaEnough(int manaCost)
    {
        return mana >= manaCost;
    }
    public void UpdateMana(int manaCost)
    {
        mana += manaCost;
    }
}
