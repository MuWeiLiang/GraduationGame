using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform destination; // ��קĿ����������
    private Vector3 destinationPosition; // Ŀ���λ��
    private bool playerInRange = false;
    private float cooldownTime = 15f;
    private bool isOnCooldown = false;
    void Start()
    {
        if (destination != null)
        {
            destinationPosition = destination.position;
        }
        else
        {
            destinationPosition = transform.position;
            Debug.LogError("����Inspector��ΪTeleporter�ű�����Ŀ������");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (isOnCooldown)
            {
                Debug.Log("������ȴ�У����Ժ����ԡ�");
                return;
            }
            else if (!isOnCooldown)
            {
                Debug.Log("������...");
                StartCoroutine(TeleportWithCooldown());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // ������������ʾ��ʾUI����"��F����"
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
    IEnumerator TeleportWithCooldown()
    {
        isOnCooldown = true;
        TeleportPlayer();
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
    private void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if(destination != null)
                player.transform.position = destination.position;
            else
                player.transform.position = destinationPosition;
            // ������������Ӵ�����Ч����Ч
        }
    }
}
