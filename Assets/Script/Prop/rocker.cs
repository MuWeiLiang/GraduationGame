using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocker : MonoBehaviour
{
    private bool playerInRange = false;
    private float cooldownTime = 3f;
    private bool isOnCooldown = false;
    public GameObject wall; // 玩家对象
    private bool isOnActive = true;
    public bool initActive = true; // 初始状态
    // Start is called before the first frame update
    void Start()
    {
        if(wall != null)
        {
            if(!initActive)
            {
                UpdateRocker();
            }
        }
        else
        {
            Debug.LogError("请在Inspector中为rocker脚本分配wall对象。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (isOnCooldown)
            {
                Debug.Log("冷却中，请稍后再试。");
                return;
            }
            else if (!isOnCooldown)
            {
                //Debug.Log("传送中...");
                //StartCoroutine(TeleportWithCooldown());
                UpdateRocker();
            }
        }
    }
    void UpdateRocker()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        isOnCooldown = true;
        isOnActive = !isOnActive;
        if(wall != null)
            wall.SetActive(isOnActive);
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
}
