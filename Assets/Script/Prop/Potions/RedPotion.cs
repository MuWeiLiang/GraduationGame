using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPotion : MonoBehaviour
{
    private int healAmount = 20;
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
            SPlayerController player = collision.GetComponent<SPlayerController>();

            if (player != null && !beUsed)
            {
                anim.SetTrigger("using");
                // �������Ѫ��
                player.GetHeal(healAmount);
                beUsed = true;
                FindObjectOfType<PropSoundBase>().Pickup(2);

                // �ӳ����ٶ���ȷ����Ч�ܲ�����
                Destroy(gameObject, 1f);
            }
        }
    }
}
