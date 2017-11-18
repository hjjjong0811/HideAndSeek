using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {

    public GameObject Inventory_Prefab;
    public GameObject Setting_Prefab;

    public bool isOpenInven = false;

    public void Btn_Inven()
    {
        if (isOpenInven)
        {
            GameObject.Destroy(GameObject.Find("Inven"));
            isOpenInven = false;
        }
        else
        {
            GameObject test = Instantiate(Inventory_Prefab);
            test.name = "Inven";
            isOpenInven = true;
        }
    }

    public void Btn_Setting()
    {
        GameObject temp = Instantiate(Setting_Prefab);
        temp.name = "Setting";
           
    }


}
