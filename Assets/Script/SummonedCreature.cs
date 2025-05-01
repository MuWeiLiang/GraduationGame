using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedCreature : MonoBehaviour
{
    [Header("��������")]
    public float moveSpeed = 3f;          // �ƶ��ٶ�
    public float attackRange = 2f;        // ������Χ
    public float attackCooldown = 1f;     // ������ȴʱ��
    public int damage = 10;               // �����˺�

    [Header("��������")]
    public float followDistance = 2f;     // �����˵ĸ������
    public float tooCloseDistance = 0.5f;   // ̫���ľ���

    public Transform master;             // ����(�ٻ���)

    private int direction = 1;
    private float localScaleNum;

    // ��ʼ���ٻ���
    public void Initialize(Transform masterTransform)
    {
        master = masterTransform;
    }

    void Start()
    {
        // rb = GetComponent<Rigidbody>();
        localScaleNum = transform.localScale.x;
    }

    void Update()
    {
        if (master == null)
        {
            // ���˲����ڣ���������
            Destroy(gameObject);
            return;
        }

        FollowMaster();
    }

    // ��������
    private void FollowMaster()
    {
        float distanceToMaster = Vector3.Distance(transform.position, master.position);
        if (distanceToMaster > 10f)
        {
            transform.position = master.position; // ֱ�Ӵ��͵����λ��
            return; // ���ֺ���ִ�к����߼�
        }
        if (distanceToMaster > followDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, master.position, moveSpeed * Time.deltaTime);
        }
        else if (distanceToMaster < tooCloseDistance)
        {
            Vector3 retreatDirection = (transform.position - master.position).normalized;

            Vector3 retreatTarget = master.position + retreatDirection * tooCloseDistance;

            transform.position = Vector2.MoveTowards(
                transform.position,
                retreatTarget,
                moveSpeed * Time.deltaTime
            );
        }
        int newDirection = master.position.x - transform.position.x >= 0 ? 1 : -1;
        if (newDirection * direction < 0) {
            transform.localScale = new Vector3(newDirection * localScaleNum, localScaleNum, localScaleNum);
        }
        direction = newDirection;
    }
}
