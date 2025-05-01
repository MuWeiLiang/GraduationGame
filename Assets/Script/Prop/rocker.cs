using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocker : MonoBehaviour
{
    private bool playerInRange = false;
    private float cooldownTime = 3f;
    private bool isOnCooldown = false;
    public GameObject wall; // ��Ҷ���
    private bool isOnActive = true;
    public bool initActive = true; // ��ʼ״̬
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
            Debug.LogError("����Inspector��Ϊrocker�ű�����wall����");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (isOnCooldown)
            {
                Debug.Log("��ȴ�У����Ժ����ԡ�");
                return;
            }
            else if (!isOnCooldown)
            {
                //Debug.Log("������...");
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
}
