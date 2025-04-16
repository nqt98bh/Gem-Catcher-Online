using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    string[] characterType = new string[2] { "Character1", "Character2" };
    string selectCharacter;
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
        if (PhotonNetwork.IsMasterClient)
        {
            selectCharacter = characterType[0];

        }
        else
        {
            selectCharacter = characterType[1];
        }
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", selectCharacter),Vector3.zero,Quaternion.identity);
        Debug.Log("Instantiate player controller");
    }
}
