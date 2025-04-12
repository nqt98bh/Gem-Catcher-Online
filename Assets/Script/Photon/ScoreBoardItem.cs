using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreBoardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text userNameText;
    public TMP_Text scoreText;
    Player player;
    public void Innitialize(Player player)
    {
        userNameText.text=player.NickName;
        this.player = player;
        ShowScores();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(player == targetPlayer)
        {
            if (changedProps.ContainsKey("score"))
            {
                ShowScores();
            }
        }
    }

    public void ShowScores()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object score;
            if (this.player == player && player.CustomProperties.TryGetValue("score", out score))
            {
                scoreText.text=score.ToString();
                Debug.Log($"{player.NickName}: {score}");
            }
            else
            {
                Debug.Log($"{player.NickName}: No score yet");
            }
        }
    }
}
