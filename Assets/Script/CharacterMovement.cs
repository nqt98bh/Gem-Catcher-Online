using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5.0f;
    private Animator animator;
    private Camera mainCamera;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        
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

}
