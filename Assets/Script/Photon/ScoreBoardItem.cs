using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardItem : MonoBehaviour
{
    public TMP_Text userNameText;
    public TMP_Text scoreText;

    public void Innitialize(Player player)
    {
        userNameText.text=player.NickName;
    }
}
