using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField input;
    public Transform roomContent;
    public List<RoomInfo> updatedRooms;
    public UIRoomProfile roomPrefab;
    public List<RoomProfile> rooms=new List<RoomProfile>();

    private void Start()
    {
        input.text = "Room1";
    }
    public virtual void Join()
    {
        string name = input.text;
        Debug.Log(transform.name + "created rom: " + name);
        PhotonNetwork.JoinRoom(name);
    }
    public virtual void Create()
    {
        string name = input.text;
        Debug.Log(transform.name + "created room" + name);
        PhotonNetwork.CreateRoom(name);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Lefted Room");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("create room failed" + message);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Roomlist updated");
        this.updatedRooms = roomList;
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) this.RoomRemove(roomInfo);
            else this.RoomAdd(roomInfo);
        }
        this.UpdateRoomProfileUI();

    }
    protected virtual void UpdateRoomProfileUI()
    {
        foreach(Transform child in roomContent)
        {
            Destroy(child);
        }
        foreach(RoomProfile roomProfile in rooms)
        {
            UIRoomProfile uIRoomProfile = Instantiate(this.roomPrefab);
            uIRoomProfile.SetRoomProfile(roomProfile);
            uIRoomProfile.transform.SetParent(this.roomContent);
        }
    }
    protected virtual void RoomAdd(RoomInfo roomInfo)
    {
        RoomProfile roomProfile = new RoomProfile { name = roomInfo.Name };
        if(roomProfile==null) return;
        this.rooms.Add(roomProfile);
    }
    public virtual void RoomRemove(RoomInfo roomInfo)
    {
        RoomProfile roomProfile = this.RoomByName(roomInfo.Name);
        if(roomProfile == null) return;
        this.rooms.Remove(roomProfile);
    }
    public virtual RoomProfile RoomByName(string name)
    {
        foreach(RoomProfile room in rooms)
        {
            if(room.name == name) return room;
        }
        return null;
    }
}
