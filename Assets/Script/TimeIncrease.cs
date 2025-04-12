using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeIncrease : MonoBehaviourPun,ISpawnableGem
{
    Vector3 moveDirection ;
    public float extraTime = 5f;
    public float speed = 5f;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        transform.Translate(moveDirection*speed*Time.deltaTime);
    }
    
    void OnTriggerEnter2D(Collider2D other) //other là thông tin của bất kì collider va chạm với collider này
    {
        //thiết lập diều kiện kiểm tra thông tin của OTHER
        if (other.gameObject.CompareTag("Player")) //nếu other có gắn tag player
        {
            AudioSource audioSource = other.GetComponent<AudioSource>();
            audioSource.Play();
            DestroyItem();
            ScoreManager.Instance.AddTime(5);
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
        
            moveDirection = dir.normalized;
       
    }
}
