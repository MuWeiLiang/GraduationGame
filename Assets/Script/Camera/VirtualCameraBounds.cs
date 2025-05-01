using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCameraBounds : MonoBehaviour
{
    private float minX, maxX, minY, maxY; // 相机的移动范围
    private CinemachineVirtualCamera vcam;
    private Transform target; // 被跟随的目标对象

    void Start()
    {
        // 获取 Virtual Camera 组件
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            // 获取 Follow 目标
            target = vcam.Follow;
            if (target == null)
            {
                Debug.LogError("CinemachineVirtualCamera 的 Follow 目标未设置！");
            }
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera 组件未找到！");
        }
        float[] CameraData = LevelBaseData.Instance.LevelMode == 0 ? 
            LevelBaseData.Instance.LevelCamera[LevelBaseData.Instance.currentLevel] : LevelBaseData.Instance.SLevelCamera[LevelBaseData.Instance.currentLevel];
        // 设置相机的移动范围
        minX = CameraData[0]; // 最小 X 轴位置
        maxX = CameraData[1]; // 最大 X 轴位置
        minY = CameraData[2];  // 最小 Y 轴位置
        maxY = CameraData[3]; // 最大 Y 轴位置
        // Debug.Log("相机范围设置为: " + minX + ", " + maxX + ", " + minY + ", " + maxY);
    }

    void LateUpdate()
    {
        if (vcam == null || target == null)
            return;

        // 获取目标对象的位置
        Vector3 vcamPosition = target.position;

        // 计算相机的位置，限制在范围内
        Vector3 desiredPosition = new Vector3(
            Mathf.Clamp(vcamPosition.x, minX, maxX), // 限制 X 轴
            Mathf.Clamp(vcamPosition.y, minY, maxY), // 限制 Y 轴
            target.position.z // 保持相机的 Z 轴不变
        );

        // 获取 Virtual Camera 的 Body 设置
        var body = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (body != null)
        {
            // 设置 Virtual Camera 的位置为目标位置
            //body.m_TrackedObjectOffset.x = clampedPosition.x;
            //body.m_TrackedObjectOffset.y = clampedPosition.y;
            body.m_TrackedObjectOffset = new Vector3(
                desiredPosition.x - target.position.x,
                desiredPosition.y - target.position.y,
                body.m_TrackedObjectOffset.z
            );
        }
        else
        {
            Debug.Log("CinemachineFramingTransposer 组件未找到！");
        }

        // 如果需要限制相机的位置而不依赖 CinemachineBody，可以直接设置 Transform
        // vcam.transform.position = clampedPosition;
    }
}
