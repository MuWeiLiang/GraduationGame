using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocker2 : MonoBehaviour
{
    private bool playerInRange = false;
    private float cooldownTime = 3f;
    private bool isOnCooldown = false;
    private bool isOnActive = true;
    public bool initActive = true; // 初始状态

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
                Debug.Log("冷却中，请稍后再试。");
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
        // 翻转开关的朝向（可选）
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        // 设置冷却
        isOnCooldown = true;
        isOnActive = !isOnActive; // 切换状态

        // 获取所有子物体（包括自己），并设置它们的激活状态
        foreach (Transform child in transform)
        {
            // 如果 child 不是自己（rocker2），才执行
            if (child != transform)
            {
                child.gameObject.SetActive(isOnActive);
            }
        }

        // 重置冷却
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
            // 可以在这里显示提示UI，如"按F切换墙壁"
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
}
