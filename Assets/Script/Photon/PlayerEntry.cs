
using UnityEngine;
using UnityEngine.UI;


public class PlayerEntry : MonoBehaviour
{
    [Header("UI References")]
    public Text PlayerNameText;


    private int ownerId;

    public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
        PlayerNameText.text = playerName;
    }

 
}

