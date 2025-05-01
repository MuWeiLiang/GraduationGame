using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPotion : MonoBehaviour
{
    private bool beUsed = false;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �����ײ�����Ƿ������
        if (collision.CompareTag("Player"))
        {
            // ��ȡ��ҵ�Ѫ�����
            BaseCharacter playerHealth = collision.GetComponent<BaseCharacter>();

            if (playerHealth != null && !beUsed)
            {
                anim.SetTrigger("using");
                // �������Ѫ��
                // playerHealth.GetHeal(healAmount);
                beUsed = true;

                // �ӳ����ٶ���ȷ����Ч�ܲ�����
                Destroy(gameObject, 1f);
            }
        }
    }
}
