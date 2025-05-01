using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionSliderAboveStone : MonoBehaviour
{
    // public Transform MagicStone; // 拖拽MagicStone到Inspector
    private Slider slider;        // 拖拽Slider到Inspector

    void Start()
    {
        slider = GetComponent<MagicStone>().GetSlider();
        if (slider != null)
        {
            slider.gameObject.SetActive(false);
            // 设置Slider的尺寸（宽度80，保持原有高度）
            RectTransform rt = slider.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(80f, rt.sizeDelta.y);



            Image fillImage = slider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                // 设置颜色为橙色（RGBA: 1, 0.65, 0, 1）
                fillImage.color = new Color(1f, 0.65f, 0f, 1f);
            }

            Image bgImage = slider.transform.Find("Background").GetComponent<Image>();
            if (bgImage != null)
            {
                // 设置半透明灰色（R=0.5, G=0.5, B=0.5, A=0.5）
                bgImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }

            Transform handle = slider.transform.Find("Handle Slide Area/Handle");
            if (handle != null)
            {
                handle.gameObject.SetActive(false); // 隐藏手柄
                // 或者直接销毁：Destroy(handle.gameObject);
            }
        }
    }

    void Update()
    {
        if (slider != null)
        {
            // 计算MagicStone上方1单位的世界坐标
            Vector3 worldPosition = transform.position + Vector3.up * 1f;

            // 转换为屏幕坐标
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            // 更新Slider位置
            slider.GetComponent<RectTransform>().position = screenPosition;
        }
    }
}
