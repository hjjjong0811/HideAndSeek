using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour
{
    public readonly int code_item = 0;
    public static int curCode = -1;
    public static Sprite[] itemSprites;

    [SerializeField]
    public static List<Item> list_item;

    private void Start() {
        itemSprites = Resources.LoadAll<Sprite>("Sprites/Items/item_0");
    }
    public static int findItemSpriteByName(String pname) {
        int index = -1;
        for (int i = 0; i < itemSprites.Length; i++) {
            if(itemSprites[i].name == pname) {
                index = i;
            }
        }

        return index;
    }
    public void LoadData(String path) {
        if (list_item != null) list_item.Clear();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.OpenOrCreate);

        //현재 코드가 아이템
        if(curCode == code_item) {
            list_item = new List<Item>();
            if(file!=null && file.Length > 0) {
                list_item = (List<Item>)bf.Deserialize(file);

            }
        }
        file.Close();
    }
    public void SaveData(String path) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);

        if (curCode == code_item) {
            bf.Serialize(file, list_item);
        }
        file.Close();
    }
}