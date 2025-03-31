using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputName;

   
    public virtual void Login()
    {
        string name = inputName.text;
        PhotonNetwork.LocalPlayer.NickName = name;
        PhotonNetwork.ConnectUsingSettings();

    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
       // Debug.Log("Joined Lobby");
    }
}
