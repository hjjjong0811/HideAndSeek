using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

[Serializable]
public class Script {
    [SerializeField]
    public int Key;
    public String Name;
    public int Obj_Img;
    public String[] Scripts;
    public byte[] Picture;
}

public class ScriptManager : MonoBehaviour {
    private enum numCharImg{
        no = 0, hyojung=1, habin =2, jungyeon = 3, suun = 4, pension =5 
    }
    private static ScriptManager instance = null;
    public delegate void del();

    private List<Script> script_story, script_obj;
    public Sprite[] sprite_charImg;
    
    public Text txtScript;
    public GameObject pnlScript, imgChar_obj, imgIllust_obj;
    public Image imgChar, imgIllust;

    private bool isClicked = false;
    private List<Script> scripts;

    public bool isPlaying;

    private void Update() {
        if (Input.GetButtonDown("Action")) {
            OnClicked();
        }
    }

    private void Awake() {
        if (instance == null) instance = this;
        else if(instance != this) {
            Destroy(this.gameObject);
            Destroy(this);
        }
        DontDestroyOnLoad(this.gameObject);
        LoadData();
        scripts = new List<Script>();
        this.gameObject.SetActive(false);
        isPlaying = false;
    }

    public static ScriptManager getInstance() {
        if (instance == null) {
            instance = new ScriptManager();
        }
        return instance;
    }

    private IEnumerator playScript(List<Script> scripts, del wake) {
        while (scripts.Count >0) {
            Script curScript = scripts[0];
            if(curScript.Picture != null) {
                imgIllust.sprite = LoadSpriteFromBytes(curScript.Picture);
                imgIllust_obj.SetActive(true);
            } else {
                imgIllust_obj.SetActive(false);
            }
            if (curScript.Obj_Img == (int)numCharImg.no) imgChar_obj.SetActive(false);
            else {
                imgChar.sprite = sprite_charImg[curScript.Obj_Img];
                imgChar_obj.SetActive(true);
            }

            for (int j = 0; j < curScript.Scripts.Length; j++) {
                isClicked = false;
                if (curScript.Name != "") txtScript.text = curScript.Name + "\n";
                else txtScript.text = "";
                txtScript.text += curScript.Scripts[j];
                yield return new WaitUntil(()=>isClicked);
            }
            scripts.Remove(curScript);
        }
        this.gameObject.SetActive(false);
        GameObject go = GameObject.Find("Canvas_UI");
        if (go != null) go.GetComponent<CanvasGroup>().interactable = true;
        if (wake != null) wake();
        isPlaying = false;
        yield break;
    }

    public void OnClicked() {
        isClicked = true;
    }

    /// <summary>
    /// Show Script array
    /// </summary>
    /// <param name="isObj"></param>
    /// <param name="scripts_key"></param>
    public void showScript(bool isObj, int[] scripts_key) {
        isPlaying = true;
        for (int i = 0; i < scripts_key.Length; i++) {
            scripts.Add(findScript(isObj, scripts_key[i]));
        }
        this.gameObject.SetActive(true);
        GameObject gameUI = GameObject.Find("Canvas_UI");
        if (gameUI != null) gameUI.GetComponent<CanvasGroup>().interactable = false;
        GameObject inven = GameObject.Find("Inven");
        if (inven != null) gameUI.GetComponent<GameUIManager>().Btn_Inven();
        StartCoroutine(playScript(scripts, null));
    }

    /// <summary>
    /// Show Script array
    /// </summary>
    /// <param name="isObj"></param>
    /// <param name="scripts_key"></param>
    /// <param name="wake">if want call Method at end of scripts</param>
    public void showScript(bool isObj, int[] scripts_key, del wake) {
        isPlaying = true;
        for (int i = 0; i < scripts_key.Length; i++) {
            scripts.Add(findScript(isObj, scripts_key[i]));
        }
        this.gameObject.SetActive(true);
        GameObject gameUI = GameObject.Find("Canvas_UI");
        if (gameUI != null) gameUI.GetComponent<CanvasGroup>().interactable = false;
        GameObject inven = GameObject.Find("Inven");
        if (inven != null) gameUI.GetComponent<GameUIManager>().Btn_Inven();
        StartCoroutine(playScript(scripts, wake));
    }

    private Script findScript(bool isObj,int key) {
        if (isObj) {
            for (int i = 0; i < script_obj.Count; i++) {
                if(script_obj[i].Key == key) {
                    return script_obj[i];
                }
            }
        } else {
            for (int i = 0; i < script_story.Count; i++) {
                if (script_story[i].Key == key) {
                    return script_story[i];
                }
            }
        }
        
        return null;
    }

    void LoadData() {
        if (script_obj != null) script_obj.Clear();
        if (script_story != null) script_story.Clear();
        script_obj = new List<Script>();
        script_story = new List<Script>();

        TextAsset DataStory_sc = Resources.Load("GameData/script_story") as TextAsset;
        //TextAsset DataObj_sc = Resources.Load("GameData/script_obj") as TextAsset;

        BinaryFormatter bf = new BinaryFormatter();

        MemoryStream ms = new MemoryStream(DataStory_sc.bytes);
        if (ms != null && ms.Length > 0) {
            script_story = (List<Script>)bf.Deserialize(ms);
        }
        ms.Close();

        //ms = new MemoryStream(DataObj_sc.bytes);
        //if (ms != null && ms.Length > 0) {
        //    script_obj = (List<Script>)bf.Deserialize(ms);
        //}
        //ms.Close();
    }

    private Sprite LoadSpriteFromBytes(byte[] data) {
        Texture2D texture2D = new Texture2D(500, 500);
        texture2D.LoadImage(data);
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));

        return sprite;
    }


}
