using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

[Serializable]
public class Script {
    [SerializeField]
    public int Key;
    public String Name;
    public int Obj_Img;
    public String[] Scripts;
    public byte[] Picture;
}
