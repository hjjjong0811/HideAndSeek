using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditUIManager_Compose : EditUIManager {
    public DataManager_Compose dataManager;
    public GameObject pnlEdit_prefab, pnlEdit, scrollList;
    
    public GameObject btnCompose;
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
        for (int i = 0; i < dataManager.list_Compose.Count; i++) {
            ComposeItem item = dataManager.list_Compose[i];
            GameObject btn = Instantiate(btnCompose, scrollList.transform);

            btn.name = item.Key_Compose + "";
            btn.transform.GetChild(0).GetComponent<Text>().text = item.Key_Compose + "";
            btn.transform.GetChild(2).GetComponent<Text>().text = item.Count + "";
            btn.transform.GetChild(4).GetComponent<Text>().text = item.ResultItemKey + "";

            btn.transform.GetChild(6).GetComponent<Text>().text = "";
            for (int j = 0; j < item.Count; j++) {
                btn.transform.GetChild(6).GetComponent<Text>().text += item.MaterialItemsKey[j] + " + ";
            }
            
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
        
        ComposeItem item = (ComposeItem)arg;
        //아이템 정보 출력
        GameObject.Find("EditItem_key").GetComponent<InputField>().text = item.Key_Compose + "";
        GameObject.Find("EditItem_result").GetComponent<InputField>().text = item.ResultItemKey + "";
        GameObject.Find("EditItem_material").GetComponent<InputField>().text = "";

        for (int j = 0; j < item.Count; j++) {
            if(j == item.Count -1)
                GameObject.Find("EditItem_material").GetComponent<InputField>().text += item.MaterialItemsKey[j];
            else
                GameObject.Find("EditItem_material").GetComponent<InputField>().text += item.MaterialItemsKey[j] + ",";
        }

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
        ComposeItem item = dataManager.list_Compose[clickedIndex];
        editPanelSetting(item);
        nClikedIndex = clickedIndex;
    }

    //수정창 OK 클릭
    public void editPanelOKClick() {
        //UI로부터 정보얻음
        ComposeItem item = new ComposeItem();
        item.Key_Compose = int.Parse(GameObject.Find("EditItem_key").GetComponent<InputField>().text);
        item.ResultItemKey = int.Parse(GameObject.Find("EditItem_result").GetComponent<InputField>().text);

        string[] strMaterial = GameObject.Find("EditItem_material").GetComponent<InputField>().text.Split(',');
        item.Count = strMaterial.Length;
        item.MaterialItemsKey = new int[item.Count];
        for (int i = 0; i < item.Count; i++) {
            item.MaterialItemsKey[i] = int.Parse(strMaterial[i]);
        }

        //Create의 경우
        if (isNewItem) {
            if (isExistKey(item.Key_Compose)) return;       //키값 겹치는 경우
            dataManager.list_Compose.Add(item);
        }
        //기존 아이템 수정의 경우
        else {
            if (dataManager.list_Compose[nClikedIndex].Key_Compose != item.Key_Compose
                && isExistKey(item.Key_Compose)) return;        //키값 겹치는 경우
            dataManager.list_Compose[nClikedIndex].Key_Compose = item.Key_Compose;
            dataManager.list_Compose[nClikedIndex].ResultItemKey = item.ResultItemKey;
            dataManager.list_Compose[nClikedIndex].Count = item.Count;
            dataManager.list_Compose[nClikedIndex].MaterialItemsKey = item.MaterialItemsKey;

        }
        editPanelCloseClick();
        displayItemList();
    }
    //아이템삭제
    public void editPanelDeleteClick() {
        if (!isNewItem) {
            dataManager.list_Compose.RemoveAt(nClikedIndex);
            editPanelCloseClick();
            displayItemList();
        }

    }
    //item 수정창 Close 클릭
    public void editPanelCloseClick() {
        Destroy(pnlEdit);
    }


    //키값 존재 검사
    public bool isExistKey(int key) {
        for (int i = 0; i < dataManager.list_Compose.Count; i++) {
            if (dataManager.list_Compose[i].Key_Compose == key) {
                return true;
            }
        }
        return false;
    }
}
