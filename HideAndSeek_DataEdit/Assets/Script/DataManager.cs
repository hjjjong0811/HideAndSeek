using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public abstract class DataManager : MonoBehaviour
{
    public abstract void LoadData(String path);
    public abstract void SaveData(String path);
    
}