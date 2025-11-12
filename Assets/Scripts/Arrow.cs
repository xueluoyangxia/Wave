using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("箭矢属性")]
    public float moveSpeed = 10f;
    public int damage = 1; // 箭的伤害值（每个箭可不同）

    [Header("特效")]
    public GameObject hitEffect; // 命中特效预制体

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // 自动向右飞（根据你的朝向调整）
        rb.velocity = transform.right * moveSpeed;
    }

    // 核心：碰撞检测
    void OnTriggerEnter2D(Collider2D other)
    {
        // 尝试获取碰撞物体的怪物组件
        MonsterBase monster = other.GetComponent<MonsterBase>();

        if (monster != null)
        {
            // 命中怪物
            monster.TakeDamage(damage);

            // 生成命中特效
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            // 销毁箭矢
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // 射到地面也销毁（参数可调整）
            Destroy(gameObject);
        }
    }

    // 3秒后自动销毁（防止箭矢飞出地图无限存在）
    void Start()
    {
        Destroy(gameObject, 3f);
    }
}
