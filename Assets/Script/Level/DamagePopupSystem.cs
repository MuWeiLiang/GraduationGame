using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopupSystem : MonoBehaviour
{
    [SerializeField] private GameObject damagePopupPrefab;

    // 生成伤害数字
    public void ShowDamage(float damage, Vector3 spawnPosition, bool isCritical = false)
    {
        GameObject popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);

        if(popup == null)
        {
            Debug.LogError("Damage Popup Prefab is not assigned in the inspector.");
            return;
        }

        // 设置文本内容
        TextMeshPro text = popup.GetComponent<TextMeshPro>();
        text.text = Mathf.RoundToInt(damage).ToString();

        // 暴击效果
        if (isCritical)
        {
            text.color = Color.yellow;
            text.fontSize *= 1.5f;
        }

        // 自动销毁
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

        // 设置文本内容
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

        // 设置文本内容
        TextMeshPro text = popup.GetComponent<TextMeshPro>();
        text.text = Mathf.RoundToInt(damage).ToString();
        text.color = Color.blue;

        Destroy(popup, 0.5f);
    }

    // 动态获取物体高度
    private float GetHeightOffset(Transform target)
    {
        if (target.TryGetComponent<Renderer>(out var renderer))
        {
            return renderer.bounds.extents.y + 0.5f;
        }
        return 1.5f; // 默认高度
    }
}
