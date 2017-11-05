using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class EditUIManager_Item : EditUIManager {
    public DataManager_Item dataManager;
    public GameObject pnlEdit_prefab, pnlEdit, scrollList;

    //아이템용
    public GameObject btnItem;
    public bool isNewItem = true;
    public int nClikedIndex;
    
    public override void Init() {
        displayItemList();
    }

    //Scroll View의 아이템들 보여줌
    public override void displayItemList() {
        //삭제
        for (int i = scrollList.transform.childCount - 1; i >= 0; i--) {
            Destroy(scrollList.transform.GetChild(i).gameObject);
        }
        //출력
        for (int i = 0; i < dataManager.list_item.Count; i++) {
            Item item = dataManager.list_item[i];
            GameObject btn = Instantiate(btnItem, scrollList.transform);

            btn.name = item.Key + "";
            btn.transform.GetChild(0).GetComponent<Text>().text = item.Key + "";
            btn.transform.GetChild(1).GetComponent<Image>().sprite = dataManager.LoadSpriteFromBytes(item.Img_data);
            btn.transform.GetChild(2).GetComponent<Text>().text = item.Name;
            btn.transform.GetChild(3).GetComponent<Text>().text = item.Info;

            int index = i;
            btn.GetComponent<Button>().onClick.AddListener(() => itemClick(index));
        }
    }

    //수정창 열기
    public override void editPanelSetting() {
        pnlEdit = Instantiate(pnlEdit_prefab, GameObject.Find("Canvas").transform);
        pnlEdit.name = "myEdit";

        GameObject.Find("EditItem_BtnOK").GetComponent<Button>().
            onClick.AddListener(() => editPanelOKClick());
        GameObject.Find("EditItem_BtnDelete").GetComponent<Button>().
            onClick.AddListener(() => editPanelDeleteClick());
        GameObject.Find("EditItem_exit").GetComponent<Button>().
            onClick.AddListener(() => editPanelCloseClick());
    }

    public override void editPanelSetting(object arg) {
        pnlEdit = Instantiate(pnlEdit_prefab, GameObject.Find("Canvas").transform);
        pnlEdit.name = "myEdit";

        Item item = (Item)arg;
        //아이템 정보 출력
        GameObject.Find("EditItem_key").GetComponent<InputField>().text = item.Key + "";
        GameObject.Find("EditItem_name").GetComponent<InputField>().text = item.Name;
        GameObject.Find("EditItem_info").GetComponent<InputField>().text = item.Info;
        GameObject.Find("Image").GetComponent<Image>().sprite = dataManager.LoadSpriteFromBytes(item.Img_data);

        GameObject.Find("EditItem_BtnOK").GetComponent<Button>().
            onClick.AddListener(() => editPanelOKClick());
        GameObject.Find("EditItem_BtnDelete").GetComponent<Button>().
            onClick.AddListener(() => editPanelDeleteClick());
        GameObject.Find("EditItem_exit").GetComponent<Button>().
            onClick.AddListener(() => editPanelCloseClick());
    }

    public override void CreateClick() {
        editPanelSetting();
        isNewItem = true;
        nClikedIndex = -1;
    }
    
    
    //item 클릭
    public void itemClick(int clickedIndex) {
        editPanelCloseClick();
        isNewItem = false;
        Item item = dataManager.list_item[clickedIndex];
        editPanelSetting(item);
        nClikedIndex = clickedIndex;
    }

    //수정창 OK 클릭
    public void editPanelOKClick() {
        //UI로부터 정보얻음
        Item item = new Item();
        item.Key = int.Parse(GameObject.Find("EditItem_key").GetComponent<InputField>().text);
        item.Name = GameObject.Find("EditItem_name").GetComponent<InputField>().text;
        item.Info = GameObject.Find("EditItem_info").GetComponent<InputField>().text;
        String imgPath = GameObject.Find("EditItem_img").GetComponent<InputField>().text;
        if (!imgPath.Equals("")) item.Img_data = dataManager.LoadBytefromImgPath(imgPath);
        //Create의 경우
        if (isNewItem) {
            if (isExistKey(item.Key)) return;       //키값 겹치는 경우
            dataManager.list_item.Add(item);
        }
        //기존 아이템 수정의 경우
        else {
            if (dataManager.list_item[nClikedIndex].Key != item.Key
                && isExistKey(item.Key)) return;        //키값 겹치는 경우
            dataManager.list_item[nClikedIndex].Key = item.Key;
            dataManager.list_item[nClikedIndex].Name = item.Name;
            dataManager.list_item[nClikedIndex].Info = item.Info;
            if (!imgPath.Equals("")) dataManager.list_item[nClikedIndex].Img_data = item.Img_data;

        }
        editPanelCloseClick();
        displayItemList();
    }
    //아이템삭제
    public void editPanelDeleteClick() {
        if (!isNewItem) {
            dataManager.list_item.RemoveAt(nClikedIndex);
            editPanelCloseClick();
            displayItemList();
        }

    }
    //item 수정창 Close 클릭
    public void editPanelCloseClick() {
        Destroy(pnlEdit);
    }


    //키값으로 인덱스 조사
    public bool isExistKey(int key) {
        for (int i = 0; i < dataManager.list_item.Count; i++) {
            if (dataManager.list_item[i].Key == key) {
                return true;
            }
        }
        return false;
    }
}
