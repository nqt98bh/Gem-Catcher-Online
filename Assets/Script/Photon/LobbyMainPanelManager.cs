using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMainPanelManager : MonoBehaviourPunCallbacks
{
    [Header ("PhotonLogin Panel")]
    public GameObject LoginPanel;
    public TMP_InputField PlayerNameInput;

    [Header ("Seletion Panel")]
    public GameObject SelectionPanel;

    [Header("CreateRoom Panel")]
    public GameObject CreateRoomPanel;
    public TMP_InputField RoomNameInput;
    public TMP_InputField PlayerNumberInput;

    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomPanel;

    [Header("Room List Panel")]
    public GameObject RoomListPanel;
    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomPanel;
    public GameObject StartButton;
    public GameObject PlayerListEntriesPrafab;

    private Dictionary<string, RoomInfo> cachedRoomList; //bộ nhớ đệm Room List
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
        PlayerNameInput.text = "Player" + Random.Range(001, 999);
    }

    //Login //
    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;
        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else Debug.LogError("Player name is invalid");
    }
    public override void OnConnectedToMaster()
    {
        this.SetActivePanel(SelectionPanel.name);
    }

    //Seletion Panel//
    public void OnSelectCreateRoom()
    {
        this.SetActivePanel(CreateRoomPanel.name);
    }
    public void OnSelectJoinRandomRoom()
    {
        this.SetActivePanel(JoinRandomRoomPanel.name);

    }
    public void OnSelectRoomList()
    {
        this.SetActivePanel(RoomListPanel.name);
    }
    public void OnExitButtonClick()
    {
     
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Stop play mode in the editor
#else
            Application.Quit();  // Quit the game in a build
#endif
    }

    //Create Room//

    public void OnCreateRoomButtonClicked()
    {
        string roomName = RoomNameInput.text;
        if(RoomNameInput.text == "")
        {
            roomName = "Room " + Random.Range(001, 999);
        }
        
        byte maxPlayer;
        byte.TryParse(PlayerNumberInput.text, out maxPlayer);
        maxPlayer = (byte) Mathf.Clamp(maxPlayer, 0, 8);
        if (PlayerNumberInput.text == "") maxPlayer = 8;
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = maxPlayer,PlayerTtl =100000 };
        //If a player disconnects unexpectedly, Photon keeps their data in the room for this duration before removing them.
        PhotonNetwork.CreateRoom(roomName, roomOptions,null);
    }
  
    public override void OnJoinedRoom()
    {
        // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)
        cachedRoomList.Clear();
        SetActivePanel(InsideRoomPanel.name);

        if(playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }
        
        foreach (Player p in PhotonNetwork.PlayerList) //duyệt qua danh sách playerlist của Photon
        {
            GameObject entry = Instantiate(PlayerListEntriesPrafab); 
            entry.transform.SetParent(InsideRoomPanel.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerListEntry>().Initialize(p.ActorNumber, p.NickName);

            object isPlayerReady;
            
            playerListEntries.Add(p.ActorNumber, entry);
        }
        StartButton.SetActive(PhotonNetwork.IsMasterClient);

    }
   
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(SelectionPanel.name);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(SelectionPanel.name);
    }


    //Join Random Room//
    public void OnJoinRandomRoomButtonClicked()
    {
        SetActivePanel(JoinRandomRoomPanel.name);
        PhotonNetwork.JoinRandomRoom(); 
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(001, 999);
        RoomOptions options = new RoomOptions { MaxPlayers = 8 };
        PhotonNetwork.CreateRoom(roomName, options);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject player = Instantiate(PlayerListEntriesPrafab);
        player.transform.SetParent(InsideRoomPanel.transform);
        player.transform.localScale = Vector3.one;
        player.GetComponent<PlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);
        playerListEntries.Add(newPlayer.ActorNumber, player);

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);

    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        StartButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    //Room List//
    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            Debug.Log("Joined Lobby");
        }
        SetActivePanel(RoomListPanel.name);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();
        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
        Debug.Log("Updated room list");
    }
    

    private void UpdateRoomListView()
    {
        foreach(RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name,(byte) info.PlayerCount, (byte) info.MaxPlayers);
            roomListEntries.Add(info.Name, entry);
        }
        Debug.Log("ROOM LIST IS UPDATED");
    }
    private void ClearRoomListView()
    {
        foreach(GameObject entry in roomListEntries.Values)
        {
            Destroy(entry);
        }
        roomListEntries.Clear();
    }
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach(RoomInfo info in roomList)
        {
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }

        }
        foreach (var entry in  cachedRoomList)
        {
            Debug.Log(entry.Key);
        }
    }
    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        SetActivePanel(SelectionPanel.name);
    }

    //Inside Room//
    public void OnLeaveRoomButtonClicked()
    {
        
            PhotonNetwork.LeaveRoom();
        //PhotonNetwork.JoinLobby();
    }
    public void OnStartButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel("Game");
        else Debug.LogWarning("You are not the master client!!");
    }
    public override void OnLeftRoom()
    {
        SetActivePanel(SelectionPanel.name);
        foreach (GameObject entry in playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }
        playerListEntries.Clear();
        playerListEntries = null;


    }
    public override void OnJoinedLobby()
    {
        // whenever this joins a new lobby, clear any previous room lists
        cachedRoomList.Clear();
        ClearRoomListView();
        Debug.Log("Joined Lobby");
    }

    // note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
        Debug.Log("Onleft lobby");
    }

    private void SetActivePanel(string panelName)
    {
        LoginPanel.SetActive(panelName.Equals(LoginPanel.name));
        SelectionPanel.SetActive(panelName.Equals(SelectionPanel.name));
        CreateRoomPanel.SetActive(panelName.Equals(CreateRoomPanel.name));
        JoinRandomRoomPanel.SetActive(panelName.Equals(JoinRandomRoomPanel.name));
        RoomListPanel.SetActive(panelName.Equals(RoomListPanel.name));
        InsideRoomPanel.SetActive(panelName.Equals(InsideRoomPanel.name));

    }

}
