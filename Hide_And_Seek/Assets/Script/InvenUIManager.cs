using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenUIManager : MonoBehaviour {
    public GameObject pnlScrollList, itemInfo;
    public GameObject btnItem_prefab, imgCheck_prefab;

    private ItemManager mng;
    private int selectedItem_key;
    private bool isComposeMode;

    private List<int> selectedCompose;
    private List<GameObject> btnItemList;

	// Use this for initialization
	void Start () {
        selectedItem_key = -1;
        isComposeMode = false;
        itemInfo.SetActive(false);
        selectedCompose = null;
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
        mng = GetComponent<ItemManager>();
        for (int i = 0; i < mng.ListItem.Count; i++) {
            int index = i;
            GameObject button = Instantiate(btnItem_prefab, pnlScrollList.transform);
            button.name = mng.ListItem[i].Key+"";
            button.GetComponent<Image>().sprite = mng.LoadSpriteFromBytes(mng.ListItem[i].Img_data);
            button.GetComponent<Button>().onClick.AddListener(() => itemClick(index));
            btnItemList.Add(button);
        }
    }

    private void itemClick(int index) {
        selectedItem_key = mng.ListItem[index].Key;
        itemInfo.transform.GetChild(0).GetComponent<Image>().sprite = mng.LoadSpriteFromBytes(mng.ListItem[index].Img_data);
        itemInfo.transform.GetChild(1).GetComponent<Text>().text = mng.ListItem[index].Name;
        itemInfo.transform.GetChild(2).GetComponent<Text>().text = mng.ListItem[index].Info;
        itemInfo.SetActive(true);

        if (isComposeMode) {
            selectedCompose.Add(mng.ListItem[index].Key);
            Debug.Log("조합 아이템 추가 : " + mng.ListItem[index].Key);
            Instantiate(imgCheck_prefab, btnItemList[index].transform);
        }
    }

    public void itemEquipClick() {
        if (selectedItem_key != -1) {
            //Change inventory-> curEquipItem

        }
        
    }

    public void composeClick() {
        if (isComposeMode) {
            int result = mng.getComposeItem(selectedCompose.Count, selectedCompose);
            if(result == -1) {
                Debug.Log("조합실패");
            } else {
                //조합성공, 인벤토리 수정
            }

            selectedCompose.Clear();
            selectedCompose = null;
            isComposeMode = false;
            LoadItem();
        } else {
            selectedCompose = new List<int>();
            isComposeMode = true;
            Debug.Log("조합 아이템 선택 시작");
        }
    }
    
}
