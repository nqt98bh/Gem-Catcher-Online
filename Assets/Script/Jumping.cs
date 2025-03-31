using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{
    public AudioSource JumpAudioSource;
    [SerializeField] private float jumpForce = 1f; // Lực nhảy
    public LayerMask groundLayer; // Layer cho mặt đất
    private Rigidbody2D rb;

    private int jumpCount; // Đếm số lần nhảy
    private bool isGrounded; // Kiểm tra có đang trên mặt đất

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = 0; // Khởi tạo số lần nhảy
    }

    void Update()
    {
        // Kiểm tra nhảy
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || jumpCount < 2))
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); // Đặt vận tốc Y về 0 để nhảy chính xác
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpCount++; // Tăng số lần nhảy
        isGrounded = false; // Đánh dấu là không còn trên mặt đất
        JumpAudioSource.Play();

        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu chạm vào mặt đất
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0; // Đặt lại số lần nhảy khi chạm đất
        }
    }


}
