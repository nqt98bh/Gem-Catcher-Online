using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRoomProfile : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI roomName;
    [SerializeField] protected RoomProfile roomProfile;

    public virtual void SetRoomProfile(RoomProfile roomProfile)
    {
        this.roomProfile = roomProfile;
        this.roomName.text = roomProfile.name;
    }
}
