using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    DataManager dataManager;
    InputField inputPath;
    Dropdown dropdown;
    public GameObject pnlEdit_item, scrollList;

    //아이템용
    public GameObject btnItem;
    public bool isNewItem = true;
    public int nClikedIndex;

    private void Start() {
        dataManager = GameObject.Find("Manager").GetComponent<DataManager>();
        inputPath = GameObject.Find("InputPath").GetComponent<InputField>();
        dropdown = GameObject.Find("Dropdown").GetComponent<Dropdown>();

        pnlEdit_item.SetActive(false);
    }

    public void OpenClick() {
        DataManager.curCode = dropdown.value;
        dataManager.LoadData(inputPath.text);

        if(DataManager.curCode == dataManager.code_item) {
            displayItemList();
        }
    }
    public void saveClick() {
        dataManager.SaveData(inputPath.text);
    }
    public void createClick() {
        if (DataManager.curCode == dataManager.code_item) {
            pnlEdit_item.SetActive(true);
            isNewItem = true;
            nClikedIndex = -1;
            GameObject.Find("EditItem_key").GetComponent<InputField>().text = "";
            GameObject.Find("EditItem_img").GetComponent<InputField>().text = "";
            GameObject.Find("EditItem_name").GetComponent<InputField>().text = "";
            GameObject.Find("EditItem_info").GetComponent<InputField>().text = "";
            GameObject.Find("Image").GetComponent<Image>().sprite = null;
        }
    }


    //Item에 관련된 부분

    //Scroll View의 아이템들 보여줌
    public void displayItemList() {
        //삭제
        for (int i = scrollList.transform.childCount - 1; i >= 0; i--) {
            Destroy(scrollList.transform.GetChild(i).gameObject);
        }
        //출력
        for (int i=0; i<DataManager.list_item.Count; i++) {
            Item item = DataManager.list_item[i];
            GameObject btn = Instantiate(btnItem, scrollList.transform);
            btn.name = item.Key + "";
            btn.transform.GetChild(0).GetComponent<Text>().text = item.Key + "";
            int spriteIndex = DataManager.findItemSpriteByName(item.Img);
            btn.transform.GetChild(1).GetComponent<Image>().sprite = 
                (spriteIndex == -1)? null:DataManager.itemSprites[spriteIndex];
            btn.transform.GetChild(2).GetComponent<Text>().text = item.Name;
            btn.transform.GetChild(3).GetComponent<Text>().text = item.Info;
            
            btn.GetComponent<Button>().onClick.AddListener(()=>itemClick(item.Key));
            Debug.Log(item.Img);
        }
    }
    //item 클릭
    public void itemClick(int item_key) {
        nClikedIndex = findListItem(item_key);  //오류체크
        if (nClikedIndex == -1) return;

        isNewItem = false;

        //아이템 정보 출력
        pnlEdit_item.SetActive(true);
        Item item = DataManager.list_item[nClikedIndex];
        GameObject.Find("EditItem_key").GetComponent<InputField>().text = item.Key +"";
        GameObject.Find("EditItem_img").GetComponent<InputField>().text = item.Img;
        GameObject.Find("EditItem_name").GetComponent<InputField>().text = item.Name;
        GameObject.Find("EditItem_info").GetComponent<InputField>().text = item.Info;
        int spriteIndex = DataManager.findItemSpriteByName(item.Img);
        GameObject.Find("Image").GetComponent<Image>().sprite = 
            (spriteIndex == -1) ? null : DataManager.itemSprites[spriteIndex];
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
            DataManager.list_item.Add(item);
        }
        //기존 아이템 수정의 경우
        else {
            if (DataManager.list_item[nClikedIndex].Key != item.Key
                && findListItem(item.Key) != -1) return;        //키값 겹치는 경우
            DataManager.list_item[nClikedIndex].Key = item.Key;
            DataManager.list_item[nClikedIndex].Img = item.Img;
            DataManager.list_item[nClikedIndex].Name = item.Name;
            DataManager.list_item[nClikedIndex].Info = item.Info;

        }
        pnlEdit_item.SetActive(false);
        displayItemList();
    }
    //아이템삭제
    public void itemEditDeleteClick() {
        if (nClikedIndex != -1) {
            DataManager.list_item.RemoveAt(nClikedIndex);
            pnlEdit_item.SetActive(false);
            displayItemList();
        }

    }
    //item 수정창 Close 클릭
    public void itemEditCloseClick() {
        pnlEdit_item.SetActive(false);
    }
    //키값으로 인덱스 조사
    public int findListItem(int key) {
        int index = -1;

        for(int i=0; i<DataManager.list_item.Count; i++) {
            if(DataManager.list_item[i].Key == key) {
                index = i;
                break;
            }
        }
        return index;
    }
}
