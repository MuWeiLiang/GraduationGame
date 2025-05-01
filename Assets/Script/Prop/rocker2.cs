using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocker2 : MonoBehaviour
{
    private bool playerInRange = false;
    private float cooldownTime = 3f;
    private bool isOnCooldown = false;
    private bool isOnActive = true;
    public bool initActive = true; // ��ʼ״̬

    void Start()
    {
        if (!initActive)
        {
            UpdateRocker();
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (isOnCooldown)
            {
                Debug.Log("��ȴ�У����Ժ����ԡ�");
                return;
            }
            else
            {
                UpdateRocker();
            }
        }
    }

    void UpdateRocker()
    {
        // ��ת���صĳ��򣨿�ѡ��
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        // ������ȴ
        isOnCooldown = true;
        isOnActive = !isOnActive; // �л�״̬

        // ��ȡ���������壨�����Լ��������������ǵļ���״̬
        foreach (Transform child in transform)
        {
            // ��� child �����Լ���rocker2������ִ��
            if (child != transform)
            {
                child.gameObject.SetActive(isOnActive);
            }
        }

        // ������ȴ
        Invoke("ResetIsOnCooldown", cooldownTime);
    }

    void ResetIsOnCooldown()
    {
        isOnCooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // ������������ʾ��ʾUI����"��F�л�ǽ��"
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // ����������������ʾUI
        }
    }
}
