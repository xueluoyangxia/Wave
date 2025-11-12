using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("目标设置")]
    public Transform player; // 玩家Transform

    [Header("跟随参数")]
    public float smoothSpeed = 5f; // 相机平滑移动速度
    public float rightOffsetRatio = 0.7f; // 向右时玩家位于画面的7/10处
    public float leftOffsetRatio = 0.3f; // 向左时玩家位于画面的3/10处

    [Header("边界限制")]
    public bool useBoundary = false;
    public float minX = -50f;
    public float maxX = 50f;
    public float minY = -10f;
    public float maxY = 10f;

    private Camera cam;
    private float cameraHalfWidth;

    void Start()
    {
        cam = GetComponent<Camera>();
        CalculateCameraWidth();
    }

    void CalculateCameraWidth()
    {
        // 计算相机的一半宽度（世界坐标）
        cameraHalfWidth = cam.orthographicSize * cam.aspect;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // 检测玩家水平移动方向
        float playerVelocityX = player.GetComponent<Rigidbody2D>()?.velocity.x ?? 0f;

        // 计算目标位置
        Vector3 targetPosition = CalculateTargetPosition(playerVelocityX);

        // 平滑移动相机
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        // 应用边界限制
        if (useBoundary)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);
        }

        // 保持z轴不变
        smoothedPosition.z = transform.position.z;
        transform.position = smoothedPosition;
    }

    Vector3 CalculateTargetPosition(float velocityX)
    {
        Vector3 playerPos = player.position;
        Vector3 targetPos = playerPos;

        // 根据移动方向计算偏移
        if (velocityX > 0.01f) // 向右移动
        {
            // 玩家应在画面的7/10处，即从左边起0.7的位置
            // 中点是0.5，所以需要向右偏移0.2个屏幕宽度
            float offsetX = cameraHalfWidth * (0.5f - rightOffsetRatio); // 0.5 - 0.7 = -0.2
            targetPos.x = playerPos.x + offsetX;
        }
        else if (velocityX < -0.01f) // 向左移动
        {
            // 玩家应在画面的3/10处，即从左边起0.3的位置
            // 中点是0.5，所以需要向左偏移0.2个屏幕宽度
            float offsetX = cameraHalfWidth * (0.5f - leftOffsetRatio); // 0.5 - 0.3 = 0.2
            targetPos.x = playerPos.x + offsetX;
        }
        else // 静止时保持当前位置（或可以回到中心）
        {
            // 可选：静止时回到中心
            // targetPos.x = playerPos.x;
        }

        // Y轴保持与玩家相同（或可以添加一些垂直偏移）
        targetPos.y = playerPos.y;

        return targetPos;
    }

    void OnValidate()
    {
        // 在编辑器中更新参数时重新计算
        if (cam == null)
            cam = GetComponent<Camera>();
        CalculateCameraWidth();
    }
}