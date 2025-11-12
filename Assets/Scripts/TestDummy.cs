using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 如果需要显示血量

public class TestDummy : MonsterBase
{
    

    void Start()
    {
        base.SetHP(); // 调用父类初始化血量

        // 自动添加SpriteRenderer（如果忘记加）
        if (GetComponent<SpriteRenderer>() == null)
        {
            gameObject.AddComponent<SpriteRenderer>().color = Color.gray;
        }
    }

    

    // 重写受伤方法，添加测试反馈
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage); // 调用父类逻辑扣血+判死
        // 控制台日志（方便调试）
        Debug.Log($"[TestDummy] 受到伤害: {damage}, 剩余血量: {currentHealth}");
    }

    
    
}
