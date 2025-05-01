using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeEnemy : MoveEnemy
{
    private int damageNum = 40; // �˺���ֵ
    private float lastAttackTime = 0f; // ��¼�ϴι���ʱ��
    private float attackCooldown = 1f; // ������ȴʱ�䣨1�룩
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start(); // ���û���� Start ����
        moveRange = 5f;
        moveSpeed = 3f;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SPlayerController player = collision.gameObject.GetComponent<SPlayerController>();
            int direction = GetFaceDir();
            if (player != null && (Time.time - lastAttackTime) >= attackCooldown)
            {
                player.GetHurt(direction, damageNum);
                lastAttackTime = Time.time; // �����ϴι���ʱ��
            }
        }
    }
}
