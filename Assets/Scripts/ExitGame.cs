using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject pausePanel;
    public void SetPause(bool isPause){
        if(isPause){
            Time.timeScale = 0;
            gamePanel.SetActive(false);
            pausePanel.SetActive(true);
        }
        else{
            Time.timeScale = 1;
            gamePanel.SetActive(true);
            pausePanel.SetActive(false);
        }
    }
    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
