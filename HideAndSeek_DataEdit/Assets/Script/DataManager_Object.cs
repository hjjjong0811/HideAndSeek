using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager_Object : DataManager {
    [SerializeField]
    public List<Object_Data> list_Object;

    public override void SortByKey() {
        list_Object.Sort(delegate (Object_Data A, Object_Data B) {
            if (A.Key > B.Key) return 1;
            else if (A.Key < B.Key) return -1;
            return 0;
        });
    }
    public override void LoadData(string path) {
        if (list_Object != null) list_Object.Clear();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.OpenOrCreate);

        list_Object = new List<Object_Data>();
        if (file != null && file.Length > 0) {
            list_Object = (List<Object_Data>)bf.Deserialize(file);

        }

        file.Close();
    }
    public override void SaveData(String path) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);

        bf.Serialize(file, list_Object);
        file.Close();
    }
}
