using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingUIManager : MonoBehaviour {

    //public GameObject InfoPanel;
    public GameObject Menu_Prefab;
    public GameObject Save_Prefab;
    public GameObject Setting_Prefab;
    public GameUIManager gameUI;
    

    public void Start()
    {
        GameObject test = Instantiate(Menu_Prefab);
        gameUI = test.GetComponent<GameUIManager>();
 
       // InfoPanel.SetActive(false);

    }

    public void Btn_Setting()
    {

   
            GameObject temp = Instantiate(Setting_Prefab);
            temp.name = "Setting";
    }

    public void Btn_Save()
    {
     
            GameObject test2 = Instantiate(Save_Prefab);
            test2.name = "Save";

    }

    public void Btn_Info() // 개발자 정보
    {
     //   InfoPanel.SetActive(true);
    }

    public void Btn_Start() // 첫화면으로 이동
    {
        SceneManager.LoadScene("Start");

    }

    public void Btn_MenuOff() // 메뉴창 끄기
    {

        GameObject.Destroy(GameObject.Find("Menu"));

    }

    public void Btn_SettingOff()
    {

            GameObject.Destroy(GameObject.Find("Setting"));

    }
}
