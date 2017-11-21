using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneController : MonoBehaviour {
    public GameObject hj_pre, hb_pre, jy_pre, su_pre, ps_pre, mainch_pre, light_direc_pre;   //진행에 필요한 프리팹
    private List<MoveWayPoint> myChar;    //재생중 필요한 캐릭터오브젝트

    public PlayScene.numScene number;   //씬넘버, PlayScene으로부터 받음
    private int stateInScene;           //재생중 필요한 넘버

    delegate void del_typeA();
    del_typeA play;     //씬 넘버에 따른 메서드
    
    public void setScene(PlayScene.numScene n) {
        this.number = n;
        stateInScene = 0;

        if (n == PlayScene.numScene.tutorial) play = playTutorial;
    }
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update() {
        play();
    }


    private void playTutorial() {
        if (stateInScene == 0) {
            mainch_pre = Resources.Load("Prefabs/Char_Player") as GameObject;
            light_direc_pre = Resources.Load("Prefabs/Light_directional") as GameObject;
            SceneManager.LoadScene("S_BBQ");
            stateInScene = -1;
            Invoke("delay", 0.1f);
        }
        //ScriptManager 0~몇번 대사까지 재생
        //등장인물 instantiate
        if (stateInScene == 1) {
            Debug.Log("1");
            stateInScene = 2;
            GameObject hyojung = Instantiate(mainch_pre);
            hyojung.GetComponent<MoveWayPoint>().move(new Vector2[] { new Vector2(1, -1) }, MoveWayPoint.Speed_run);
            myChar = new List<MoveWayPoint>();
            myChar.Add(hyojung.GetComponent<MoveWayPoint>());

            GameObject light = Instantiate(light_direc_pre);
            light.name = "MyLight";
        }
        if(stateInScene == 2) {
            //모든 캐릭터 이동 끝남
            int check = 0;
            for (int i = 0; i < myChar.Count; i++) {
                if (myChar[i].isIdle()) {
                    check++;
                }
            }
            if(check == myChar.Count) {
                stateInScene = 3;
            }
        }
        if(stateInScene == 3) {
            GameObject.Find("MyLight").GetComponent<Light_Directional>().fadeOut(2.0f);

            if (!GameObject.Find("MyLight").GetComponent<Light_Directional>().isFade) {

                SceneManager.LoadScene("1_Hall");
                Destroy(this.gameObject);
                Destroy(this);

            }
        }
    }

    private void delay() {
        stateInScene = 1;
    }
}