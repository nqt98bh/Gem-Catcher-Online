using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5.0f;
    private Animator animator;
    private Camera mainCamera;
    public float timeSpeedUp= 3f;
    public float originalSpeed;
    private bool isBoosted = false;
    private Rigidbody rb;
    PhotonView PV;
    int score = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        originalSpeed = speed;
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }
   

 
    void Update()
    {
        if (!PV.IsMine) return;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ScoreIncrease"))
        {
            UpdateScore(1);
        }
        else if (other.CompareTag("ScoreDecrease"))
        {
            UpdateScore(-1);
        }
       
    }

    private void UpdateScore(int amount)
    {
        if (!PV.IsMine)
        {
            return;
        }
        object Score;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("score", out Score))
        {
            score = (int)Score;
        }
        score+= amount;
        ExitGames.Client.Photon.Hashtable scoreProp = new ExitGames.Client.Photon.Hashtable();
        scoreProp["score"] = score;
        PhotonNetwork.LocalPlayer.SetCustomProperties(scoreProp);
    }
    //public void ShowScores()
    //{
    //    foreach (Player player in PhotonNetwork.PlayerList)
    //    {
    //        object score;
    //        if (player.CustomProperties.TryGetValue("score", out score))
    //        {
    //            Debug.Log($"{player.NickName}: {score}");
    //        }
    //        else
    //        {
    //            Debug.Log($"{player.NickName}: No score yet");
    //        }
    //    }
    //}
}
