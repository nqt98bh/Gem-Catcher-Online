using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int HP = 5;
    [SerializeField] private float speed = 5f;


 

    // Update is called once per frame
    void Update()
    {
        PlayerMover();
    }
    void PlayerMover()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float moveHozontal = horizontalInput * speed * Time.deltaTime;
        transform.position = new Vector2(transform.position.x + moveHozontal, transform.position.y);

    }
}
