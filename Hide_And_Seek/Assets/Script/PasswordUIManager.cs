﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordUIManager {
    public delegate void wake();
    
    private GameObject window;
    private Text txtPassword;
    private Button []btnNum;

    private GameObject receiver;
    private int password;

    public PasswordUIManager(GameObject pReceiver, int pPassword) {
        receiver = pReceiver;
        password = pPassword;

        GameObject pre = Resources.Load("Prefabs/Canvas_Password") as GameObject;
        pre.transform.GetChild(0).gameObject.SetActive(false);
        window = MonoBehaviour.Instantiate(pre);
        PasswordUI_wait wait = window.AddComponent<PasswordUI_wait>();
        wait.StartCoroutine(wait.waitScript(setUI));
    }

    public void setUI() {
        ScriptManager.getInstance().isPlaying = true;
        window.transform.GetChild(0).gameObject.SetActive(true);
        txtPassword = window.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        txtPassword.text = "";

        btnNum = new Button[10];
        Transform pnlBtn = window.transform.GetChild(0).GetChild(1);
        for (int i = 1; i <= 9; i++) {
            int n = i;
            btnNum[n] = pnlBtn.GetChild(n - 1).GetComponent<Button>();
            btnNum[n].onClick.AddListener(() => buttonClick(n));
        }
        btnNum[0] = pnlBtn.GetChild(10).GetComponent<Button>();
        btnNum[0].onClick.AddListener(() => buttonClick(0));
    }

    public void buttonClick(int num) {
        txtPassword.text += num;
        if(txtPassword.text.Length > 3) {
            ScriptManager.getInstance().isPlaying = false;
            int input = int.Parse(txtPassword.text);

            if(password == input) {
                receiver.SendMessage("inputPassword", true);
            } else {
                receiver.SendMessage("inputPassword", false);
            }

            MonoBehaviour.Destroy(window);
        }
    } //buttonClick

    public class PasswordUI_wait : MonoBehaviour {
        public IEnumerator waitScript(wake w) {
            yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);
            w();
            yield break;
        }
    }
}
