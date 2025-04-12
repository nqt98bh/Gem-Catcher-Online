using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviourPun,ISpawnableGem
{
    Vector2 direction;
    Rigidbody2D rb;
    float speed = 5f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        transform.Translate(direction*speed*Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D other) //other là thông tin của bất kì collider va chạm với collider này
    {
        //thiết lập diều kiện kiểm tra thông tin của OTHER
        if (other.gameObject.CompareTag("Player")) //nếu other có gắn tag player
        {
            AudioSource audioSource = other.GetComponent<AudioSource>();
            audioSource.Play();
            DestroyItem();
            CharacterMovement characterMovement = other.GetComponent<CharacterMovement>();
            characterMovement.EatDiamond();
            Debug.Log("SpeedUp");
            
        }

        else if (other.gameObject.CompareTag("Ground"))
        {
            DestroyItem();
        }
    }
    void DestroyItem()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        
            direction = dir.normalized;
       
    }
}
