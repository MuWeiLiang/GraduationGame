using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform destination; // 拖拽目标点对象到这里
    private Vector3 destinationPosition; // 目标点位置
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
            Debug.LogError("请在Inspector中为Teleporter脚本分配目标点对象。");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (isOnCooldown)
            {
                Debug.Log("传送冷却中，请稍后再试。");
                return;
            }
            else if (!isOnCooldown)
            {
                Debug.Log("传送中...");
                StartCoroutine(TeleportWithCooldown());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // 可以在这里显示提示UI，如"按F传送"
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // 可以在这里隐藏提示UI
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
            // 可以在这里添加传送特效或音效
        }
    }
}
