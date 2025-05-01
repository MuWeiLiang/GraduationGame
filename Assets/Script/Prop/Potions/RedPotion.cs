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
        // 检查碰撞对象是否是玩家
        if (collision.CompareTag("Player"))
        {
            // 获取玩家的血量组件
            SPlayerController player = collision.GetComponent<SPlayerController>();

            if (player != null && !beUsed)
            {
                anim.SetTrigger("using");
                // 增加玩家血量
                player.GetHeal(healAmount);
                beUsed = true;
                FindObjectOfType<PropSoundBase>().Pickup(2);

                // 延迟销毁对象，确保音效能播放完
                Destroy(gameObject, 1f);
            }
        }
    }
}
