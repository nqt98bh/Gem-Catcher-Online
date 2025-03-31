using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class PhotonStatus : MonoBehaviour
{
    private readonly string photonStatus ="Connection Status: ";
    public TextMeshProUGUI connectingStatus;

    private void Update()
    {
        
        connectingStatus.text = photonStatus+ PhotonNetwork.NetworkClientState.ToString();
    }
}
