using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬관리
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour {

#if UNITY_WEBPLAYER
        public static string webplayerQuitURL = "http://google.com";
#endif

    public GameObject Save_Prefab; // 저장하기
    public GameObject Sound_Prefab; // 사운드설정

    //프로그램 시작시 임시데이터 삭제
    private void Start() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        GameManager.getInstance().resetGame(); // 데이터 제거
    }

    public void btn_Start() // 게임시작
    {
        GameManager.getInstance().GetMainChapter();
    }

    public void btn_Load() // 이어하기
    {
        GameObject.Find("Canvas_Start").GetComponent<CanvasGroup>().interactable = false;
        GameObject temp = Instantiate(Save_Prefab);
        temp.name = "Save";
      
    } 

    public void btn_Sound() // 세팅창 열기
    {
        GameObject.Find("Canvas_Start").GetComponent<CanvasGroup>().interactable = false;
        GameObject temp = Instantiate(Sound_Prefab);
        temp.name = "Sound";
 
    }
    
    public void btn_Exit() // 게임종료 + 에디터종료
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
