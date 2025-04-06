using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }
    void CreateController()
    {
       
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","Character"),Vector3.zero,Quaternion.identity);
        Debug.Log("Instantiate player controller");
    }
}
