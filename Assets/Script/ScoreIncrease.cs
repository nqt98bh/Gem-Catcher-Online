using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreIncrease : MonoBehaviourPun,ISpawnableGem
{
    public float speed = 5f;
    public Vector2 direction;

    void Update()
    {
       transform.Translate(direction*speed*Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D other) //other là thông tin của bất kì collider va chạm với collider này
    {
        //thiết lập diều kiện kiểm tra thông tin của OTHER
        if (other.CompareTag("Player")) //nếu other có gắn tag player
        {
            AudioSource audioSource = other.GetComponent<AudioSource>();
            audioSource.Play();
            DestroyItem();
            ScoreManager.Instance.Addscore(1);
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
