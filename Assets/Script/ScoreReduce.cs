using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreReduce : MonoBehaviourPun,ISpawnableGem
{
 
    Vector2 direction;
    float speed = 5f;
 
    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D other) //other là thông tin của bất kì collider va chạm với collider này
    {
        //thiết lập diều kiện kiểm tra thông tin của OTHER
        if (other.CompareTag("Player")) //nếu other có gắn tag player
        {
            AudioSource audioSource = other.GetComponent<AudioSource>();
            audioSource.Play();
            DestroyItem();

            OnTriggerEnterProcess();
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
    protected virtual void OnTriggerEnterProcess ()
    {
        ScoreManager.Instance.Reducescore(1);
    }

    public void SetDirection(Vector2 dir)
    {
        
            direction = dir.normalized;
        
    }
}
