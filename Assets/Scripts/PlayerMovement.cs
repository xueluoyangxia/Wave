using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 5f;  // 角色移动速度

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float horizontalInput;
    private Animator anim_player;

    void Awake()
    {
        // 获取必要的组件
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim_player = GetComponent<Animator>();
    }

    void Update()
    {
        // 获取水平输入（A键 = -1，D键 = +1）
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 根据移动方向翻转角色精灵
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;  // 向右移动，不翻转
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;   // 向左移动，水平翻转
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }
    void FixedUpdate()
    {
        // 在FixedUpdate中处理物理移动
        SwitchAnim();
        Move();
    }
    public void SwitchAnim()
    {
        anim_player.SetFloat("speed", Mathf.Abs(horizontalInput * moveSpeed));
    }

}