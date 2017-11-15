using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬관리
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour {

#if UNITY_WEBPLAYER
        public static string webplayerQuitURL = "http://google.com";
#endif
    
    public GameObject Save_Prefab;
    public GameObject Setting_Prefab;

    public bool isOpenSave = false;
    public bool isOpenSetting = false;


    

    public void Btn_Start() // 게임시작
    {
        SceneManager.LoadScene("1_Hall");
    }

    public void Btn_Load() // 이어하기
    {
        if (isOpenSave)
        {
            GameObject.Destroy(GameObject.Find("Save"));
            isOpenSave = false;
        }
        else
        {
            GameObject temp = Instantiate(Save_Prefab);
            temp.name = "Save";
            isOpenSave = true;
        }
    } 

    public void Btn_Setting() // 세팅창 열기
    {
        if (isOpenSetting)
        {
            GameObject.Destroy(GameObject.Find("Setting"));
            isOpenSetting = false;
        }
        else
        {
            GameObject temp = Instantiate(Setting_Prefab);
            temp.name = "Setting";
            isOpenSetting = true;
        }
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
