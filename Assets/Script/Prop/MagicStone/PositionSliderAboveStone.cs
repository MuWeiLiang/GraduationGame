using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionSliderAboveStone : MonoBehaviour
{
    // public Transform MagicStone; // ��קMagicStone��Inspector
    private Slider slider;        // ��קSlider��Inspector

    void Start()
    {
        slider = GetComponent<MagicStone>().GetSlider();
        if (slider != null)
        {
            slider.gameObject.SetActive(false);
            // ����Slider�ĳߴ磨���80������ԭ�и߶ȣ�
            RectTransform rt = slider.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(80f, rt.sizeDelta.y);



            Image fillImage = slider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                // ������ɫΪ��ɫ��RGBA: 1, 0.65, 0, 1��
                fillImage.color = new Color(1f, 0.65f, 0f, 1f);
            }

            Image bgImage = slider.transform.Find("Background").GetComponent<Image>();
            if (bgImage != null)
            {
                // ���ð�͸����ɫ��R=0.5, G=0.5, B=0.5, A=0.5��
                bgImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }

            Transform handle = slider.transform.Find("Handle Slide Area/Handle");
            if (handle != null)
            {
                handle.gameObject.SetActive(false); // �����ֱ�
                // ����ֱ�����٣�Destroy(handle.gameObject);
            }
        }
    }

    void Update()
    {
        if (slider != null)
        {
            // ����MagicStone�Ϸ�1��λ����������
            Vector3 worldPosition = transform.position + Vector3.up * 1f;

            // ת��Ϊ��Ļ����
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            // ����Sliderλ��
            slider.GetComponent<RectTransform>().position = screenPosition;
        }
    }
}
