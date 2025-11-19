using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class GoldGenerator : MonoBehaviour
{
    [Header("生产设置")]
    [Tooltip("每秒生产金币数量")]
    [SerializeField] private float productionRate = 2f;

    [Tooltip("最大存储容量")]
    [SerializeField] private int maxCapacity = 100;

    [Tooltip("起始金币（用于关卡初始资源）")]
    [SerializeField] private float initialGold = 0f;

    [Header("收集设置")]
    [Tooltip("检测玩家距离")]
    [SerializeField] private float detectionRadius = 2f;

    [Tooltip("互动按键")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("可选：视觉效果")]
    [Tooltip("显示当前存储金币的UI文本")]
    [SerializeField] private Text goldDisplayText;

    [Tooltip("互动提示（如'按E收集'）")]
    [SerializeField] private GameObject interactPrompt;

    [Tooltip("收集时的特效")]
    [SerializeField] private ParticleSystem collectEffect;

    [Tooltip("生产时的粒子效果")]
    [SerializeField] private ParticleSystem productionEffect;

    private float storedGold = 0f;
    public Transform playerTransform;
    private bool isPlayerNear = false;

    private void Start()
    {
        storedGold = initialGold;
        UpdateDisplay();

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        // 检查PlayerGoldManager是否存在
        if (PlayerGoldManager.Instance == null)
        {
            Debug.LogError("❌ GoldGenerator: PlayerGoldManager.Instance 未找到！请确保场景中有PlayerGoldManager。");
        }
    }

    private void Update()
    {
        // 生产金币
        if (storedGold < maxCapacity)
        {
            storedGold += productionRate * Time.deltaTime;
            storedGold = Mathf.Min(storedGold, maxCapacity);

            // 生产特效
            if (productionEffect != null && !productionEffect.isPlaying)
                productionEffect.Play();
        }
        else
        {
            if (productionEffect != null && productionEffect.isPlaying)
                productionEffect.Stop();
        }

        // 检测玩家距离
        CheckPlayerProximity();
    }

    private void CheckPlayerProximity()
    {
        if (playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        isPlayerNear = distance <= detectionRadius;

        if (isPlayerNear)
        {
            // 显示互动提示
            if (interactPrompt != null)
                interactPrompt.SetActive(true);

            // 检测互动键
            if (Input.GetKeyDown(interactKey))
            {
                CollectGold();
            }
        }
        else
        {
            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }

        UpdateDisplay();
    }

    private void CollectGold()
    {
        if (storedGold < 1f) return;

        int collectedAmount = Mathf.FloorToInt(storedGold);

        // 添加到玩家金币
        PlayerGoldManager.Instance.AddGold(collectedAmount);

        // 播放收集特效
        if (collectEffect != null)
            collectEffect.Play();

        // 清零存储
        storedGold = 0f;

        // 可以在这里添加音效
        // AudioManager.Instance.PlaySound("CollectGold");
    }

    private void UpdateDisplay()
    {
        if (goldDisplayText != null)
        {
            goldDisplayText.text = Mathf.FloorToInt(storedGold).ToString();
        }
    }

    // 通过触发器检测玩家（不需要每帧计算距离，性能更好）
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerTransform == other.transform)
        {
            playerTransform = null;
            isPlayerNear = false;
            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }

    // 在Scene视图中显示检测范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
