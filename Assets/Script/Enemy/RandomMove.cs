using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : BaseEnemy
{
    public float xOffset; // X�����ƫ����
    public float yOffset; // Y�����ƫ����
    private float moveSpeed = 2f; // �ƶ��ٶ�
    private float idleTimeMin = 1f; // ��С����ʱ��
    private float idleTimeMax = 3f; // ������ʱ��

    protected Vector2 initialPosition; // ��ʼλ��
    private Vector2 targetPosition; // Ŀ��λ��
    protected bool isMoving = false; // �Ƿ������ƶ�
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
            // �ȴ����ʱ��
            float idleTime = Random.Range(idleTimeMin, idleTimeMax);
            yield return new WaitForSeconds(idleTime);

            // �����µ����Ŀ��λ��
            float randomX = initialPosition.x + Random.Range(-xOffset, xOffset);
            float randomY = initialPosition.y + Random.Range(-yOffset, yOffset);
            targetPosition = new Vector2(randomX, randomY);

            // �����ƶ�״̬
            isMoving = true;

            // �ȴ�����Ŀ��λ��
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

        // ����Ƿ񵽴�Ŀ��λ��
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }

        // ���³��򣨿�ѡ��
        UpdateFacingDirection();
    }

    // ���µ��˳��������ƶ�����
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
