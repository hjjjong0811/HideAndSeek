using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public override void editPanelSetting() {
        pnlEdit = Instantiate(pnlEdit_prefab, GameObject.Find("Canvas").transform);
        pnlEdit.name = "myEdit";

        GameObject.Find("EditItem_BtnOK").GetComponent<Button>().
            onClick.AddListener(() => itemEditOKClick());
        GameObject.Find("EditItem_BtnDelete").GetComponent<Button>().
            onClick.AddListener(() => itemEditDeleteClick());
        GameObject.Find("EditItem_exit").GetComponent<Button>().
            onClick.AddListener(() => itemEditCloseClick());
    }

    public override void editPanelSetting(object arg) {
        pnlEdit = Instantiate(pnlEdit_prefab, GameObject.Find("Canvas").transform);
        pnlEdit.name = "myEdit";

        Item item = (Item)arg;
        //아이템 정보 출력
        GameObject.Find("EditItem_key").GetComponent<InputField>().text = item.Key + "";
        GameObject.Find("EditItem_img").GetComponent<InputField>().text = item.Img;
        GameObject.Find("EditItem_name").GetComponent<InputField>().text = item.Name;
        GameObject.Find("EditItem_info").GetComponent<InputField>().text = item.Info;
        int spriteIndex = dataManager.findItemSpriteByName(item.Img);
        GameObject.Find("Image").GetComponent<Image>().sprite =
            (spriteIndex == -1) ? null : dataManager.itemSprites[spriteIndex];

        GameObject.Find("EditItem_BtnOK").GetComponent<Button>().
            onClick.AddListener(() => itemEditOKClick());
        GameObject.Find("EditItem_BtnDelete").GetComponent<Button>().
            onClick.AddListener(() => itemEditDeleteClick());
        GameObject.Find("EditItem_exit").GetComponent<Button>().
            onClick.AddListener(() => itemEditCloseClick());
    }

    public override void CreateClick() {
        editPanelSetting();
        isNewItem = true;
        nClikedIndex = -1;
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
            int spriteIndex = dataManager.findItemSpriteByName(item.Img);
            btn.transform.GetChild(1).GetComponent<Image>().sprite =
                (spriteIndex == -1) ? null : dataManager.itemSprites[spriteIndex];
            btn.transform.GetChild(2).GetComponent<Text>().text = item.Name;
            btn.transform.GetChild(3).GetComponent<Text>().text = item.Info;

            btn.GetComponent<Button>().onClick.AddListener(() => itemClick(item.Key));
        }
    }
    
    //item 클릭
    public void itemClick(int item_key) {
        itemEditCloseClick();
        nClikedIndex = findListItem(item_key);  //오류체크
        if (nClikedIndex == -1) return;

        isNewItem = false;

        Item item = dataManager.list_item[nClikedIndex];
        editPanelSetting(item);
    }

    //item 수정창 OK 클릭
    public void itemEditOKClick() {
        //UI로부터 정보얻음
        Item item = new Item();
        item.Key = int.Parse(GameObject.Find("EditItem_key").GetComponent<InputField>().text);
        item.Img = GameObject.Find("EditItem_img").GetComponent<InputField>().text;
        item.Name = GameObject.Find("EditItem_name").GetComponent<InputField>().text;
        item.Info = GameObject.Find("EditItem_info").GetComponent<InputField>().text;
        //Create의 경우
        if (isNewItem) {
            if (-1 != findListItem(item.Key)) return;       //키값 겹치는 경우
            dataManager.list_item.Add(item);
        }
        //기존 아이템 수정의 경우
        else {
            if (dataManager.list_item[nClikedIndex].Key != item.Key
                && findListItem(item.Key) != -1) return;        //키값 겹치는 경우
            dataManager.list_item[nClikedIndex].Key = item.Key;
            dataManager.list_item[nClikedIndex].Img = item.Img;
            dataManager.list_item[nClikedIndex].Name = item.Name;
            dataManager.list_item[nClikedIndex].Info = item.Info;

        }
        itemEditCloseClick();
        displayItemList();
    }
    //아이템삭제
    public void itemEditDeleteClick() {
        if (nClikedIndex != -1) {
            dataManager.list_item.RemoveAt(nClikedIndex);
            itemEditCloseClick();
            displayItemList();
        }

    }
    //item 수정창 Close 클릭
    public void itemEditCloseClick() {
        Destroy(pnlEdit);
    }
    //키값으로 인덱스 조사
    public int findListItem(int key) {
        int index = -1;

        for (int i = 0; i < dataManager.list_item.Count; i++) {
            if (dataManager.list_item[i].Key == key) {
                index = i;
                break;
            }
        }
        return index;
    }
}
