using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public enum GemType 
{
    ScoreIncrease,
    ScoreDecrease,
    TimeIncrease,
    TimeDecrease,
    SpeedUp,
}

[System.Serializable]
public class GemProperties
{
    public GemType gemType;
    public GameObject GemPrefab;
    public float SpawnInterval;
}

public  class GemFallScript : MonoBehaviour
{
    
    public List<GemProperties> gemProperties;
    private Dictionary<GemType, float> timers; // Track timers for each gem type

    private void Start()
    {
        // Initialize timers for each gem type
        timers = new Dictionary<GemType, float>();
        foreach (var property in gemProperties)
        {
            timers[property.gemType] = 0f; // Initialize timer to zero
        }
    }

    void Update()
    {
        // Update timers for each gem type
        foreach (var property in gemProperties)
        {
            timers[property.gemType] += Time.deltaTime; // Increment timer

            // Check if the timer has reached the spawn interval
            if (timers[property.gemType] >= property.SpawnInterval)
            {
                SpawnGem(property); // Spawn the gem
                timers[property.gemType] = 0f; // Reset timer

            }
        }

    }

    void  SpawnGem(GemProperties property)
    {
        
        //khai báo một biến có giá trị X ngẫu nhiên 
        float randomX = Random.Range(-8.5f, 8.5f); //random trong khoảng màn hình
        float randomY = Random.Range(2f, 6f);

        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f); //tọa độ randomx ,y,z tạo gem ở vị trí tọa độ x,y,z


        //tạo một bản sao của Gem tại vị trí và hướng quy định

        //int randomNumber = Random.Range(0, gemProperties.Count);
        //GameObject go = gemProperties[randomNumber].GemPrefab; //lấy môt phần tử từ list
        Instantiate(property.GemPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("íntantiate Object " + property.GemPrefab.name);

    }

}
