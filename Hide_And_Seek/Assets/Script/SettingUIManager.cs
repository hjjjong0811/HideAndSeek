using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingUIManager : MonoBehaviour {

    //public GameObject InfoPanel;
    public GameObject MenuPanel;
    public GameObject Save_Prefab;
    public GameObject Setting_Prefab;

    public bool isOpenSave = false;
    public bool isOpenSetting = false;

    public void Start()
    {
       // InfoPanel.SetActive(false);

    }

    public void Btn_Setting()
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

    public void Btn_Save()
    {
        if (isOpenSave)
        {
            GameObject.Destroy(GameObject.Find("Save"));
            isOpenSave = false;
        }
        else
        {
            GameObject test2 = Instantiate(Save_Prefab);
            test2.name = "Save";
            isOpenSave = true;
        }
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
