using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour {

    public GameObject Inventory_Prefab;
    public GameObject Menu_Prefab;

    public bool isOpenInven = false;
    public bool isOpenMenu = false;

    public void Btn_Inven()
    {
        if (isOpenInven)
        {
            GameObject.Destroy(GameObject.Find("TTTTEST"));
            isOpenInven = false;
        }
        else
        {
            GameObject test = Instantiate(Inventory_Prefab);
            test.name = "TTTTEST";
            isOpenInven = true;
        }
    }

    public void Btn_Menu()
    {

        if (isOpenMenu)
        {
            GameObject.Destroy(GameObject.Find("Menu"));
            isOpenMenu = false;
        }
        else
        {
            GameObject test1 = Instantiate(Menu_Prefab);
            test1.name = "Menu";
            isOpenMenu = true;
        }

    }


}
