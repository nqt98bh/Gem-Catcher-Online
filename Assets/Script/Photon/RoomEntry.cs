using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    public Text roomNameText;
    public Text roomPlayersText;
    public Button JoinRoomButton;
    private string roomName;


    private void Start()
    {
        JoinRoomButton.onClick.AddListener(() =>
        {
            PhotonNetwork.JoinRoom(roomName);
        });
    }

    public void Initialize(string name,byte currentPlayer,byte maxPlayer)
    {
        roomName = name;
        roomNameText.text = name;
        roomPlayersText.text = currentPlayer + "/" +maxPlayer;


    }
    
}
