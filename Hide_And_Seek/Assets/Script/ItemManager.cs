using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine.UI;   //test

public class ItemManager : MonoBehaviour {
    public TextAsset DataItem, DataCompose;
    public List<Item> ListItem;

	// Use this for initialization
	void Start () {
        LoadData();
        for (int i = 0; i < ListItem.Count; i++) {
            //GameObject test = Instantiate(ItemPrefab, PnlList.transform);
            //test.GetComponent<Image>().sprite = LoadSpriteFromBytes(ListItem[i].Img_data);
            Debug.Log(ListItem[i].Key + ", " + ListItem[i].Name + " : " + ListItem[i].Info);
        }
	}
	
    void LoadData() {
        if (ListItem != null) ListItem.Clear();
        BinaryFormatter bf = new BinaryFormatter();

        MemoryStream ms = new MemoryStream(DataItem.bytes);

        ListItem = new List<Item>();
        if(ms != null && ms.Length > 0) {
            ListItem = (List<Item>)bf.Deserialize(ms);
        }
        ms.Close();
    }

    public Sprite LoadSpriteFromBytes(byte[] data) {
        Texture2D texture2D = new Texture2D(500, 500);
        texture2D.LoadImage(data);
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));

        return sprite;
    }

    Item getItemInfo(int key) {
        for (int i = 0; i < ListItem.Count; i++) {
            if(ListItem[i].Key == key) {
                return ListItem[i];
            }
        }
        return null;
    }

    int getComposeItem(int count, int[] material) {
        int resultKey = -1;

        return resultKey;
    }
}
