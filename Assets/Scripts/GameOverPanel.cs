using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] TMP_Text highestScoreText;

    public void OnEnable(){
        ScoreData Data;
        Data = DataManager.LoadData();

        finalScoreText.text = "Final Score: " + Data.Score.ToString();
        highestScoreText.text = "Highest Score: " + Data.HighScore.ToString();
    }
}
