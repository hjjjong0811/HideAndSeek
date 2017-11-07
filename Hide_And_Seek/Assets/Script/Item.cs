using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

[Serializable]
public class Item
{
    [SerializeField]
    public int Key;
    public String Name;
    public String Info;
    public byte[] Img_data;
}
