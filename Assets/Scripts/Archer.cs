using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Archer : MonoBehaviour
{
    public GameObject arrowPrefab; // 拖入Arrow预制体
    public Transform firePoint;    // 发射位置

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootArrow();
        }
    }

    void ShootArrow()
    {
        // 实例化箭矢
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

        // 箭矢的Awake会自动让它飞起来
    }
}
    

