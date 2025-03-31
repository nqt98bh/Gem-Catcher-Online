using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreReduce : MonoBehaviour
{
    public float speed = 5f;
    void Update()
    {
        //transform.Translate(Vector3.down * speed * Time.deltaTime); //tạo chuyển dộng rơi xuống

    }

    void OnTriggerEnter2D(Collider2D other) //other là thông tin của bất kì collider va chạm với collider này
    {
        //thiết lập diều kiện kiểm tra thông tin của OTHER
        if (other.CompareTag("Player")) //nếu other có gắn tag player
        {
            AudioSource audioSource = other.GetComponent<AudioSource>();
            audioSource.Play();
            Destroy(gameObject); //xóa GameObject đang gắn collider này, GameObject chính là đối tượng dc gắn script này
            OnTriggerEnterProcess();
        }

        else if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject); //xóa GameObject đang gắn collider này, GameObject chính là đối tượng dc gắn script này

        }
    }

    protected virtual void OnTriggerEnterProcess ()
    {
        ScoreManager.Instance.Reducescore(1);
    }
}
