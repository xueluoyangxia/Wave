using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerGoldManager : MonoBehaviour
{
    // 单例实例
    private static PlayerGoldManager _instance;
    public static PlayerGoldManager Instance
    {
        get 
        { 
            return _instance; 
        }
        private set 
        {
            _instance = value; 
        }
    }
    void Awake()
    {
        // 单例初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 场景切换时不销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("初始金币")]
    [SerializeField] private int currentGold = 50;

    // 事件：金币变化时触发（解耦关键）
    public UnityEvent<int> OnGoldChanged;
    //上面一句等价于下面关于事件的定义
    //// 定义一个只能传int参数的委托类型
    //public delegate void GoldChangedDelegate(int amount);

    //// 使用这个委托类型声明事件
    //public event GoldChangedDelegate OnGoldChanged;


    // 增加金币（怪物死亡时调用）
    public void AddGold(int amount)
    {
        currentGold += amount;
        OnGoldChanged?.Invoke(currentGold); // 触发事件，通知所有监听者
        //                                    // 触发事件前，必须检查是否为 null（没人监听时会为 null）
        //if (OnGoldChanged != null)
        //{
        //    OnGoldChanged.Invoke(currentGold);
        //}// 等同于上面的简写形式

        // 播放音效、飘字特效（可选）
       
    }

    // 消耗金币（建造、招募等）
    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            OnGoldChanged?.Invoke(currentGold);
            return true;
        }
        return false;
    }

    // 获取当前金币（其他脚本查询用）
    public int GetCurrentGold() => currentGold;

   
}
