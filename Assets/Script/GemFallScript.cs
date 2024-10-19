using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class GemFallScript : MonoBehaviour
{
    public GameObject GemPrefab; //khai báo biến prefab của gem


    public float timer; //biến thời gian kể từ lần sinh ra cuối của gem

    public float maxSpawnInterval = 0f;//khoảng thời gian giữa mỗi lần tạo gem (3s)
    public float minSpawnInterval = 0f;
    private void Start()
    {
        //SpawnGem();
    }

    void Update()
    {
        
        timer += Time.deltaTime;
        if (timer >= GetRanDomSpawnInterval()) //nếu thời gian lớn hơn hoặc băng thời gian sinh gem, gọi hàm dưới
        {
            SpawnGem(); // gọi hàm tạo gem
            timer= 0; //đặt lại biến bộ đếm
        }
        
    }

    void SpawnGem()
    {
        
        //khai báo một biến có giá trị X ngẫu nhiên 
        float randomX = Random.Range(-11f, 5.5f); //random trong khoảng màn hình
        float randomY = Random.Range(5f, 9.5f);

        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f); //tọa độ randomx ,y,z tạo gem ở vị trí tọa độ x,y,z

        Instantiate(GemPrefab, spawnPosition, Quaternion.identity);
        //tạo một bản sao của Gem tại vị trí và hướng quy định

    }

    float GetRanDomSpawnInterval()
    {
        return Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
