using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 버튼 클릭이벤트
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {

    

    /* 구성 */
    public Button Btn_Save;
    public Button Btn_Load;
    public Button Btn_Delete;

    public String tempName = "";
    public int tempChapter = 3;
    public String tempSave = "";


    /*화면표기*/

    public Text[] Text_Name = new Text[3]; // 플레이어 이름
    public Text[] Text_Chapter = new Text[3]; // 게임 챕터
    public Text[] Text_SaveTime = new Text[3]; // 저장시간
  

    /*체크마크*/
    public GameObject[] Check = new GameObject[3];
    



    public static int SlotNumber;
    public int Slotflag = 0;





    [Serializable]
    public class PlayerData
    {
        public String Name;
        public String SaveTime;
        public int MainChapter;
    }
    



    public void Start()
    {
        
        tempName = "정원";
        tempChapter = 3;
       

        /*모든버튼 비활성화*/
        Btn_Save.GetComponent<Button>().interactable = false;
        Btn_Load.GetComponent<Button>().interactable = false;
        Btn_Delete.GetComponent<Button>().interactable = false;

        for (int i = 0; i < 3; i++)
        {
            Check[i].SetActive(false);
        }

        DataLoad();
    }


    public void Update()
    {
        FileExist();
        DataLoad();
        CheckSlot();
        tempSave = DateTime.Now.ToString("HH-mm-ss");
    }

    public void Btn_Slot() // 슬롯 눌렀을때
    {
        String ClickSlot = EventSystem.current.currentSelectedGameObject.name;
        String SlotName = ClickSlot.Substring(4, 1);

        SlotNumber = int.Parse(SlotName);
        Debug.Log(SlotNumber);
        Slotflag = 1;
    }

    public void CheckSlot() // 체크 마크
    {
        if(Slotflag == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                Check[i].SetActive(false);
            }

            Check[SlotNumber-1].SetActive(true);
           
        }

    }

    public bool FileExist() // 파일여부에 따른 버튼상태 변경
    {
        bool SlotFileExist = false; // 파일 존재 여부

        if (SlotFileExist = File.Exists(Application.persistentDataPath + "/" + SlotNumber + ".dat")) // 데이터 저장 파일이 있으면
        {
            Btn_Save.interactable = true;
            Btn_Load.interactable = true;
            Btn_Delete.interactable = true;
            return true;
        }
        else // 데이터 파일이 없으면
        {
            if (Slotflag == 0) // 안눌렸으면 세이브불가
            {
                Btn_Save.interactable = false;

            }
            else // 눌렸으면 세이브가능
            {
                Btn_Save.interactable = true;

            }

            Btn_Load.interactable = false;
            Btn_Delete.interactable = false;
            return false;
        }

    }

    public void Btn_SaveData() // 저장하기
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + SlotNumber + ".dat");

        PlayerData data = new PlayerData();
        
        data.Name = tempName;
        data.MainChapter = tempChapter;
        data.SaveTime = tempSave;
        
        bf.Serialize(file, data);
        file.Close();


        Debug.Log("저장" + SlotNumber + " / " + data.Name );

    }

    public void DataLoad() // 데이터 기존에 있으면 불러옴!
    {
        for (int i=1; i<4; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/"+ i + ".dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/"+i+".dat", FileMode.Open);

                if (file != null && file.Length > 0)
                {
                    PlayerData data = (PlayerData)bf.Deserialize(file);

                    Text_Name[i-1].text = "name." + data.Name;
                    Text_Chapter[i-1].text = "ep. " + data.MainChapter.ToString();
                    Text_SaveTime[i-1].text = "Save.T " + data.SaveTime;

                }

                file.Close();
            }

        }
     

    }

    public void Btn_LoadData() // 게임정보 불러오기
    {
      

    }



    public void Btn_DeleteData() // 데이터삭제
    {
        File.Delete(Application.persistentDataPath + "/" + SlotNumber + ".dat");

        Text_Name[SlotNumber - 1].text = "name. ";
        Text_Chapter[SlotNumber - 1].text = "ep. ";
        Text_SaveTime[SlotNumber - 1].text = "Save.T. ";
        
    }

    public void Btn_Off() // 이전 씬으로 돌아가기
    {

        GameObject.Destroy(GameObject.Find("Save"));
    }

}
