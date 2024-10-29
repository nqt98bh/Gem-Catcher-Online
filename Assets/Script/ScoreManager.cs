using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TextMeshProUGUI scoreText;
 
    public float remainingTime = 60f;
    public int score { get; private set; } = 0; //tạo điểm bắt đầu =0

    public GameObject GameOverPanel;
    public TextMeshProUGUI gameOverText;

    public GameObject DestroyGO1;
    public GameObject DestroyGO2;

    private bool isRunning = true;

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

    //Khai báo một hàm tăng số điểm người chơi


    void Start()
    {
        UpdateUI();
        remainingTime = 60f; //thời gian của màn chơi
        StartCoroutine(CountdownTimer()); //gọi hàm đếm ngược cho phép đồng hồ chạy song song,tiếp tục ở farme mới, kết thúc khi đủ thời gian
        
    }

    private void Update()
    {
        UpdateUI();
    }
    private IEnumerator CountdownTimer()
    {
        while (isRunning && remainingTime> 0) //nếu remaining >0, liên tục lặp lại lệnh dưới
        {
            yield return new WaitForSeconds(1f); //mỗi giây trôi qua
            remainingTime--; //trừ 1
            //UpdateUI();
        }
        if (remainingTime <= 0)
        {
            isRunning = false;
            GameOver();
        }

    }

    public void UpdateUI()
    {
        scoreText.text = "Score:" + score + "/ Time:" + Mathf.CeilToInt(remainingTime);//làm tròn thành số nguyên

    }
    private void GameOver()
    {
        
        gameOverText.text = "Game Over! / Score:" + score;
        GameOverPanel.SetActive(true);
        Destroy(DestroyGO1);
        Destroy(DestroyGO2);
    }

    public void Addscore(int amount ) //hàm nhận giá trị int và tên amount
    {
        
        score += amount;
        Debug.Log("addscore");
    }
    public void Reducescore(int x) 
    {

        score -= x;
        Debug.Log("Reducescore");
    }
    public  void Reducetime(float z) 
    {
        remainingTime -= z;
        //UpdateUI();
       
        Debug.Log("reducetime");
    }
    public  void AddTime( float z)
    {
        remainingTime += z;
        //UpdateUI();
        Debug.Log("extratime");
    }

   
}
