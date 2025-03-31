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

public class GemFallScript : MonoBehaviour
{
    [SerializeField] private float spawnForce = 100f;
    public List<GemProperties> gemProperties;
    private Dictionary<GemType, float> timers = new Dictionary<GemType, float>(); // Track timers for each gem type

    private Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;

        // Initialize timers for each gem type
        
        foreach (var property in gemProperties)
        {
            //timers[property.gemType] = 0f; // Initialize timer to zero
            ////timers.Add(property.gemType, property.SpawnInterval);
            if (timers.ContainsKey(property.gemType))
            {
                timers[property.gemType] = 0f; // update timer to zero
            }
            else
            {
                timers.Add(property.gemType, property.SpawnInterval); //init 
                timers[property.gemType] = 0f; // set timer to zero
            }

            if(gemProperties.IndexOf(property) == 3)
            {

            }

        }
        foreach (var property in gemProperties)
        {
            
            Debug.Log(timers[property.gemType]);
        }

        for (int i = 0; i < gemProperties.Count; i++)
        {
            var property = gemProperties[i];
            if (timers.ContainsKey(property.gemType))
            {
                timers[property.gemType] = 0f; // update timer to zero
            }
            else
            {
                timers.Add(property.gemType, property.SpawnInterval); //init 
                timers[property.gemType] = 0f; // set timer to zero
            }

            if(i == 3)
            {

            }
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

    void SpawnGem(GemProperties property)
    {

        Vector2 spawnDirection = GetRandomSpawnDirection();
        Vector2 spawnPosition = GetRandomSpawnPosition(spawnDirection);
       
        GameObject Gems = Instantiate(property.GemPrefab, spawnPosition, Quaternion.identity);

        //tạo một bản sao của Gem tại vị trí và hướng quy định
        //int randomNumber = Random.Range(0, gemProperties.Count);
        //GameObject go = gemProperties[randomNumber].GemPrefab; //take an object from the list

        Rigidbody2D gemRigidbody = Gems.GetComponent<Rigidbody2D>();
        
        if (gemRigidbody != null)
        {
            // Áp dụng lực theo hướng spawn
            gemRigidbody.AddForce(spawnDirection * spawnForce, ForceMode2D.Impulse);
        }
        Debug.Log("íntantiate Object " + property.GemPrefab.name);
    }
    Vector2 GetRandomSpawnDirection()
    {
        // create a random direction from 4 screen sides
        int randomDirection = Random.Range(0, 4); // choose one of 4 direction
        switch (randomDirection)
        {
            case 0: // Spawn from down side
                return Vector2.down;
            case 1: // Spawn from up side
                return Vector2.up;
            case 2: // Spawn from right side
                return Vector2.right;
            case 3: // Spawn from left side
                return Vector2.left;
            default:
                return Vector2.zero;
        }
    }
    //get a random spawn position based on the spawn direction
    Vector2 GetRandomSpawnPosition(Vector2 direction)
    {
        float height = 2f * mainCamera.orthographicSize; // Calculate the screen height
        float width = height * mainCamera.aspect; // Calculate the screen width

        float posX = 0f;
        float posY = 0f;

        // Position based on spawn direction
        if (direction == Vector2.up || direction == Vector2.down) // Spawn from top or bottom
        {
            posX = Random.Range(-width / 2, width / 2); // Randomize x
            posY = (direction == Vector2.up) ? -height / 2 : height / 2; // Spawn from bottom or top
        }
        else if (direction == Vector2.left || direction == Vector2.right) // Spawn from left or right
        {
            posY = Random.Range(-height / 2, height / 2); // Randomize y
            posX = (direction == Vector2.left) ? width / 2 : -width / 2; // Spawn from left or right
        }

        return new Vector2(posX, posY); // Return calculated position
    }
}
