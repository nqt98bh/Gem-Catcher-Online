using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public enum GemType 
{
    ScoreIncrease =0,
    ScoreDecrease=1,
    TimeIncrease=2,
    TimeDecrease=3,
    SpeedUp=4,
}

[System.Serializable]
public class GemProperties
{
    public GemType gemType;
    public GameObject GemPrefab;
    public float SpawnInterval;
}

public class GemFallScript : MonoBehaviourPun
{
    //[SerializeField] private float spawnForce = 50f;
    //[SerializeField] private float spawnSpeed = 5f;

    public List<GemProperties> gemProperties;
    private Dictionary<GemType, float> timers = new Dictionary<GemType, float>(); // Track timers for each gem type
    private Camera mainCamera;


   
    private void Start()
    {
        mainCamera = Camera.main;

        // Initialize timers for each gem type
        
        foreach (var property in gemProperties)
        {
          
            if (timers.ContainsKey(property.gemType))
            {
                timers[property.gemType] = 0f; // update timer to zero
            }
            else
            {
                timers.Add(property.gemType, property.SpawnInterval); //init 
                timers[property.gemType] = 0f; // set timer to zero
            }
        }
    }

    void Update()
    {
       if(!PhotonNetwork.IsMasterClient) return;
        SpawnGem();
    }

    public void SpawnGem()
    {
        foreach (var property in gemProperties)
        {
            timers[property.gemType] += Time.deltaTime; // Increment timer

            // Check if the timer has reached the spawn interval
            if (timers[property.gemType] >= property.SpawnInterval )
            {

                Vector2 spawnDirection = GetRandomSpawnDirection();
                Vector2 spawnPosition = GetRandomSpawnPosition(spawnDirection);
               
                GameObject gem = PhotonNetwork.Instantiate(property.GemPrefab.name, spawnPosition, Quaternion.identity);

                int viewID = gem.GetComponent<PhotonView>().ViewID;
                photonView.RPC("RPC_SetDirection", RpcTarget.AllViaServer, viewID, spawnDirection);
                //int typeIndex = (int)property.gemType;
                //int gemPrefabIndex = gemProperties.IndexOf(property);
                //float spawnInterval = (float)property.SpawnInterval;
                //photonView.RPC("Spawn", RpcTarget.AllViaServer, gemPrefabIndex);

                timers[property.gemType] = 0f; // Reset timer
            }
        }
    }

    [PunRPC]
    void RPC_SetDirection(int viewID, Vector2 direction)
    {
        PhotonView view = PhotonView.Find(viewID);
        if (view != null)
        {
            ISpawnableGem gem = view.GetComponent<ISpawnableGem>();
            if (gem != null)
            {
                
                gem.SetDirection(direction);
            }
        }
    }
   
    //void Spawn( int gamePrefabIndex)
    //{
       
    //    Vector2 spawnDirection = GetRandomSpawnDirection();
    //    Vector2 spawnPosition = GetRandomSpawnPosition(spawnDirection);
       
    //    GameObject Gems = Instantiate(gemProperties[gamePrefabIndex].GemPrefab, spawnPosition, Quaternion.identity);
     

    //    Rigidbody2D gemRigidbody = Gems.GetComponent<Rigidbody2D>();
    //    Debug.Log("Rigidbody Type After Instantiating: " + gemRigidbody.bodyType);
    //    if (gemRigidbody != null)
    //    {
    //       // gemRigidbody.bodyType = RigidbodyType2D.Dynamic;

    //        // Áp dụng lực theo hướng spawn
    //        gemRigidbody.AddForce(spawnDirection * spawnForce, ForceMode2D.Impulse);
    //    }
    //    Debug.Log("RPC_Spawn");

    //}
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
