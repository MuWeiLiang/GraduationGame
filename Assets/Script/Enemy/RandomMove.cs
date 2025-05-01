using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : BaseEnemy
{
    public float xOffset; // X轴最大偏移量
    public float yOffset; // Y轴最大偏移量
    private float moveSpeed = 2f; // 移动速度
    private float idleTimeMin = 1f; // 最小空闲时间
    private float idleTimeMax = 3f; // 最大空闲时间

    protected Vector2 initialPosition; // 初始位置
    private Vector2 targetPosition; // 目标位置
    protected bool isMoving = false; // 是否正在移动
    protected Coroutine movementCoroutine;
    // Start is called before the first frame update
    public void StartMove()
    {
        movementCoroutine = StartCoroutine(RandomMovement());
    }

    public void Tick()
    {
        if (isMoving)
        {
            MoveToTarget();
        }
    }

    IEnumerator RandomMovement()
    {
        while (true)
        {
            // 等待随机时间
            float idleTime = Random.Range(idleTimeMin, idleTimeMax);
            yield return new WaitForSeconds(idleTime);

            // 生成新的随机目标位置
            float randomX = initialPosition.x + Random.Range(-xOffset, xOffset);
            float randomY = initialPosition.y + Random.Range(-yOffset, yOffset);
            targetPosition = new Vector2(randomX, randomY);

            // 设置移动状态
            isMoving = true;

            // 等待到达目标位置
            yield return new WaitUntil(() => !isMoving);
        }
    }

    void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // 检查是否到达目标位置
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }

        // 更新朝向（可选）
        UpdateFacingDirection();
    }

    // 更新敌人朝向（面向移动方向）
    void UpdateFacingDirection()
    {
        if ((targetPosition.x - transform.position.x) * transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
    public void UpdateFacingDirection(Vector3 position)
    {
        if ((position.x - transform.position.x) * transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

}
