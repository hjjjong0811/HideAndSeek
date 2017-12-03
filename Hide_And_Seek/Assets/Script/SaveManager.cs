using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 버튼 클릭이벤트
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{


    /*화면구성용*/
    public Text[] Text_Chapter = new Text[3];
    public Text[] Text_SaveTime = new Text[3];

    public Button Btn_Save;
    public Button Btn_Load;
    public Button Btn_Delete;

    public GameObject[] Img_CheckMark = new GameObject[3];
    public bool isSlot = false;
    public int SlotNumber = 0;


    /*저장용*/
    
    public Vector3 PlayerPos;
    public int Player_Spot;

    public Room Player_Room;
    public Room Enemy_Room;

    public ISpot Player_ISpot;
    public ISpot Enemy_ISpot;


    [Serializable]
    class PlayerData
    {
        public int MainChapter;
        public String SaveTime;
        public List<int> Inventory;
        public float x;
        public float y;
        public float z;
        public float hp;
        public float Battery;
        public int P_Room;
        public int P_Spot;
        public int E_Room;
        public int E_Spot;
        public int[] End;
        public int[] Find;
        public int[] Dead;
        public int[] Meet;
        public int[] FindJ;
        public int[] SaveArray;
        public bool[] Check;


    }


    public void Start()
    {
        //저장오류날때이걸로 일단 모든파일지우고 그담에 다시 제거한후에 테스트하세욤
        /*
        File.Delete(Application.persistentDataPath + "/0.dat");
        File.Delete(Application.persistentDataPath + "/1.dat");
        File.Delete(Application.persistentDataPath + "/2.dat");
        File.Delete(Application.persistentDataPath + "/3.dat");
        */


        Btn_Save.GetComponent<Button>().interactable = false;
        Btn_Load.GetComponent<Button>().interactable = false;
        Btn_Delete.GetComponent<Button>().interactable = false;
        
        isSlot = false;
        SlotNumber = 0;

        for (int i = 0; i < 3; i++)
            Img_CheckMark[i].SetActive(false);


        DataCheck();
        StateButtonChange();
    }

    public void Update()
    {
        DataCheck();
        StateButtonChange();

    }



    public void DataCheck() // 기존 데이터 확인
    {
        for (int i = 1; i < 4; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/" + i + ".dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + i + ".dat", FileMode.Open);

                if (file != null && file.Length > 0)
                {
                    PlayerData data = (PlayerData)bf.Deserialize(file);

                    Text_Chapter[i - 1].text = "ep. " + data.MainChapter.ToString();
                    Text_SaveTime[i - 1].text = "Save.T " + data.SaveTime;

                }

                file.Close();
            }
        }


    }

    public void Btn_Slot() // 슬롯 눌렀을때
    {
        String Slot = EventSystem.current.currentSelectedGameObject.name;
        String SlotName = Slot.Substring(4, 1);


        SlotNumber = int.Parse(SlotName);

        for (int i = 0; i < 3; i++)
            Img_CheckMark[i].SetActive(false);

        Img_CheckMark[SlotNumber - 1].SetActive(true);
        isSlot = true;

        StateButtonChange();

    }

    public void StateButtonChange() // 상태에 따른 버튼바꾸기
    {
        bool FileExists = File.Exists(Application.persistentDataPath + "/" + SlotNumber + ".dat"); // 파일 존재여부.

        if (isSlot) // 버튼 눌렀나.
        {
            if (SceneManager.GetActiveScene().name == "UI_Start") // 스타트일때
            {
                if (FileExists)
                {
                    Btn_Save.interactable = false;
                    Btn_Load.interactable = true;
                    Btn_Delete.interactable = true;
                }

                else
                {
                    Btn_Save.interactable = false;
                    Btn_Load.interactable = false;
                    Btn_Delete.interactable = false;
                }

            }

            else // 스타트 아닐때
            {
                if (FileExists) // 파일이 있으면 
                {
                    Btn_Save.interactable = true;
                    Btn_Load.interactable = true;
                    Btn_Delete.interactable = true;
                }

                else
                {
                    Btn_Save.interactable = true;
                    Btn_Load.interactable = false;
                    Btn_Delete.interactable = false;
                }
            }

        }
        else
        {
            Btn_Save.interactable = false;
            Btn_Load.interactable = false;
            Btn_Delete.interactable = false;
        }
    }

    public void Btn_SaveData() // 데이터 저장하기
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + SlotNumber + ".dat");

        PlayerData data = new PlayerData();

        
        GameManager gameManager = GameManager.getInstance();
        Player.getPlayerData(ref data.hp, ref PlayerPos, ref Player_ISpot);

        data.SaveArray = new int[9];
        gameManager.get_save_data_state(data.SaveArray);

        data.End = gameManager.EndScene;
        data.Find = gameManager.FindCharacter;
        data.Dead = gameManager.DeadCharacter;
        data.Meet = gameManager.MeetCharacter;
        data.FindJ = gameManager.FindJeongyeon;
        data.Check = gameManager.isOverlap;
        data.Battery = FlashLight.getFlashData();
        data.SaveTime = DateTime.Now.ToString("yyyy/MM/dd/HH:mm");
        data.MainChapter = gameManager.GetMainChapter();
        data.x = PlayerPos.x;
        data.y = PlayerPos.y;
        data.z = PlayerPos.z;
        data.Inventory = Inventory.getInstance().inventory;
        data.P_Room = (int)Player_ISpot._room;
        data.P_Spot = Player_ISpot._spot;
        // data.E_Room = (int)Enemy_ISpot._room;

        // data.E_Spot = Enemy_ISpot._spot;


  

        bf.Serialize(file, data);
        file.Close();
    }

    public void Btn_LoadData() // 데이터 불러오기
    {


        if (File.Exists(Application.persistentDataPath + "/" + SlotNumber + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + SlotNumber + ".dat", FileMode.Open);

            if (file != null && file.Length > 0)
            {
                PlayerData data = (PlayerData)bf.Deserialize(file);

                GameManager gamemanager = GameManager.getInstance();

                Player_ISpot = new ISpot(0, 0);
                Player_ISpot._room = (Room)data.P_Room;
                Player_ISpot._spot = data.P_Spot;

                String SaveScene = Scene_Manager.scene_name[(int)Player_ISpot._room];
                SceneManager.LoadScene(SaveScene);

                
                PlayerPos.x = data.x;
                PlayerPos.y = data.y;
                PlayerPos.z = data.z;

                // Enemy_ISpot._room = (Room)data.E_Room;
                //Enemy_ISpot._spot = data.E_Spot;

           
                Inventory.getInstance().inventory = data.Inventory;
                gamemanager.SetMainChapter(data.MainChapter);
                gamemanager.set_save_data_array(data.End, data.Find, data.Dead, data.Meet, data.FindJ, data.Check);
                gamemanager.set_save_data_state(data.SaveArray);
                FlashLight.Init(data.Battery, true);
                Player.Init(data.hp, PlayerPos, Player_ISpot);
                
                Debug.Log("load " + "room" + Player_ISpot._room + "spot" + Player_ISpot._spot);
            

            }

            file.Close();
        }

    }


    public void Btn_DeleteData() // 데이터삭제
    {
        File.Delete(Application.persistentDataPath + "/" + SlotNumber + ".dat");

        Text_Chapter[SlotNumber - 1].text = "ep. ";
        Text_SaveTime[SlotNumber - 1].text = "Save.T. ";

    }

    public void Btn_Off() // 세이브 창 끄기
    {
        if (GameObject.Find("Setting"))
        {
            GameObject.Find("Setting").GetComponent<CanvasGroup>().interactable = true;
        }
        else if (SceneManager.GetActiveScene().name == "UI_Start")
        {
            GameObject.Find("Canvas_Start").GetComponent<CanvasGroup>().interactable = true;

        }
        GameObject.Destroy(GameObject.Find("Save"));
    }
}
