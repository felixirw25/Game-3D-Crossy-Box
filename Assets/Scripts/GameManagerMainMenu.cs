using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerMainMenu : MonoBehaviour
{
    [SerializeField] Canvas mainMenu;
    [SerializeField] Canvas settingMenu;
    [SerializeField] TMP_Dropdown dropdown;

    private void Awake(){
        mainMenu.enabled = true;
        settingMenu.enabled = false;
    }

    public void LoadScene(int sceneIndex){
        SceneManager.LoadScene(sceneIndex);
    }
    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
        Debug.Log("Anda sedang di halaman " + sceneName);
    }
    public void SettingGame(){
        mainMenu.enabled = false;
        settingMenu.enabled = true;
        Debug.Log("Tombol Setting Ditekan");
    }
    public void BackMenu(){
        mainMenu.enabled = true;
        settingMenu.enabled = false;
        Debug.Log("Tombol Back Ditekan");
    }
    public void QuitGame(){
        Application.Quit();
        Debug.Log("Tombol Exit Ditekan");
    }
}
