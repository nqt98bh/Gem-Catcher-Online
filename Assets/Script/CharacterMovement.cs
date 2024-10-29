using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement Instance;
    public float speed = 5.0f;
    private Animator animator;
    private Camera mainCamera;
    public float timeSpeedUp= 3f;
    public float originalSpeed;
    private bool isBoosted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        originalSpeed = speed;
    }

 
    void Update()
    {
        PlayerMover();
    }

    void PlayerMover()
    {
        float moveHorizonal = Input.GetAxis("Horizontal"); 
        bool isMoving = moveHorizonal != 0;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            transform.position += new Vector3(moveHorizonal * speed * Time.deltaTime, 0f, 0f);
            ConstrainPosition();

        }
    }
    void ConstrainPosition()
    {
        // Get camera bounds
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        // Get the player's current position
        Vector3 pos = transform.position;

        // Clamp the position
        pos.x = Mathf.Clamp(pos.x, -halfWidth , halfWidth );
        pos.y = Mathf.Clamp(pos.y, -halfHeight , halfHeight*2 );

        // Set the new position
        transform.position = pos;
    }
    public void EatDiamond()
    {
        if (!isBoosted)
        {
            StartCoroutine(SpeedBoost());
        }
    }

    private IEnumerator SpeedBoost()
    {
        isBoosted = true;
        speed += 5f; // Tăng tốc độ

        yield return new WaitForSeconds(timeSpeedUp); // Đợi 3 giây

        speed = originalSpeed; // Quay lại tốc độ ban đầu
        isBoosted = false;
    }

}
