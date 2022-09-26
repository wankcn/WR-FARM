using System;
using Cinemachine;
using UnityEngine;

/// <summary>
/// 切换相机边界
/// </summary>
public class SwitchBounds : MonoBehaviour
{
    private void Start()
    {
        SwitchBoundsShape();
    }

    /// <summary>
    /// 切换相机边界，更换场景时，获取场景内的bondsconfiner，来修改虚拟相机边界
    /// </summary>
    private void SwitchBoundsShape()
    {
        PolygonCollider2D boundShape2D =
            GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        var confiner = GetComponent<CinemachineConfiner>();
        confiner.m_BoundingShape2D = boundShape2D;

        // 每一次在runtime下切换的时候应该调用InvalidatePathCache方法清空缓存
        confiner.InvalidatePathCache();
    }
}