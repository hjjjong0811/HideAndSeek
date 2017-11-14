using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenUIManager : MonoBehaviour {
    public GameObject pnlScrollList, itemInfo;
    public GameObject btnItem_prefab, imgCheck_prefab;
    
    private int selectedItem_key;
    private bool isComposeMode;

    private List<int> selectedCompose_key;
    private List<GameObject> btnItemList;

	// Use this for initialization
	void Start () {
        selectedItem_key = -1;
        isComposeMode = false;
        itemInfo.SetActive(false);
        selectedCompose_key = null;
        btnItemList = new List<GameObject>();
        LoadItem();
	}
	
	private void LoadItem() {
        for (int i = 0; i < btnItemList.Count; i++) {
            Destroy(btnItemList[i]);
        }
        btnItemList.Clear();
        //inventory class + itemManager

        //test
        ItemManager mng = ItemManager.getInstance();
        Inventory inven = Inventory.getInstance();

        for (int i = 0; i < inven.inventory.Count; i++) {
            int index = i;
            Item item = mng.getItemInfo(inven.inventory[i]);
            GameObject button = Instantiate(btnItem_prefab, pnlScrollList.transform);
            button.name = item.Key+"";
            button.GetComponent<Image>().sprite = mng.LoadSpriteFromBytes(item.Img_data);
            button.GetComponent<Button>().onClick.AddListener(() => itemClick(index));
            btnItemList.Add(button);
        }
    }

    private void itemClick(int index) {
        ItemManager mng = ItemManager.getInstance();
        Inventory inven = Inventory.getInstance();

        Item item = mng.getItemInfo(inven.inventory[index]);
        selectedItem_key = item.Key;
        itemInfo.transform.GetChild(0).GetComponent<Image>().sprite = mng.LoadSpriteFromBytes(item.Img_data);
        itemInfo.transform.GetChild(1).GetComponent<Text>().text = item.Name;
        itemInfo.transform.GetChild(2).GetComponent<Text>().text = item.Info;
        itemInfo.SetActive(true);

        if (isComposeMode) {
            if (selectedCompose_key.Contains(item.Key)) {
                selectedCompose_key.Remove(item.Key);
                Debug.Log("조합 아이템 재료 제거 : " + item.Key);
                Destroy(btnItemList[index].transform.GetChild(0).gameObject);
            } else {
                selectedCompose_key.Add(item.Key);
                Debug.Log("조합 아이템 추가 : " + item.Key);
                Instantiate(imgCheck_prefab, btnItemList[index].transform);
            }
        }
    }

    public void itemEquipClick() {
        if (selectedItem_key != -1) {
            //Change inventory-> curEquipItem
            Inventory inven = Inventory.getInstance();
            inven.equipItem(selectedItem_key);
            Debug.Log(inven.curEquipItem + " 을 장착");
        }
        
    }

    public void composeClick() {
        if (isComposeMode) {
            ItemManager mng = ItemManager.getInstance();
            int result = mng.getComposeItem(selectedCompose_key.Count, selectedCompose_key);
            if(result == -1) {
                Debug.Log("조합실패");
            } else {
                //조합성공, 인벤토리 수정
                Inventory inven = Inventory.getInstance();
                inven.composeItem(result, selectedCompose_key);

                Item item = mng.getItemInfo(result);
                selectedItem_key = result;
                itemInfo.transform.GetChild(0).GetComponent<Image>().sprite = mng.LoadSpriteFromBytes(item.Img_data);
                itemInfo.transform.GetChild(1).GetComponent<Text>().text = item.Name;
                itemInfo.transform.GetChild(2).GetComponent<Text>().text = item.Info;
                itemInfo.SetActive(true);
            }

            selectedCompose_key.Clear();
            selectedCompose_key = null;
            isComposeMode = false;
            LoadItem();
        } else {
            selectedCompose_key = new List<int>();
            isComposeMode = true;
            Debug.Log("조합 아이템 선택 시작");
        }
    }
    
}
