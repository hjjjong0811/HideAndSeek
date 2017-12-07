using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {

    public GameObject Inventory_Prefab;
    public GameObject Setting_Prefab;
    public Image ImgItem;
    public Image ImgBattery;
    public Sprite[] Battery_ImgSource = new Sprite[5];

    public Player player;

    public bool isOpenInven = false;

    private void Start() {
        int chapter = GameManager.getInstance().GetMainChapter();
        ImgBattery = GameObject.Find("Canvas_UI").GetComponent<GameUIManager>().ImgBattery;

        if (GameManager.getInstance().isScenePlay) {
            Destroy(this.gameObject);
            Destroy(this);
        }
        Inventory inven = Inventory.getInstance();
        if (inven.curEquipItem != -1) {
            inven.equipItem(inven.curEquipItem);
        }
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void Update()
    {
        BatteryState();
    }

    public void Btn_Inven()
    {
        if (isOpenInven)
        {
            GameObject.Destroy(GameObject.Find("Inven"));
            isOpenInven = false;
        }
        else
        {
         
            GameObject test = Instantiate(Inventory_Prefab);
            test.name = "Inven";
            isOpenInven = true;
            
        }
    }

    public void Btn_Setting()
    {
        GameObject.Find("Canvas_UI").GetComponent<CanvasGroup>().interactable = false;
        GameObject temp = Instantiate(Setting_Prefab);
        temp.name = "Setting";
           
    }


    public void BatteryState() // 배터리 상태에 따른 배터리 이미지 바꾸기
    {

        float charge = FlashLight.getFlashData();
     
        if(charge <= 100 && charge >75 ) 
        {
            ImgBattery.sprite = Battery_ImgSource[0];
        }

        else if (charge <= 75 && charge > 50)
        {
            ImgBattery.sprite = Battery_ImgSource[1];
        }

        else if (charge <= 50 && charge > 25)
        {
            ImgBattery.sprite = Battery_ImgSource[2];
        }

        else if (charge <= 25 && charge > 0)
        {
            ImgBattery.sprite = Battery_ImgSource[3];
        }

        else if (charge <= 0)
        {
            ImgBattery.sprite = Battery_ImgSource[4];
        }

    }

    //현정추가
    public void DisplayEquipItem(Sprite s) {
        ImgItem.sprite = s;
    }
    public void btnActionClick() {
        player.SendMessage("action");
    }
    public void btnItemClick() {
        player.SendMessage("action_item");
    }
    public void btnFlashClick() {
        player.Flash.setLight(!player.Flash.getIsLighted());
    }

}
