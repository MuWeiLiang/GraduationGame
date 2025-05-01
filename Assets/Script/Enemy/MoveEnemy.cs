using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : BaseEnemy
{
    public float moveRange = 3f; // 移动范围
    public float moveSpeed = 2f; // 移动速度

    private Vector3 startPosition; // 初始位置
    private Vector3 previousPosition; // 上一帧的位置
    private float faceDir = 1;
    private float elapsedTime = 0f;
    // Start is called before the first frame update
    public virtual void Start()
    {
        startPosition = transform.position;
        previousPosition = startPosition;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        elapsedTime += Time.deltaTime * moveSpeed;
        float pingPongValue = Mathf.PingPong(elapsedTime, moveRange * 2);

        Vector3 newPosition = startPosition + Vector3.right * (pingPongValue - moveRange);

        transform.position = newPosition;

        float moveDirectionX = newPosition.x - previousPosition.x;

        if (moveDirectionX * faceDir < 0)
        {
            transform.Rotate(0, 180, 0);
            faceDir *= -1;
        }

        previousPosition = newPosition;
    }

    public int GetFaceDir()
    {
        return (int)faceDir;
    }
}
