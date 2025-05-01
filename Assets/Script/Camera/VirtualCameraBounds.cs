using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCameraBounds : MonoBehaviour
{
    private float minX, maxX, minY, maxY; // ������ƶ���Χ
    private CinemachineVirtualCamera vcam;
    private Transform target; // �������Ŀ�����

    void Start()
    {
        // ��ȡ Virtual Camera ���
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            // ��ȡ Follow Ŀ��
            target = vcam.Follow;
            if (target == null)
            {
                Debug.LogError("CinemachineVirtualCamera �� Follow Ŀ��δ���ã�");
            }
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera ���δ�ҵ���");
        }
        float[] CameraData = LevelBaseData.Instance.LevelMode == 0 ? 
            LevelBaseData.Instance.LevelCamera[LevelBaseData.Instance.currentLevel] : LevelBaseData.Instance.SLevelCamera[LevelBaseData.Instance.currentLevel];
        // ����������ƶ���Χ
        minX = CameraData[0]; // ��С X ��λ��
        maxX = CameraData[1]; // ��� X ��λ��
        minY = CameraData[2];  // ��С Y ��λ��
        maxY = CameraData[3]; // ��� Y ��λ��
        // Debug.Log("�����Χ����Ϊ: " + minX + ", " + maxX + ", " + minY + ", " + maxY);
    }

    void LateUpdate()
    {
        if (vcam == null || target == null)
            return;

        // ��ȡĿ������λ��
        Vector3 vcamPosition = target.position;

        // ���������λ�ã������ڷ�Χ��
        Vector3 desiredPosition = new Vector3(
            Mathf.Clamp(vcamPosition.x, minX, maxX), // ���� X ��
            Mathf.Clamp(vcamPosition.y, minY, maxY), // ���� Y ��
            target.position.z // ��������� Z �᲻��
        );

        // ��ȡ Virtual Camera �� Body ����
        var body = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (body != null)
        {
            // ���� Virtual Camera ��λ��ΪĿ��λ��
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
            Debug.Log("CinemachineFramingTransposer ���δ�ҵ���");
        }

        // �����Ҫ���������λ�ö������� CinemachineBody������ֱ������ Transform
        // vcam.transform.position = clampedPosition;
    }
}
