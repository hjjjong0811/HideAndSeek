using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditUIManager_Object : EditUIManager {
    public DataManager_Script dataManager;
    public GameObject pnlEdit, scrollList;

    public GameObject scriptScrollList, pre_txtScript;

    public List<InputField> inputlistScript;
    public InputField txtKey, txtName, txtPicturePath, txtLength;
    public Image imgObj, imgPicture;
    public Dropdown dropdownImg;

    //스크립트용
    public GameObject btnScript;
    public bool isNewItem = true;
    public int nClikedIndex;

    public override void Init() {
        displayItemList();
    }

    //Scroll View의 아이템들 보여줌
    public override void displayItemList() {
        //삭제
        for (int i = scrollList.transform.childCount - 1; i >= 0; i--) {
            Destroy(scrollList.transform.GetChild(i).gameObject);
        }
        //출력
        for (int i = 0; i < dataManager.list_script.Count; i++) {
            Script script = dataManager.list_script[i];
            GameObject btn = Instantiate(btnScript, scrollList.transform);

            btn.name = script.Key + "";
            btn.transform.GetChild(0).GetComponent<Text>().text = script.Key + "";
            btn.transform.GetChild(1).GetComponent<Text>().text = script.Name;
            btn.transform.GetChild(2).GetComponent<Text>().text = script.Scripts.Length + "";
            btn.transform.GetChild(3).GetComponent<Text>().text = script.Scripts[0];

            int index = i;
            btn.GetComponent<Button>().onClick.AddListener(() => itemClick(index));
        }
    }

    //수정창 열기
    public override void editPanelSetting() {
        pnlEdit.SetActive(true);
        inputlistScript = new List<InputField>();

        txtKey.text = "";
        txtLength.text = "";
        txtName.text = "";
        txtPicturePath.text = "";
        imgObj.sprite = null;
        imgPicture.sprite = null;
        dropdownImg.value = 0;
        txtLength.text = "1";
        for (int i = scriptScrollList.transform.childCount - 1; i >= 0; i--) {
            Destroy(scriptScrollList.transform.GetChild(i).gameObject);
        }
        GameObject go = Instantiate(pre_txtScript, scriptScrollList.transform);
        inputlistScript.Add(go.GetComponent<InputField>());
        inputlistScript[0].text = "";
    }

    public override void editPanelSetting(object arg) {
        pnlEdit.SetActive(true);

        Script item = (Script)arg;

        txtKey.text = item.Key + "";
        txtName.text = item.Name;
        txtPicturePath.text = "";
        txtLength.text = item.Scripts.Length + "";
        dropdownImg.value = item.Obj_Img;

        imgObj.sprite = dataManager.getObjImg(item.Obj_Img);
        imgPicture.sprite = dataManager.LoadSpriteFromBytes(item.Picture);

        if (inputlistScript != null) {
            inputlistScript.Clear();
        }
        inputlistScript = new List<InputField>();
        for (int i = scriptScrollList.transform.childCount - 1; i >= 0; i--) {
            Destroy(scriptScrollList.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < item.Scripts.Length; i++) {
            GameObject go = Instantiate(pre_txtScript, scriptScrollList.transform);
            inputlistScript.Add(go.GetComponent<InputField>());
            inputlistScript[i].text = item.Scripts[i];
        }
    }

    public override void CreateClick() {
        editPanelSetting();
        isNewItem = true;
        nClikedIndex = -1;
    }


    //item 클릭
    public override void itemClick(int clickedIndex) {
        editPanelCloseClick();
        isNewItem = false;
        Script script = dataManager.list_script[clickedIndex];
        editPanelSetting(script);
        nClikedIndex = clickedIndex;
    }

    //수정창 OK 클릭
    public override void editPanelOKClick() {
        //UI로부터 정보얻음
        Script script = new Script();

        script.Key = int.Parse(txtKey.text);
        script.Name = txtName.text;
        script.Obj_Img = dropdownImg.value;
        if (!txtPicturePath.text.Equals("")) script.Picture = dataManager.LoadBytefromImgPath(txtPicturePath.text);

        script.Scripts = new string[scriptScrollList.transform.childCount];
        for (int i = 0; i < script.Scripts.Length; i++) {
            script.Scripts[i] = scriptScrollList.transform.GetChild(i).GetComponent<InputField>().text;
        }

        //Create의 경우
        if (isNewItem) {
            if (isExistKey(script.Key)) return;       //키값 겹치는 경우
            dataManager.list_script.Add(script);
        }
        //기존 아이템 수정의 경우
        else {
            if (dataManager.list_script[nClikedIndex].Key != script.Key
                && isExistKey(script.Key)) return;        //키값 겹치는 경우

            dataManager.list_script[nClikedIndex].Key = script.Key;
            dataManager.list_script[nClikedIndex].Name = script.Name;
            dataManager.list_script[nClikedIndex].Obj_Img = script.Obj_Img;
            dataManager.list_script[nClikedIndex].Scripts = script.Scripts;
            if (!txtPicturePath.text.Equals("")) dataManager.list_script[nClikedIndex].Picture = script.Picture;

        }
        editPanelCloseClick();
        displayItemList();
    }
    //아이템삭제
    public override void editPanelDeleteClick() {
        if (!isNewItem) {
            dataManager.list_script.RemoveAt(nClikedIndex);
            editPanelCloseClick();
            displayItemList();
        }

    }
    //item 수정창 Close 클릭
    public override void editPanelCloseClick() {
        pnlEdit.SetActive(false);
    }


    //키값 존재 조사
    public bool isExistKey(int key) {
        for (int i = 0; i < dataManager.list_script.Count; i++) {
            if (dataManager.list_script[i].Key == key) {
                return true;
            }
        }
        return false;
    }

    public void OnDropDownChange() {
        imgObj.sprite = dataManager.getObjImg(dropdownImg.value);
    }
    public void OnPicturepathChange() {
        try {
            byte[] data = dataManager.LoadBytefromImgPath(txtPicturePath.text);
            imgPicture.sprite = dataManager.LoadSpriteFromBytes(data);

        } catch {

        }
    }

    public void OnLengthChange() {
        if (int.Parse(txtLength.text) < 1) {
            txtLength.text = "1";
        }
        String[] temp = new string[scriptScrollList.transform.childCount];
        for (int i = 0; i < temp.Length; i++) {
            temp[i] = scriptScrollList.transform.GetChild(i).GetComponent<InputField>().text;
        }
        if (inputlistScript != null) {
            inputlistScript.Clear();
        }
        int len = int.Parse(txtLength.text);
        inputlistScript = new List<InputField>();
        for (int i = scriptScrollList.transform.childCount - 1; i >= 0; i--) {
            Destroy(scriptScrollList.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < len; i++) {
            GameObject go = Instantiate(pre_txtScript, scriptScrollList.transform);
            inputlistScript.Add(go.GetComponent<InputField>());
            if (i < temp.Length) inputlistScript[i].text = temp[i];
        }
    }
}
