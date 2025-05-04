using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeEnemy : MoveEnemy
{
    private int damageNum = 20; // �˺���ֵ
    private float lastAttackTime = 0f; // ��¼�ϴι���ʱ��
    private float attackCooldown = 1f; // ������ȴʱ�䣨1�룩
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start(); // ���û���� Start ����
        damagePopupSystem = FindObjectOfType<DamagePopupSystem>();
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
            ContactPoint2D contact = collision.contacts[0];
            Vector2 normal = contact.normal;
            SPlayerController player = collision.gameObject.GetComponent<SPlayerController>();
            // �������λ�ü��
            Collider2D myCollider = GetComponent<Collider2D>();
            Vector2 relativePosition = myCollider.bounds.center - player.transform.position;
            if (normal.y < -0.75f && relativePosition.y < -0.1f)
            {               
                Die();
            }
            else
            {
                // ����Ƿ�����ȴʱ����
                if (Time.time - lastAttackTime >= attackCooldown)
                {                  
                    int direction = GetFaceDir();
                    if (player != null)
                    {
                        player.GetHurt(direction, damageNum);
                        lastAttackTime = Time.time; // �����ϴι���ʱ��
                    }
                }
            }
        }
    }

    public override Vector3 GetShowPosition()
    {
        Vector3 position = transform.position;
        position.y -= 1f;
        return position;
    }
}
