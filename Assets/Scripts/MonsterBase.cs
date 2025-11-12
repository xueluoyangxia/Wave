using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    [Header("基础属性")]
    public float maxHealth = 10f;
    public int rewardGold = 5; // 每个怪物的金币奖励

    protected float currentHealth;

     protected void SetHP()
     {
        currentHealth = maxHealth;
     }

    // 受伤方法
    virtual public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 死亡方法（核心）
    protected void Die()
    {
        // 关键：在Destroy前调用
        if (PlayerGoldManager.Instance != null)
        {
            PlayerGoldManager.Instance.AddGold(rewardGold);
        }

        // 生成死亡特效（可选）
        // Instantiate(deathEffect, transform.position, Quaternion.identity);

        // 最后销毁
        Destroy(gameObject);
    }
}
