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


    /*화면구성용*/
    public Text[] Text_Name = new Text[3];
    public Text[] Text_Chapter = new Text[3];
    public Text[] Text_SaveTime = new Text[3];

    public Button Btn_Save;
    public Button Btn_Load;
    public Button Btn_Delete;

    public GameObject[] Img_CheckMark = new GameObject[3];
    public int SlotNumber = 0;

    public GameObject Player_Prefab;

   
    public Vector3 PlayerPos;
    public int NowScene = 0;
    public float Player_x;
    public float Player_y;
    public List<String> Player_Object;
    public float Player_hp;
    public ISpot Player_Ispot;
    public int Player_Spot;
    public ISpot Enemy_Ispot;



    [Serializable]
    class PlayerData
    {
        public String Name;
        public int MainChapter;
        public String SaveTime;
        public int SaveScene;
        public List<int> Inventory;
        public List<String> Object;
        public float x;
        public float y;
        public float z;
        public float hp;
        public int P_Ispot;
    //    public ISpot E_Ispot;
    }


    public void Start()
    {

        Btn_Save.GetComponent<Button>().interactable = false;
        Btn_Load.GetComponent<Button>().interactable = false;
        Btn_Delete.GetComponent<Button>().interactable = false;

        for (int i=0; i<3; i++)
            Img_CheckMark[i].SetActive(false);
        
       
        DataCheck();
    }

    public void Update()
    {
        DataCheck();
        StateButtonChange();

    }



    public void DataCheck() // 기존 데이터 확인
    {
        for(int i=1; i<4;i++)
        {
            if(File.Exists(Application.persistentDataPath + "/" + i + ".dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + i + ".dat", FileMode.Open);

                if (file != null && file.Length > 0)
                {
                    PlayerData data = (PlayerData)bf.Deserialize(file);

                    Text_Name[i - 1].text = "name." + data.Name;
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

        StateButtonChange();

    }

    public void StateButtonChange() // 상태에 따른 버튼바꾸기
    {
        if (File.Exists(Application.persistentDataPath + "/" + SlotNumber + ".dat"))
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

    public void Btn_SaveData() // 데이터 저장하기
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + SlotNumber + ".dat");

        PlayerData data = new PlayerData();

        Player.getPlayerData(ref Player_hp,ref PlayerPos,ref Player_Ispot);
        
        data.Name = "주인공";
        data.hp = Player_hp;
        data.SaveScene = NowScene;
        data.MainChapter = GameManager.getInstance().GetMainChapter();
        data.SaveTime = DateTime.Now.ToString("HH-mm-ss");
        data.Inventory = Inventory.getInstance().inventory;
        data.x = PlayerPos.x;
        data.y = PlayerPos.y;
        data.z = PlayerPos.z;
        
        //data.P_Ispot = Player_Ispot;
        //data.E_Ispot = Enemy.get_enemy_spot();
        data.Object = Player_Object;
        

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

                PlayerPos.x = data.x;
                PlayerPos.y = data.y;
                PlayerPos.z = data.z;
               // Player_Ispot = (ISpot)data.P_Ispot;

                Player.Init(data.hp, PlayerPos, Player_Ispot);
                Inventory.getInstance().inventory = data.Inventory; // 인벤토리



            }

            file.Close();
        }

    }

    public void AddActiveObject()
    {
        String Active = "1/shoes/true"; // 오브젝트발동시 날라올 스트링

        if (!Player_Object.Contains(Active)) // 리스트에 없으면 추가
            Player_Object.Add(Active);

    }


    public List<String> GetObject()
    {
        return Player_Object;
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
