using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMover : MonoBehaviour
{
    // 定义移动范围
    private Vector2 moveRange = new Vector2(1, 1); // 例如，在x轴和y轴上各移动5个单位

    // 移动速度
    private float moveSpeed = 2f;

    // 目标位置
    private Vector2 targetPosition;
    private Vector2 originPosition;
    private int direction = -1;

    void Start()
    {
        // 获取初始位置
        originPosition = transform.position;
        // 初始化时设置第一个目标位置
        SetNewTargetPosition();
    }

    void Update()
    {
        // 平滑移动到目标位置
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 如果接近目标位置，设置新的目标位置
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
    }
    void SetNewTargetPosition()
    {
        // 获取当前对象的位置
        Vector2 startPosition = transform.position;

        // 计算新的目标位置，确保在移动范围内
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