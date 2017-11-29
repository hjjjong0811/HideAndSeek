﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

[Serializable]
public class Object_Data {
    [SerializeField]
    public int Key;
    public String Name;
    public List<Object_Data_detail> DetailList;
}

[Serializable]
public enum Object_Type {
    script = 0, sound_effect = 1, sound_default = 2, item = 3, 
}

[Serializable]
public class Object_Data_detail {
    [SerializeField]
    public int StartChapter;
    public int SpriteNum;
    public Object_Output Auto;
    public Object_Output Action;
    public Object_Output[] UseItem;
}

[Serializable]
public class Object_Output {
    public int type;
    public int sound_key;
    public int script_key;
    public int item_use_key;
    public int item_result_key;
}