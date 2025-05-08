using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawn : MonoBehaviour
{
    [Header("药水生成设置")]
    private GameObject potionPrefab; // 当前要生成的药水预制体
    private GameObject potionPrefab1, potionPrefab2; // 药水预制体
    private Transform spawnPoint;    // 药水生成位置（可选）
    private SLevelManager levelManager;
    public KeyCode pickupKey = KeyCode.F; // 拾取按键
    public float cooldownTime = 15f; // 冷却时间（秒）

    private bool playerInRange = false;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    private int samePotionCount = 0;
    private int lastPotionIndex = -1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // 可以在这里显示UI提示，如"按F拾取"
            if (levelManager != null)
            {
                levelManager.ActivePrompt("Press F");
            }
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

    private void Start()
    {
        potionPrefab1 = Resources.Load<GameObject>("Prefab/Potions/bluePotion");
        potionPrefab2 = Resources.Load<GameObject>("Prefab/Potions/redPotion");
        if (potionPrefab1 == null || potionPrefab2 == null)
        {
            Debug.LogError("未找到药水预制体，请检查路径！");
            return;
        }
        spawnPoint = transform.Find("spawnPoint");

        levelManager = FindObjectOfType<SLevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("SLevelManager not found!");
        }
    }

    private void Update()
    {
        // 冷却计时器更新
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
                Debug.Log("冷却结束，可以再次生成药水");
            }
        }

        if (playerInRange && Input.GetKeyDown(pickupKey) && !isOnCooldown)
        {
            FindObjectOfType<PropSoundBase>().Pickup(3);
            // 随机选择一个药水预制体

            GetRandomPotion();

            GeneratePotion();

            // 开始冷却
            isOnCooldown = true;
            cooldownTimer = cooldownTime;
            Debug.Log($"开始冷却，剩余时间: {cooldownTime}秒");
        }
    }

    void GetRandomPotion()
    {
        if (samePotionCount >= 2)
        {
            samePotionCount = 0;
            lastPotionIndex = (lastPotionIndex == 0) ? 1 : 2;
            potionPrefab = (lastPotionIndex == 0) ? potionPrefab1 : potionPrefab2;
            return;
        }

        int randomIndex = Random.Range(0, 2);

        // 更新连续相同计数
        if (randomIndex == lastPotionIndex)
        {
            samePotionCount++;
        }
        else
        {
            samePotionCount = 1; // 重置计数
        }

        lastPotionIndex = randomIndex;
        potionPrefab = (lastPotionIndex == 0) ? potionPrefab1 : potionPrefab2;
    }

    void GeneratePotion()
    {
        if (potionPrefab == null)
        {
            Debug.LogError("未分配药水预制体！");
            return;
        }

        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
        Instantiate(potionPrefab, spawnPosition, Quaternion.identity);
        Debug.Log($"{potionPrefab.name}药水已生成！");
    }
}
