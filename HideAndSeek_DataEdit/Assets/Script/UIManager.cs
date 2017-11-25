using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private readonly int code_item = 0, code_compose = 1, code_script = 2;

    DataManager dataManager = null;
    EditUIManager editUIManager = null;

    InputField inputPath;   //파일경로입력
    Dropdown dropdown;      //종류선택

    public GameObject scrollList;
    

    private void Start() {
        inputPath = GameObject.Find("InputPath").GetComponent<InputField>();
        dropdown = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        
    }

    public void OpenClick() {
        for (int i = scrollList.transform.childCount - 1; i >= 0; i--) {
            Destroy(scrollList.transform.GetChild(i).gameObject);
        }
        if (dropdown.value == code_item) {
            dataManager = GameObject.Find("Manager_Item").GetComponent<DataManager_Item>();
            dataManager.LoadData(inputPath.text);
            editUIManager = GameObject.Find("Manager_Item").GetComponent<EditUIManager_Item>();
            editUIManager.Init();
        } else if (dropdown.value == code_compose) {
            dataManager = GameObject.Find("Manager_Compose").GetComponent<DataManager_Compose>();
            dataManager.LoadData(inputPath.text);
            editUIManager = GameObject.Find("Manager_Compose").GetComponent<EditUIManager_Compose>();
            editUIManager.Init();
        } else if (dropdown.value == code_script) {
            dataManager = GameObject.Find("Manager_Script").GetComponent<DataManager_Script>();
            dataManager.LoadData(inputPath.text);
            editUIManager = GameObject.Find("Manager_Script").GetComponent<EditUIManager_Script>();
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
