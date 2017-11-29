using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DataManager_Object datamng = GetComponent<DataManager_Object>();
        datamng.LoadData("test2.bytes");

        Object_Data obd = new Object_Data();
        obd.Key = 1;
        obd.Name = "찻잔";
        obd.DetailList = new List<Object_Data_detail>();

        Object_Data_detail obdd = new Object_Data_detail();
        obdd.StartChapter = 10;
        obdd.SpriteNum = 0;
        obdd.Auto = new Object_Output();
        obdd.Auto.type = (int)Object_Type.sound_effect;
        obdd.Auto.sound_key = 18;
        obd.DetailList.Add(obdd);

        datamng.list_Object.Add(obd);
        datamng.SaveData("test2.bytes");

        datamng.list_Object.Clear();

        datamng.LoadData("test2.bytes");
        Debug.Log("load" + datamng.list_Object.Count);
        for (int i = 0; i < datamng.list_Object.Count; i++) {
            Debug.Log(datamng.list_Object[i].Key + " : " + datamng.list_Object[i].Name);
            Debug.Log(datamng.list_Object[i].DetailList[0].Auto.type + " : " + datamng.list_Object[i].DetailList[0].Auto.sound_key);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
