using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager_Item : DataManager {
    public Sprite[] itemSprites;

    [SerializeField]
    public List<Item> list_item;

    public void Start() {
        itemSprites = Resources.LoadAll<Sprite>("Sprites/Items/item_0");
    }
    public override void SortByKey() {
        list_item.Sort(delegate (Item A, Item B) {
            if (A.Key > B.Key) return 1;
            else if (A.Key < B.Key) return -1;
            return 0;
        });
    }
    public override void LoadData(string path) {
        if (list_item != null) list_item.Clear();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.OpenOrCreate);
        
        list_item = new List<Item>();
        if (file != null && file.Length > 0) {
            list_item = (List<Item>)bf.Deserialize(file);

        }

        file.Close();
    }
    public override void SaveData(String path) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        
        bf.Serialize(file, list_item);
        file.Close();
    }
    public int findItemSpriteByName(String pname) {
        int index = -1;
        for (int i = 0; i < itemSprites.Length; i++) {
            if (itemSprites[i].name == pname) {
                index = i;
            }
        }

        return index;
    }
}
