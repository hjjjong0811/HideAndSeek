using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditUIManager_Object : EditUIManager {
    public DataManager_Object dataManager;
    public GameObject pnlEdit, scrollList;

    public GameObject detailScrollList, pre_detailPanel;
    public InputField txtKey, txtName, txtLength;

    public GameObject pre_btnObject;
    public bool isNewItem = true;
    public int nClikedIndex;

    public Object_Data curObject;

    public override void Init() {
        displayItemList();
        curObject = null;
    }

    //Scroll View의 아이템들 보여줌
    public override void displayItemList() {
        //삭제
        for (int i = scrollList.transform.childCount - 1; i >= 0; i--) {
            Destroy(scrollList.transform.GetChild(i).gameObject);
        }
        //출력
        for (int i = 0; i < dataManager.list_Object.Count; i++) {
            Object_Data item = dataManager.list_Object[i];
            GameObject btn = Instantiate(pre_btnObject, scrollList.transform);

            btn.name = item.Key + "";
            btn.transform.GetChild(0).GetComponent<Text>().text = item.Key + "";
            btn.transform.GetChild(1).GetComponent<Text>().text = item.Name;
            Text bot = btn.transform.GetChild(2).GetComponent<Text>();
            bot.text = item.DetailList.Count + ">>";
            for (int j = 0; j < item.DetailList.Count; j++) {
                bot.text += item.DetailList[j].StartChapter + "~";
                bot.text += item.DetailList[j].SpriteNum + ", At:";
                bot.text += item.DetailList[j].Auto.script_key + "," + item.DetailList[j].Auto.sound_key + ",";
                bot.text += item.DetailList[j].Auto.item_use_key + "," + item.DetailList[j].Auto.item_result_key + ",";

                bot.text += ", Ac:" + item.DetailList[j].Action.script_key + "," + item.DetailList[j].Action.sound_key + ",";
                bot.text += item.DetailList[j].Action.item_use_key + "," + item.DetailList[j].Action.item_result_key + ",";

                bot.text += ", Us:" + item.DetailList[j].Action.script_key + "," + item.DetailList[j].Action.sound_key + ",";
                bot.text += item.DetailList[j].Action.item_use_key + "," + item.DetailList[j].Action.item_result_key + "/";
                
            }

            int index = i;
            btn.GetComponent<Button>().onClick.AddListener(() => itemClick(index));
        }
    }

    //수정창 열기
    public override void editPanelSetting() {
        pnlEdit.SetActive(true);
        curObject = new Object_Data();

        txtKey.text = "";
        txtName.text = "";
        txtLength.text = "1";
        for (int i = detailScrollList.transform.childCount - 1; i >= 0; i--) {
            Destroy(detailScrollList.transform.GetChild(i).gameObject);
        }
        GameObject go = Instantiate(pre_detailPanel, detailScrollList.transform);
    }

    public override void editPanelSetting(object arg) {
        pnlEdit.SetActive(true);

        Object_Data item = (Object_Data)arg;

        txtKey.text = item.Key + "";
        txtName.text = item.Name;
        txtLength.text = item.DetailList.Count + "";
        
        for (int i = detailScrollList.transform.childCount - 1; i >= 0; i--) {
            Destroy(detailScrollList.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < item.DetailList.Count; i++) {
            GameObject go = Instantiate(pre_detailPanel, detailScrollList.transform);
            go.name = i+"";
            go.transform.GetChild(0).GetComponent<InputField>().text = item.DetailList[i].StartChapter +"";
            go.transform.GetChild(1).GetComponent<InputField>().text = item.DetailList[i].SpriteNum + "";
            //Auto
            go.transform.GetChild(2).GetChild(0).GetComponent<InputField>().text = item.DetailList[i].Auto.script_key + "";
            go.transform.GetChild(2).GetChild(1).GetComponent<InputField>().text = item.DetailList[i].Auto.sound_key + "";
            go.transform.GetChild(2).GetChild(2).GetComponent<InputField>().text = item.DetailList[i].Auto.item_use_key + "";
            go.transform.GetChild(2).GetChild(3).GetComponent<InputField>().text = item.DetailList[i].Auto.item_result_key + "";
            //Action
            go.transform.GetChild(3).GetChild(0).GetComponent<InputField>().text = item.DetailList[i].Auto.script_key + "";
            go.transform.GetChild(3).GetChild(1).GetComponent<InputField>().text = item.DetailList[i].Auto.sound_key + "";
            go.transform.GetChild(3).GetChild(2).GetComponent<InputField>().text = item.DetailList[i].Auto.item_use_key + "";
            go.transform.GetChild(3).GetChild(3).GetComponent<InputField>().text = item.DetailList[i].Auto.item_result_key + "";
            //UseItem
            go.transform.GetChild(4).GetChild(0).GetComponent<InputField>().text = item.DetailList[i].Auto.script_key + "";
            go.transform.GetChild(4).GetChild(1).GetComponent<InputField>().text = item.DetailList[i].Auto.sound_key + "";
            go.transform.GetChild(4).GetChild(2).GetComponent<InputField>().text = item.DetailList[i].Auto.item_use_key + "";
            go.transform.GetChild(4).GetChild(3).GetComponent<InputField>().text = item.DetailList[i].Auto.item_result_key + "";
        }
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
        Object_Data item = dataManager.list_Object[clickedIndex];
        editPanelSetting(item);
        nClikedIndex = clickedIndex;
    }

    //수정창 OK 클릭
    public override void editPanelOKClick() {
        //UI로부터 정보얻음
        Object_Data item= new Object_Data();

        item.Key = int.Parse(txtKey.text);
        item.Name = txtName.text;

        item.DetailList = new List<Object_Data_detail>();
        for (int i = 0; i < detailScrollList.transform.childCount; i++) {
            GameObject go = detailScrollList.transform.GetChild(i).gameObject;
            item.DetailList[i].StartChapter = int.Parse(go.transform.GetChild(0).GetComponent<InputField>().text);
            item.DetailList[i].SpriteNum = int.Parse(go.transform.GetChild(1).GetComponent<InputField>().text);
            //Auto
            item.DetailList[i].Auto.script_key = int.Parse(go.transform.GetChild(2).GetChild(0).GetComponent<InputField>().text);
            item.DetailList[i].Auto.sound_key = int.Parse(go.transform.GetChild(2).GetChild(1).GetComponent<InputField>().text);
            item.DetailList[i].Auto.item_use_key = int.Parse(go.transform.GetChild(2).GetChild(2).GetComponent<InputField>().text);
            item.DetailList[i].Auto.item_result_key = int.Parse(go.transform.GetChild(2).GetChild(3).GetComponent<InputField>().text);
            //Action                                                            
            item.DetailList[i].Action.script_key = int.Parse(go.transform.GetChild(3).GetChild(0).GetComponent<InputField>().text);
            item.DetailList[i].Action.sound_key = int.Parse(go.transform.GetChild(3).GetChild(1).GetComponent<InputField>().text);
            item.DetailList[i].Action.item_use_key = int.Parse(go.transform.GetChild(3).GetChild(2).GetComponent<InputField>().text);
            item.DetailList[i].Action.item_result_key = int.Parse(go.transform.GetChild(3).GetChild(3).GetComponent<InputField>().text);
            //UseItem                                                           
            item.DetailList[i].UseItem.script_key = int.Parse(go.transform.GetChild(4).GetChild(0).GetComponent<InputField>().text);
            item.DetailList[i].UseItem.sound_key = int.Parse(go.transform.GetChild(4).GetChild(2).GetComponent<InputField>().text);
            item.DetailList[i].UseItem.item_use_key = int.Parse(go.transform.GetChild(4).GetChild(3).GetComponent<InputField>().text);
            item.DetailList[i].UseItem.item_result_key = int.Parse(go.transform.GetChild(4).GetChild(1).GetComponent<InputField>().text);
        }

        //Create의 경우
        if (isNewItem) {
            if (isExistKey(item.Key)) return;       //키값 겹치는 경우
            dataManager.list_Object.Add(item);
        }
        //기존 아이템 수정의 경우
        else {
            if (dataManager.list_Object[nClikedIndex].Key != item.Key
                && isExistKey(item.Key)) return;        //키값 겹치는 경우

            dataManager.list_Object[nClikedIndex].Key = item.Key;
            dataManager.list_Object[nClikedIndex].Name = item.Name;
            dataManager.list_Object[nClikedIndex].DetailList = item.DetailList;
        }
        editPanelCloseClick();
        displayItemList();
    }
    //아이템삭제
    public override void editPanelDeleteClick() {
        if (!isNewItem) {
            dataManager.list_Object.RemoveAt(nClikedIndex);
            editPanelCloseClick();
            displayItemList();
        }

    }
    //item 수정창 Close 클릭
    public override void editPanelCloseClick() {
        pnlEdit.SetActive(false);
    }


    //키값 존재 조사
    public bool isExistKey(int key) {
        for (int i = 0; i < dataManager.list_Object.Count; i++) {
            if (dataManager.list_Object[i].Key == key) {
                return true;
            }
        }
        return false;
    }

    public void OnLengthChange() {
        if (int.Parse(txtLength.text) < 1) {
            txtLength.text = "1";
        }
        List<Object_Data_detail> temp = new List<Object_Data_detail>();
        for (int i = 0; i < detailScrollList.transform.childCount; i++) {
            GameObject go = detailScrollList.transform.GetChild(i).gameObject;
            temp[i].StartChapter = int.Parse(go.transform.GetChild(0).GetComponent<InputField>().text);
            temp[i].SpriteNum = int.Parse(go.transform.GetChild(1).GetComponent<InputField>().text);
            //Auto
            temp[i].Auto.script_key = int.Parse(go.transform.GetChild(2).GetChild(0).GetComponent<InputField>().text);
            temp[i].Auto.sound_key = int.Parse(go.transform.GetChild(2).GetChild(1).GetComponent<InputField>().text);
            temp[i].Auto.item_use_key = int.Parse(go.transform.GetChild(2).GetChild(2).GetComponent<InputField>().text);
            temp[i].Auto.item_result_key = int.Parse(go.transform.GetChild(2).GetChild(3).GetComponent<InputField>().text);
            //Action                                                            
            temp[i].Action.script_key = int.Parse(go.transform.GetChild(3).GetChild(0).GetComponent<InputField>().text);
            temp[i].Action.sound_key = int.Parse(go.transform.GetChild(3).GetChild(1).GetComponent<InputField>().text);
            temp[i].Action.item_use_key = int.Parse(go.transform.GetChild(3).GetChild(2).GetComponent<InputField>().text);
            temp[i].Action.item_result_key = int.Parse(go.transform.GetChild(3).GetChild(3).GetComponent<InputField>().text);
            //UseItem                                                           
            temp[i].UseItem.script_key = int.Parse(go.transform.GetChild(4).GetChild(0).GetComponent<InputField>().text);
            temp[i].UseItem.sound_key = int.Parse(go.transform.GetChild(4).GetChild(2).GetComponent<InputField>().text);
            temp[i].UseItem.item_use_key = int.Parse(go.transform.GetChild(4).GetChild(3).GetComponent<InputField>().text);
            temp[i].UseItem.item_result_key = int.Parse(go.transform.GetChild(4).GetChild(1).GetComponent<InputField>().text);
        }
        for (int i = detailScrollList.transform.childCount - 1; i >= 0; i--) {
            Destroy(detailScrollList.transform.GetChild(i).gameObject);
        }

        int len = int.Parse(txtLength.text);

        for (int i = 0; i < len; i++) {
            GameObject go = Instantiate(pre_detailPanel, detailScrollList.transform);

            if (i < temp.Count) {
                go.name = i + "";
                go.transform.GetChild(0).GetComponent<InputField>().text = temp[i].StartChapter + "";
                go.transform.GetChild(1).GetComponent<InputField>().text = temp[i].SpriteNum + "";
                //Auto
                go.transform.GetChild(2).GetChild(0).GetComponent<InputField>().text = temp[i].Auto.script_key + "";
                go.transform.GetChild(2).GetChild(1).GetComponent<InputField>().text = temp[i].Auto.sound_key + "";
                go.transform.GetChild(2).GetChild(2).GetComponent<InputField>().text = temp[i].Auto.item_use_key + "";
                go.transform.GetChild(2).GetChild(3).GetComponent<InputField>().text = temp[i].Auto.item_result_key + "";
                //Action        
                go.transform.GetChild(3).GetChild(0).GetComponent<InputField>().text = temp[i].Auto.script_key + "";
                go.transform.GetChild(3).GetChild(1).GetComponent<InputField>().text = temp[i].Auto.sound_key + "";
                go.transform.GetChild(3).GetChild(2).GetComponent<InputField>().text = temp[i].Auto.item_use_key + "";
                go.transform.GetChild(3).GetChild(3).GetComponent<InputField>().text = temp[i].Auto.item_result_key + "";
                //UseItem                   
                go.transform.GetChild(4).GetChild(0).GetComponent<InputField>().text = temp[i].Auto.script_key + "";
                go.transform.GetChild(4).GetChild(1).GetComponent<InputField>().text = temp[i].Auto.sound_key + "";
                go.transform.GetChild(4).GetChild(2).GetComponent<InputField>().text = temp[i].Auto.item_use_key + "";
                go.transform.GetChild(4).GetChild(3).GetComponent<InputField>().text = temp[i].Auto.item_result_key + "";
            }
        }
    }
}
