using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine.UI;   //test

public class ItemManager {
    private static ItemManager instance = null;
    
    public List<Item> ListItem;
    public List<ComposeItem> ListCompose;

    private ItemManager() {
        LoadData();
        Debug.Log("Create ItemManager");
    }
    
    public static ItemManager getInstance() {
        if(instance == null) {
            instance = new ItemManager();
        }
        return instance;
    }
	
    void LoadData() {
        if (ListItem != null) ListItem.Clear();
        ListItem = new List<Item>();

        if (ListCompose != null) ListCompose.Clear();
        ListCompose = new List<ComposeItem>();

        TextAsset DataItem = Resources.Load("GameData/item") as TextAsset;
        TextAsset DataCompose = Resources.Load("GameData/compose") as TextAsset;

        BinaryFormatter bf = new BinaryFormatter();

        MemoryStream ms = new MemoryStream(DataItem.bytes);
        if(ms != null && ms.Length > 0) {
            ListItem = (List<Item>)bf.Deserialize(ms);
        }
        ms.Close();

        ms = new MemoryStream(DataCompose.bytes);
        if (ms != null && ms.Length > 0) {
            ListCompose = (List<ComposeItem>)bf.Deserialize(ms);
        }
        ms.Close();
    }

    public Sprite LoadSpriteFromBytes(byte[] data) {
        Texture2D texture2D = new Texture2D(500, 500);
        texture2D.LoadImage(data);
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));

        return sprite;
    }

    public Item getItemInfo(int key) {
        for (int i = 0; i < ListItem.Count; i++) {
            if(ListItem[i].Key == key) {
                return ListItem[i];
            }
        }
        return null;
    }

    public int getComposeItem(int count, List<int> material) {
        int resultKey = -1;

        //test
        String test = "조합 가능? 재료 : ";
        for (int i = 0; i < material.Count; i++) {
            test += material[i];
        }
        Debug.Log(test);

        //재료 갯수 일치 확인
        for (int i = 0; i < ListCompose.Count; i++) {
            if(ListCompose[i].Count == material.Count) {
                int check = 0;
                //재료 종류 일치 확인
                for (int j = 0; j < ListCompose[i].Count; j++) {
                    if (material.Contains(ListCompose[i].MaterialItemsKey[j])) {
                        check++;
                    }
                }
                if(check == material.Count) {
                    resultKey = ListCompose[i].ResultItemKey;
                    break;
                }
            } //if
        } //for

        return resultKey;
    }
}
