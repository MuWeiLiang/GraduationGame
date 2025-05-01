using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMover : MonoBehaviour
{
    // �����ƶ���Χ
    private Vector2 moveRange = new Vector2(1, 1); // ���磬��x���y���ϸ��ƶ�5����λ

    // �ƶ��ٶ�
    private float moveSpeed = 2f;

    // Ŀ��λ��
    private Vector2 targetPosition;
    private Vector2 originPosition;
    private int direction = -1;

    void Start()
    {
        // ��ȡ��ʼλ��
        originPosition = transform.position;
        // ��ʼ��ʱ���õ�һ��Ŀ��λ��
        SetNewTargetPosition();
    }

    void Update()
    {
        // ƽ���ƶ���Ŀ��λ��
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // ����ӽ�Ŀ��λ�ã������µ�Ŀ��λ��
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
    }
    void SetNewTargetPosition()
    {
        // ��ȡ��ǰ�����λ��
        Vector2 startPosition = transform.position;

        // �����µ�Ŀ��λ�ã�ȷ�����ƶ���Χ��
        float newX = Mathf.Clamp(startPosition.x + Random.Range(-moveRange.x, moveRange.x),
                                 startPosition.x - moveRange.x,
                                 startPosition.x + moveRange.x);
        float newY = Mathf.Clamp(startPosition.y + Random.Range(-moveRange.y, moveRange.y),
                                 startPosition.y - moveRange.y,
                                 startPosition.y + moveRange.y);
        newX = Mathf.Clamp(newX, originPosition.x + moveRange.x, originPosition.x - moveRange.x);
        newY = Mathf.Clamp(newY, originPosition.y + moveRange.y, originPosition.y - moveRange.y);

        if ((newX - startPosition.x) * direction < 0)
        {
            direction *= -1;
            UpdateLocalScaleX(-1);
        }

        targetPosition = new Vector2(newX, newY);
    }
    void UpdateLocalScaleX(float x)
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= x;
        transform.localScale = localScale;
    }
}