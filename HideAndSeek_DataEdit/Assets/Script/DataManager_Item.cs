using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager_Item : DataManager {
    [SerializeField]
    public List<Item> list_item;
    
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
    public byte[] LoadBytefromImgPath(String path) {

        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        FileInfo fi = new FileInfo(path);
        long imagelength = fi.Length;

        BinaryReader br = new BinaryReader(fs);
        byte[] imageData = br.ReadBytes((int)imagelength);

        return imageData;
    }
    public Sprite LoadSpriteFromBytes(byte[] data) {
        Texture2D texture2D = new Texture2D(500, 500);
        texture2D.LoadImage(data);
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));

        return sprite;
    }
}
