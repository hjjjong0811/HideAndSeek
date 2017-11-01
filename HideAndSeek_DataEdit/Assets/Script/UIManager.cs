﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private readonly int code_item = 0;

    DataManager dataManager = null;
    EditUIManager editUIManager = null;

    InputField inputPath;   //파일경로입력
    Dropdown dropdown;      //종류선택
    

    private void Start() {
        inputPath = GameObject.Find("InputPath").GetComponent<InputField>();
        dropdown = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        
    }

    public void OpenClick() {
        if (dropdown.value == code_item) {
            dataManager = GameObject.Find("Manager_Item").GetComponent<DataManager_Item>();
            dataManager.LoadData(inputPath.text);
            editUIManager = GameObject.Find("Manager_Item").GetComponent<EditUIManager_Item>();
            editUIManager.Init();
        }
    }
    public void saveClick() {
        if (dataManager == null) return;
        dataManager.SaveData(inputPath.text);
    }

    public void createClick() {
        if (editUIManager == null) return;
        editUIManager.CreateClick();
    }

    public void SortClick() {
        if (dataManager == null || editUIManager == null) return;
        dataManager.SortByKey();
        editUIManager.displayItemList();
    }

}
