using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingUIManager : MonoBehaviour {


    public GameObject InfoPanel; // 개발자 정보 패널(음악 리소스...)
    public GameObject Sound_Prefab; // 사운드 설정
    public GameObject Save_Prefab;

    public void Start()
    {
        InfoPanel.SetActive(false);
        
    }

    public void Btn_Sound() // 소리설정
    {
        GameObject temp = Instantiate(Sound_Prefab);
        temp.name = "Sound";
    } 

    public void Btn_Save() // 저장하기
    {
        GameObject temp = Instantiate(Save_Prefab);
        temp.name = "Save";
    }

    public void Btn_Info() // 개발자 정보
    {
        InfoPanel.SetActive(true);
    }

    public void Btn_InfoOff()
    {
        InfoPanel.SetActive(false);
    }

    public void Btn_Start() // 첫화면으로 이동
    {
        SceneManager.LoadScene("UI_Start");

    }

    public void Btn_MenuOff() // 메뉴창 끄기 
    {
        GameObject.Destroy(GameObject.Find("Setting"));
       
    }
}
