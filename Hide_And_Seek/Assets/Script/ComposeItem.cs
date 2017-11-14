using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

[Serializable]
public class ComposeItem
{
    [SerializeField]
    public int Key_Compose;
    public int Count;
    public int[] MaterialItemsKey;
    public int ResultItemKey;
}
