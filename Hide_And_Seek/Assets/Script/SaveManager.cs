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

    Inventory inven = Inventory.getInstance();
    GameManager gameManager = GameManager.getInstance();
 

    /*플레이어 정보*/
    public static String Player_Name;

    /*플레이어 위치정보*/
    public static Vector3 PlayerPos;
    public static float Player_x;
    public static float Player_y;
    public static int Player_Scene;

    /*게임정보*/
    public static float Player_Battery; // 배터리잔량
    

    // public static int Enemy_Spot; // 아저씨 위치


    /*구성 화면표기*/

    //버튼
    public Button Btn_Save;
    public Button Btn_Load;
    public Button Btn_Delete;

    public String tempName = "";

    public Text[] Text_Name = new Text[3]; // 플레이어 이름
    public Text[] Text_Chapter = new Text[3]; // 게임 챕터
    public Text[] Text_SaveTime = new Text[3]; // 저장시간
    public GameObject[] Check = new GameObject[3]; // 체크마크
    
    
    public static int SlotNumber;
    public int Slotflag = 0;


    [Serializable]
    public class PlayerData
    {
        public String Name;
        public String SaveTime;
        public int MainChapter;
        public int P_Scene;
        public float P_x;
        public float P_y;
        public float Battery;
        public List<int> Inventory;
    }



    public void Start()
    {
        
        for (int i = 0; i < 3; i++) // 체크마크 모두 제거
            Check[i].SetActive(false);

        DataLoad(); // 데이터 불러오기

        PlayerPos = transform.position;
        
        /*모든버튼 비활성화*/
        Btn_Save.GetComponent<Button>().interactable = false;
        Btn_Load.GetComponent<Button>().interactable = false;
        Btn_Delete.GetComponent<Button>().interactable = false;


    }


    public void Update()
    {
        
        FileExist();
        CheckSlot();
        DataLoad();
        gameManager.CheckMainChapter();

        Player_x = GameObject.Find("Player").transform.position.x;
        Player_y = GameObject.Find("Player").transform.position.y;

       
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
      
        if (File.Exists(Application.persistentDataPath + "/" + SlotNumber + ".dat")) // 데이터 저장 파일이 있으면
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
        data.MainChapter = gameManager.GetMainChapter(); // 현재 챕터저장
        data.SaveTime = DateTime.Now.ToString("HH-mm-ss"); // 현재시간 저장
        data.P_x = Player_x;
        data.P_y = Player_y;
        data.P_Scene = SceneManager.GetActiveScene().buildIndex; // 현재 씬 넘버 가져오기
        data.Battery = Player_Battery;
        data.Inventory = inven.inventory;
       
        bf.Serialize(file, data);
        file.Close();
        

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
     
        if (File.Exists(Application.persistentDataPath + "/" + SlotNumber + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + SlotNumber + ".dat", FileMode.Open);

            if (file != null && file.Length > 0)
            {
                PlayerData data = (PlayerData)bf.Deserialize(file);

                /*위치 세팅*/

                //Player_Scene = data.P_Scene; // 씬정보 불러와 세팅
                //SceneManager.LoadScene(Player_Scene);

                PlayerPos.x = data.P_x; 
                PlayerPos.y = data.P_y;
                GameObject.Find("Player").transform.position = PlayerPos;

              
                
                gameManager.SetMainChapter(data.MainChapter); // 챕터불러와 세팅
                
                inven.inventory = data.Inventory;
               
                Player_Battery = data.Battery; // 배터리 잔량 불러와 세팅
           
            }

            file.Close();
        }


    }



    public void Btn_DeleteData() // 데이터삭제
    {
        File.Delete(Application.persistentDataPath + "/" + SlotNumber + ".dat");

        Text_Name[SlotNumber - 1].text = "name. ";
        Text_Chapter[SlotNumber - 1].text = "ep. ";
        Text_SaveTime[SlotNumber - 1].text = "Save.T. ";
        
    }

    public void Btn_Off() // 세이브 창 끄기
    {
        GameObject.Destroy(GameObject.Find("Save"));
    }

}
