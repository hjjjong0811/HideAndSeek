using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUIManager : MonoBehaviour {


    public GameObject InfoPanel; // 개발자 정보 패널(음악 리소스...)
    public GameObject Sound_Prefab; // 사운드 설정
    public GameObject Save_Prefab;
    public List<string> SoundList;
    public Text Developter;
    public Text Sound;

    public void Start()
    {
        InfoPanel.SetActive(false);
        SoundInfoView();

    }

    public void Btn_Sound() // 소리설정
    {
        GameObject.Find("Setting").GetComponent<CanvasGroup>().interactable = false;
        GameObject temp = Instantiate(Sound_Prefab);
        temp.name = "Sound";
    } 

    public void Btn_Save() // 저장하기
    {
        GameObject.Find("Setting").GetComponent<CanvasGroup>().interactable = false;
        GameObject temp = Instantiate(Save_Prefab);
        temp.name = "Save";
    }


    public void SoundInfoView()
    {
        SoundList = new List<string>();
        SoundList.Add("================= Sound =================");
        SoundList.Add("-------------------((http://FreeSound.org))-------------------");
        SoundList.Add("Water-Swirl-Small-23 / inspertorj");
        SoundList.Add("Glass-Smash-Bottle-C/ inspertorj");
        SoundList.Add("Music-Box-Happy-Birthday / inspertorj");
        SoundList.Add("RockingChair4 / stevelalonde");
        SoundList.Add("Metal-Lid-Thumps-01 / joedeshon");
        SoundList.Add("Gramophone / setuniman");
        SoundList.Add("=========================================");


        for (int i = 0; i < SoundList.Count; i++)
            Sound.text += SoundList[i] + "\n";

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
        GameObject.Find("Canvas_UI").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Destroy(GameObject.Find("Setting"));
       
    }
}
