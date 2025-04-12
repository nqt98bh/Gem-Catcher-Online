using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard: MonoBehaviourPunCallbacks
{
    [SerializeField] Transform contaniner;
    [SerializeField] GameObject scoreboardPrefab;
    Dictionary<Player,ScoreBoardItem> scoreBoardItems = new Dictionary<Player,ScoreBoardItem>();

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreSboardItem(player);
        }
    }
  
  
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreSboardItem(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreBoardItem(otherPlayer);
    }
    public void AddScoreSboardItem(Player player)
    {
        ScoreBoardItem item = Instantiate(scoreboardPrefab, contaniner).GetComponent<ScoreBoardItem>();
        item.Innitialize(player);
        scoreBoardItems[player] = item;
    }
    public void RemoveScoreBoardItem(Player player)
    {
        Destroy(scoreBoardItems[player].gameObject);
        scoreBoardItems.Remove(player);
    }

}
