using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬관리
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour {

#if UNITY_WEBPLAYER
        public static string webplayerQuitURL = "http://google.com";
#endif

    public GameObject SettingPanel;

    public void Btn_Start() // 게임시작
    {
        SceneManager.LoadScene("1_Hall");
    }

    public void Btn_Load() // 이어하기
    {
        PlayerPrefs.SetString("lastLoadedScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("GameSave", LoadSceneMode.Additive);
    } 

    public void Btn_Setting() // 세팅창 열기
    {
        SettingPanel.SetActive(true);
    }

    public void Btn_SettingOff() // 세팅창 끄기
    {
        SettingPanel.SetActive(false);
    }

    public void Btn_Exit() // 게임종료 + 에디터종료
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #elif UNITY_WEBPLAYER
             Application.OpenURL(webplayerQuitURL);
    #else
             Application.Quit();
    #endif

    }
}
