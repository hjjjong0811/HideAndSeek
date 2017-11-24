﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 버튼 클릭이벤트
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {
    public enum PlayerPrefsIndex { hp = 0, x = 1, y = 2, z = 3, room = 4, spot = 5 };
    public static string[] PlayerPrefsKey = {"Player_hp", "Player_x", "Player_y", "Player_z",
        "Player_Pos_room", "Player_Pos_spot"};

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
    


    [Serializable]
    class PlayerData
    {
        public String Name;
        public int MainChapter;
        public String SaveTime;
        public int SaveScene;
        public List<int> Inventory;
        public List<String> Object;
        public float P_x;
        public float P_y;
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
        GetPlayerPos();

    }

    public void GetPlayerPos()
    {
        NowScene = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.room]);
        Player_x = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x]);
        Player_y = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y]);

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

        data.Name = "주인공";
        data.SaveScene = NowScene;
        data.MainChapter = GameManager.getInstance().GetMainChapter();
        data.SaveTime = DateTime.Now.ToString("HH-mm-ss");
        data.Inventory = Inventory.getInstance().inventory;
        data.P_x = Player_x;
        data.P_y = Player_y;
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



                //SceneManager.LoadScene(data.SaveScene);

                Scene MoveScene = SceneManager.GetSceneByBuildIndex(data.SaveScene);
                SceneManager.MoveGameObjectToScene(GameObject.Find("Player"),MoveScene);
              //  SceneManager.LoadScene(data.SaveScene);

               
                    PlayerPos.x = data.P_x;
                    PlayerPos.y = data.P_y;
                    GameObject.Find("Player").transform.position = PlayerPos;
                Inventory.getInstance().inventory = data.Inventory; // 인벤토리


              
             

              


                /*
                if (GameObject.Find("Player") == false)
                {
                    GameObject temp = Instantiate(Player_Prefab, new Vector3(data.P_x, data.P_y, 0), transform.rotation);
                    temp.name = "Player";
                }
                else
                    Destroy(GameObject.Find("Player"));
                    */

                
                

                Player_Object = data.Object;



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
