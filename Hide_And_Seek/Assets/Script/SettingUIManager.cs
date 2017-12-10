using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUIManager : MonoBehaviour {

    public GameObject btnMenuClose;
    public GameObject InfoPanel; // 개발자 정보 패널(음악 리소스...)
    public GameObject Sound_Prefab; // 사운드 설정
    public GameObject Save_Prefab;
    public Button btn_save; // 저장버튼

    public List<string> SoundList;
    public Text Sound;

    public void Start()
    {
        InfoPanel.SetActive(false);
        SoundInfoView();

        if (Enemy.get_enemy_chasing()) // 아저씨 chasing 이면 저장못함
        {
            btn_save.GetComponent<Button>().interactable = false;
        }
        else
            btn_save.GetComponent<Button>().interactable = true;

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
        SoundList.Add("\n");
        SoundList.Add("===========================");
        SoundList.Add("((http://FreeSound.org))");
        SoundList.Add("\"Bird Whistling, A.wav\" by InspectorJ");
        SoundList.Add("\"Water-Swirl-Small-23\" by inspertorJ");
        SoundList.Add("\"Glass-Smash-Bottle-C\" by inspertorJ");
        SoundList.Add("\"Music-Box-Happy-Birthday\" by inspertorJ");
        SoundList.Add("\"RockingChair4\" by stevelalonde");
        SoundList.Add("\"Metal-Lid-Thumps-01\" by joedeshon");
        SoundList.Add("\"Gramophone\" by setuniman");
        SoundList.Add("\"Camera-snap1\" by thecheeseman");
        SoundList.Add("\"horror-ambience-10\" by klankbeeld");
        SoundList.Add("\"blood-hitting-window\" by rock-savage");
        SoundList.Add("\"under-the-stars-loop\" by shadydave");
        SoundList.Add("===========================");
        SoundList.Add("((http:earbro.com))");
        SoundList.Add("\"A Shy smile\" by earbro");
        SoundList.Add("\"Hide And Seek\" by earbro");
        SoundList.Add("===========================");


        for (int i = 0; i < SoundList.Count; i++)
            Sound.text += SoundList[i] + "\n";

    }
    public void Btn_Info() // 개발자 정보
    {
        btnMenuClose.SetActive(false);
        InfoPanel.SetActive(true);
        
    }

    public void Btn_InfoOff()
    {
        btnMenuClose.SetActive(true);
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
