using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour {

    public GameObject Inventory_Prefab;
    public GameObject Menu_Prefab;

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

    public void Btn_Menu()
    {

            GameObject temp = Instantiate(Menu_Prefab);
            temp.name = "Menu";

    }


}
