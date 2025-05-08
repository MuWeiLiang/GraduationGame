using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawn : MonoBehaviour
{
    [Header("ҩˮ��������")]
    private GameObject potionPrefab; // ��ǰҪ���ɵ�ҩˮԤ����
    private GameObject potionPrefab1, potionPrefab2; // ҩˮԤ����
    private Transform spawnPoint;    // ҩˮ����λ�ã���ѡ��
    private SLevelManager levelManager;
    public KeyCode pickupKey = KeyCode.F; // ʰȡ����
    public float cooldownTime = 15f; // ��ȴʱ�䣨�룩

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
            // ������������ʾUI��ʾ����"��Fʰȡ"
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
            // ��������������UI��ʾ
        }
    }

    private void Start()
    {
        potionPrefab1 = Resources.Load<GameObject>("Prefab/Potions/bluePotion");
        potionPrefab2 = Resources.Load<GameObject>("Prefab/Potions/redPotion");
        if (potionPrefab1 == null || potionPrefab2 == null)
        {
            Debug.LogError("δ�ҵ�ҩˮԤ���壬����·����");
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
        // ��ȴ��ʱ������
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
                Debug.Log("��ȴ�����������ٴ�����ҩˮ");
            }
        }

        if (playerInRange && Input.GetKeyDown(pickupKey) && !isOnCooldown)
        {
            FindObjectOfType<PropSoundBase>().Pickup(3);
            // ���ѡ��һ��ҩˮԤ����

            GetRandomPotion();

            GeneratePotion();

            // ��ʼ��ȴ
            isOnCooldown = true;
            cooldownTimer = cooldownTime;
            Debug.Log($"��ʼ��ȴ��ʣ��ʱ��: {cooldownTime}��");
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

        // ����������ͬ����
        if (randomIndex == lastPotionIndex)
        {
            samePotionCount++;
        }
        else
        {
            samePotionCount = 1; // ���ü���
        }

        lastPotionIndex = randomIndex;
        potionPrefab = (lastPotionIndex == 0) ? potionPrefab1 : potionPrefab2;
    }

    void GeneratePotion()
    {
        if (potionPrefab == null)
        {
            Debug.LogError("δ����ҩˮԤ���壡");
            return;
        }

        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
        Instantiate(potionPrefab, spawnPosition, Quaternion.identity);
        Debug.Log($"{potionPrefab.name}ҩˮ�����ɣ�");
    }
}
