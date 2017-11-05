using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager_Compose : DataManager {
    [SerializeField]
    public List<ComposeItem> list_Compose;

    public override void SortByKey() {
        list_Compose.Sort(delegate (ComposeItem A, ComposeItem B) {
            if (A.Key_Compose > B.Key_Compose) return 1;
            else if (A.Key_Compose < B.Key_Compose) return -1;
            return 0;
        });
    }
    public override void LoadData(string path) {
        if (list_Compose != null) list_Compose.Clear();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.OpenOrCreate);

        list_Compose = new List<ComposeItem>();
        if (file != null && file.Length > 0) {
            list_Compose = (List<ComposeItem>)bf.Deserialize(file);

        }

        file.Close();
    }
    public override void SaveData(String path) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);

        bf.Serialize(file, list_Compose);
        file.Close();
    }
}
