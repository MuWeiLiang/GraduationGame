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
        // 检查碰撞对象是否是玩家
        if (collision.CompareTag("Player"))
        {
            // 获取玩家的血量组件
            BaseCharacter playerHealth = collision.GetComponent<BaseCharacter>();

            if (playerHealth != null && !beUsed)
            {
                anim.SetTrigger("using");
                // 增加玩家血量
                // playerHealth.GetHeal(healAmount);
                beUsed = true;

                // 延迟销毁对象，确保音效能播放完
                Destroy(gameObject, 1f);
            }
        }
    }
}
