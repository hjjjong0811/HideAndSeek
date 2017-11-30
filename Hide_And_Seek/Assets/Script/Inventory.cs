using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {
    private static Inventory instance = null;

    public List<int> inventory; //가진 아이템 키 저장
    public int curEquipItem;    //현재 착용중 아이템(사용할)

    private Inventory() {
        inventory = new List<int>();
        curEquipItem = -1;
        
    }

    public static Inventory getInstance() {
        if(instance == null) {
            instance = new Inventory();
        }
        return instance;
    }

    public bool addItem(int itemkey) {
        //이미 존재하는 아이템
        if (inventory.Contains(itemkey)) {
            return false;
        }
        inventory.Add(itemkey);
        GameManager.getInstance().GetItem(itemkey);
        return true;
    }

    public bool deleteItem(int itemkey) {
        //존재하지 않는 아이템 삭제시
        if (!inventory.Contains(itemkey)) {
            return false;
        }
        inventory.Remove(itemkey);
        return true;
    }

    public bool equipItem(int itemkey) {
        //존재하지 않는 아이템 장착
        if (!inventory.Contains(itemkey)) {
            return false;
        }
        curEquipItem = itemkey;
        Item item = ItemManager.getInstance().getItemInfo(itemkey);
        GameObject.Find("Canvas_UI").GetComponent<GameUIManager>().DisplayEquipItem(
            ItemManager.getInstance().LoadSpriteFromBytes(item.Img_data));
        return true;
    }

    public bool equipClear() {
        curEquipItem = -1;
        return true;
    }

    public bool composeItem(int result_key, List<int> material_key) {
        for (int i = 0; i < material_key.Count; i++) {
            deleteItem(material_key[i]);
        }
        addItem(result_key);
        return true;
    }

}
