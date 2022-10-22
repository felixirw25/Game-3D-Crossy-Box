using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static void SaveData(Player player) {
        if(player.MaxTravel>player.Data.HighScore)
            PlayerPrefs.SetInt("highest_score", player.MaxTravel);

        PlayerPrefs.SetInt("player_score", player.MaxTravel);
        PlayerPrefs.Save();
    }

    public static ScoreData LoadData(){
        var tempData = new ScoreData();
        tempData.Score = PlayerPrefs.GetInt("player_score", 0);
        tempData.HighScore = PlayerPrefs.GetInt("highest_score", 0);

        return tempData;
    }
}
