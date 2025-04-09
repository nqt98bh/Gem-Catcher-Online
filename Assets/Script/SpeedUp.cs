using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
   
    [SerializeField] private float speed;

  
    void OnTriggerEnter2D(Collider2D other) //other là thông tin của bất kì collider va chạm với collider này
    {
        //thiết lập diều kiện kiểm tra thông tin của OTHER
        if (other.gameObject.CompareTag("Player")) //nếu other có gắn tag player
        {
            AudioSource audioSource = other.GetComponent<AudioSource>();
            audioSource.Play();
            Destroy(gameObject); //xóa GameObject đang gắn collider này, GameObject chính là đối tượng dc gắn script này
            CharacterMovement characterMovement = other.GetComponent<CharacterMovement>();
            characterMovement.EatDiamond();
            Debug.Log("SpeedUp");
            
        }

        else if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject); //xóa GameObject đang gắn collider này, GameObject chính là đối tượng dc gắn script này

        }
    }
}
