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

    public ISpot[] Enemy_route_array;

    public Enemy_Data Player_Enemy_data;


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
        public int[] End;
        public int[] Find;
        public int[] Dead;
        public int[] Meet;
        public int[] FindJ;
        public int[] SaveArray;
        public bool[] Check;

        public bool E_enemy_working;//아저씨 발동상태
        public int E_Room;
        public int E_Spot;
        public int E_enemy_dest; // (Room)
        public int E_enemy_state;

        public int[] E_route_Room_array;
        public int[] E_route_Spot_array;
        public int E_route_length;
      

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
        Player_Enemy_data = new Enemy_Data();
        Player_Enemy_data = Enemy.enemy_save_data();
        data.SaveArray = new int[11];
        gameManager.get_save_data_state(data.SaveArray);

      
        Enemy_route_array = new ISpot[Player_Enemy_data._enemy_route_length];
        Player_Enemy_data._enemy_spot = new ISpot(0, 0);
        Enemy_route_array = Player_Enemy_data._enemy_route_array;

      
        
        //적정보
        data.E_enemy_working = Player_Enemy_data._enemy_working;
        data.E_Room = (int)Player_Enemy_data._enemy_spot._room;
        data.E_Spot = Player_Enemy_data._enemy_spot._spot;
        data.E_enemy_state = (int)Player_Enemy_data._enemy_state;
        data.E_enemy_dest = (int)Player_Enemy_data._enemy_dest;
        data.E_route_length = Player_Enemy_data._enemy_route_length;

        data.E_route_Room_array = new int[data.E_route_length];
        data.E_route_Spot_array = new int[data.E_route_length];

        for(int i=0;i<data.E_route_length;i++)
        {
            data.E_route_Room_array[i] = (int)Enemy_route_array[i]._room;
            data.E_route_Spot_array[i] = Enemy_route_array[i]._spot;
        }

        
        //게임 정보
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
                Player_Enemy_data = new Enemy_Data();
                Player_Enemy_data._enemy_spot = new ISpot(0, 0);
             
                
                Player_ISpot._room = (Room)data.P_Room;
                Player_ISpot._spot = data.P_Spot;
                PlayerPos.x = data.x;
                PlayerPos.y = data.y;
                PlayerPos.z = data.z;
                
                String SaveScene = Scene_Manager.scene_name[(int)Player_ISpot._room];
                SceneManager.LoadScene(SaveScene);

               
                Player_Enemy_data._enemy_working = data.E_enemy_working;
                Player_Enemy_data._enemy_spot._room = (Room)data.E_Room;
                Player_Enemy_data._enemy_spot._spot = data.E_Spot;
                Player_Enemy_data._enemy_state = (Enemy_State)data.E_enemy_state;
                Player_Enemy_data._enemy_dest = (Room)data.E_enemy_dest;
                Player_Enemy_data._enemy_route_length = data.E_route_length;

                Enemy_route_array = new ISpot[Player_Enemy_data._enemy_route_length];
               

                for(int i=0; i<Player_Enemy_data._enemy_route_length; i++)
                {
                    Enemy_route_array[i] = new ISpot(0, 0);
                    Enemy_route_array[i]._room = (Room)data.E_route_Room_array[i];
                    Enemy_route_array[i]._spot = data.E_route_Spot_array[i];
                }

                Player_Enemy_data._enemy_route_array = Enemy_route_array;


                Enemy.enemy_bring_data(Player_Enemy_data);
                Inventory.getInstance().inventory = data.Inventory;
                gamemanager.SetMainChapter(data.MainChapter);
                gamemanager.set_save_data_array(data.End, data.Find, data.Dead, data.Meet, data.FindJ, data.Check);
                gamemanager.set_save_data_state(data.SaveArray);
                FlashLight.Init(data.Battery, true);
                Player.Init(data.hp, PlayerPos, Player_ISpot);
                
                Debug.Log("load " + "room" + Player_ISpot._room + "spot" + Player_ISpot._spot);

                //현정추가, 챕터별 BGM재생
                if (data.MainChapter == -1) { }
                else if (data.MainChapter < 2) SoundManager.getInstance().playBgm(Resources.Load("Sounds/244") as AudioClip);
                else if (data.MainChapter < 4) SoundManager.getInstance().playBgm(Resources.Load("Sounds/000STARTBGM") as AudioClip);
                else SoundManager.getInstance().playBgm(Resources.Load("Sounds/BGM") as AudioClip);
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
