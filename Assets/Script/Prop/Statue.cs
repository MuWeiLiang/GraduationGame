using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    private bool playerInRange = false; // 玩家是否在范围内
    public KeyCode pickupKey = KeyCode.F;
    private bool beUsed = false; // 是否在冷却中
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(pickupKey) && !beUsed)
        {
            FindObjectOfType<PropSoundBase>().Pickup(3);
            GameObject Prefab = Resources.Load<GameObject>("Prefab/Prop/timeTurner");
            if (Prefab != null) {
                beUsed = true;
                Vector3 spawnPosition = transform.position;
                spawnPosition.x += 2f;
                Instantiate(Prefab, spawnPosition, Quaternion.identity);
                Debug.Log($"{Prefab.name}已生成！");
            }
            else {
                Debug.LogError("未找到道具预制体，请检查路径！");
                return;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // 可以在这里显示UI提示，如"按F拾取"
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // 可以在这里隐藏UI提示
        }
    }
}
