using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class EditUIManager_Script : EditUIManager {
    public DataManager_Script dataManager;
    public GameObject pnlEdit, scrollList;
    public GameObject scriptScrollList, txtScript;

    public List<InputField> inputlistScript;
    public InputField txtKey, txtName, txtObjImg, txtPicturePath, txtLength;
    public Image imgObj, imgPicture;

    //스크립트용
    public GameObject btnScript;
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
        for (int i = 0; i < dataManager.list_script.Count; i++) {
            Script script = dataManager.list_script[i];
            GameObject btn = Instantiate(btnScript, scrollList.transform);

            //test
            //btn.name = item.Key + "";
            //btn.transform.GetChild(0).GetComponent<Text>().text = item.Key + "";
            //btn.transform.GetChild(1).GetComponent<Image>().sprite = dataManager.LoadSpriteFromBytes(item.Img_data);
            //btn.transform.GetChild(2).GetComponent<Text>().text = item.Name;
            //btn.transform.GetChild(3).GetComponent<Text>().text = item.Info;

            int index = i;
            btn.GetComponent<Button>().onClick.AddListener(() => itemClick(index));
        }
    }

    //수정창 열기
    public override void editPanelSetting() {
        pnlEdit.SetActive(true);
    }

    public override void editPanelSetting(object arg) {
        pnlEdit.SetActive(true);

        Script item = (Script)arg;
        //test
        ////아이템 정보 출력
        //GameObject.Find("EditItem_key").GetComponent<InputField>().text = item.Key + "";
        //GameObject.Find("EditItem_name").GetComponent<InputField>().text = item.Name;
        //GameObject.Find("EditItem_info").GetComponent<InputField>().text = item.Info;
        //GameObject.Find("Image").GetComponent<Image>().sprite = dataManager.LoadSpriteFromBytes(item.Img_data);
    }

    public override void CreateClick() {
        editPanelSetting();
        isNewItem = true;
        nClikedIndex = -1;
    }


    //item 클릭
    public override void itemClick(int clickedIndex) {
        editPanelCloseClick();
        isNewItem = false;
        Script script = dataManager.list_script[clickedIndex];
        editPanelSetting(script);
        nClikedIndex = clickedIndex;
    }

    //수정창 OK 클릭
    public override void editPanelOKClick() {
        //UI로부터 정보얻음
        Script scrpit = new Script();

        //test
        //item.Key = int.Parse(GameObject.Find("EditItem_key").GetComponent<InputField>().text);
        //item.Name = GameObject.Find("EditItem_name").GetComponent<InputField>().text;
        //item.Info = GameObject.Find("EditItem_info").GetComponent<InputField>().text;
        //String imgPath = GameObject.Find("EditItem_img").GetComponent<InputField>().text;
        //if (!imgPath.Equals("")) item.Img_data = dataManager.LoadBytefromImgPath(imgPath);

        //Create의 경우
        if (isNewItem) {
            if (isExistKey(scrpit.Key)) return;       //키값 겹치는 경우
            dataManager.list_script.Add(scrpit);
        }
        //기존 아이템 수정의 경우
        else {
            if (dataManager.list_script[nClikedIndex].Key != scrpit.Key
                && isExistKey(scrpit.Key)) return;        //키값 겹치는 경우

            //test
            //dataManager.list_script[nClikedIndex].Key = item.Key;
            //dataManager.list_script[nClikedIndex].Name = item.Name;
            //dataManager.list_script[nClikedIndex].Info = item.Info;
            //if (!imgPath.Equals("")) dataManager.list_script[nClikedIndex].Img_data = item.Img_data;

        }
        editPanelCloseClick();
        displayItemList();
    }
    //아이템삭제
    public override void editPanelDeleteClick() {
        if (!isNewItem) {
            dataManager.list_script.RemoveAt(nClikedIndex);
            editPanelCloseClick();
            displayItemList();
        }

    }
    //item 수정창 Close 클릭
    public override void editPanelCloseClick() {
        Destroy(pnlEdit);
    }


    //키값 존재 조사
    public bool isExistKey(int key) {
        for (int i = 0; i < dataManager.list_script.Count; i++) {
            if (dataManager.list_script[i].Key == key) {
                return true;
            }
        }
        return false;
    }
}
