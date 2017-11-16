using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬관리
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour {

#if UNITY_WEBPLAYER
        public static string webplayerQuitURL = "http://google.com";
#endif
    
<<<<<<< HEAD
    public GameObject Save_Prefab;
    public GameObject Setting_Prefab;


=======
    public GameObject SettingPanel;
    
>>>>>>> master

    public void Btn_Start() // 게임시작
    {
        SceneManager.LoadScene("1_Hall");
    }

    public void Btn_Load() // 이어하기
    {
<<<<<<< HEAD
       
            GameObject temp = Instantiate(Save_Prefab);
            temp.name = "Save";
      
=======
        
        PlayerPrefs.SetString("lastLoadedScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("GameSave", LoadSceneMode.Additive);
>>>>>>> master
    } 

    public void Btn_Setting() // 세팅창 열기
    {
<<<<<<< HEAD
     
            GameObject temp = Instantiate(Setting_Prefab);
            temp.name = "Setting";
     
=======
        SettingPanel.SetActive(true);
>>>>>>> master
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
