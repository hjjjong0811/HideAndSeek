using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTest : MonoBehaviour {

    public void Start()
    {
        GameObject prefab = Resources.Load("Prefabs/Canvas_UI") as GameObject;
        GameObject GameUI = MonoBehaviour.Instantiate(prefab) as GameObject;
        GameUI.name = "GameUI";
    }
}
