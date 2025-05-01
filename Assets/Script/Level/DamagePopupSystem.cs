using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopupSystem : MonoBehaviour
{
    [SerializeField] private GameObject damagePopupPrefab;

    // �����˺�����
    public void ShowDamage(float damage, Vector3 spawnPosition, bool isCritical = false)
    {
        GameObject popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);

        if(popup == null)
        {
            Debug.LogError("Damage Popup Prefab is not assigned in the inspector.");
            return;
        }

        // �����ı�����
        TextMeshPro text = popup.GetComponent<TextMeshPro>();
        text.text = Mathf.RoundToInt(damage).ToString();

        // ����Ч��
        if (isCritical)
        {
            text.color = Color.yellow;
            text.fontSize *= 1.5f;
        }

        // �Զ�����
        Destroy(popup, 0.5f);
    }

    public void ShowHeal(float damage, Vector3 spawnPosition)
    {
        GameObject popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);

        if (popup == null)
        {
            Debug.LogError("Damage Popup Prefab is not assigned in the inspector.");
            return;
        }

        // �����ı�����
        TextMeshPro text = popup.GetComponent<TextMeshPro>();
        text.text = Mathf.RoundToInt(damage).ToString();
        text.color = Color.green;

        Destroy(popup, 0.5f);
    }

    public void ShowMana(float damage, Vector3 spawnPosition)
    {
        GameObject popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);

        if (popup == null)
        {
            Debug.LogError("Damage Popup Prefab is not assigned in the inspector.");
            return;
        }

        // �����ı�����
        TextMeshPro text = popup.GetComponent<TextMeshPro>();
        text.text = Mathf.RoundToInt(damage).ToString();
        text.color = Color.blue;

        Destroy(popup, 0.5f);
    }

    // ��̬��ȡ����߶�
    private float GetHeightOffset(Transform target)
    {
        if (target.TryGetComponent<Renderer>(out var renderer))
        {
            return renderer.bounds.extents.y + 0.5f;
        }
        return 1.5f; // Ĭ�ϸ߶�
    }
}
