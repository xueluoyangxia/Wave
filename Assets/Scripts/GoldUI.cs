using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 如果使用TextMeshPro
using UnityEngine.UI;

public class GoldUI : MonoBehaviour
{
    [Header("UI组件")]
    [SerializeField] public Text goldText;// 拖入TextMeshPro文本
                                          // 如果用普通UGUI Text，就改成：public Text goldText;

    void Start()
    {
        // 初始显示
        UpdateGoldDisplay(PlayerGoldManager.Instance.GetCurrentGold());

        // 订阅事件：金币变化时自动更新
        PlayerGoldManager.Instance.OnGoldChanged.AddListener(UpdateGoldDisplay);
    }

    // 金币变化时的回调
    private void UpdateGoldDisplay(int newGoldAmount)
    {
        if (goldText != null)
        {
            goldText.text = $"金币: {newGoldAmount}";
        }
    }

    void OnDestroy()
    {
        // 取消订阅，防止内存泄漏
        if (PlayerGoldManager.Instance != null)
        {
            PlayerGoldManager.Instance.OnGoldChanged.RemoveListener(UpdateGoldDisplay);
        }
    }
}