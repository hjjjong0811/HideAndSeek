using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneController : MonoBehaviour {
    public GameObject hj_pre, hb_pre, jy_pre, su_pre, ps_pre, mainch_pre, light_direc_pre;   //진행에 필요한 프리팹
    private List<MoveWayPoint> myChar;    //재생중 필요한 캐릭터오브젝트

    public PlayScene.numScene number;   //씬넘버, PlayScene으로부터 받음
    private float stateInScene;           //재생중 필요한 넘버

    delegate void del_typeA();
    del_typeA play;     //씬 넘버에 따른 메서드

    float timeSpan, timeLeft;
    
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

    //튜토리얼 씬
    private void playTutorial() {
        if (stateInScene == 0) {
            //배경 씬 로딩
            SceneManager.LoadScene("S_BBQ");

            //신로드 후 지연 필요
            timeSpan = 0.0f;
            timeLeft = 0.1f;
            stateInScene = 0.5f;
        }
        //지연 다됨
        if(stateInScene == 0.5f) {
            timeSpan += Time.deltaTime;
            if (timeSpan > timeLeft) {
                stateInScene = 1;
            }
        }
        //ScriptManager 0~몇번 대사까지 재생 << 스크립트매니저 생기면 추가

        //신 로드 후 등장인물 instantiate
        if (stateInScene == 1) {
            GameObject test = Instantiate(mainch_pre);
            test.GetComponent<MoveWayPoint>().move(new Vector2[] { new Vector2(1, -1) }, MoveWayPoint.Speed_run);
            myChar = new List<MoveWayPoint>();
            myChar.Add(test.GetComponent<MoveWayPoint>());

            GameObject light = Instantiate(light_direc_pre);
            light.name = "MyLight";
            stateInScene = 2;
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
        //화면 페이드아웃 지연
        if(stateInScene == 3) {
            GameObject.Find("MyLight").GetComponent<Light_Directional>().fadeOut(2.0f);

            timeSpan = 0.0f;
            timeLeft = 2.0f;
            stateInScene = 4;
        }
        //페이드아웃 지연 완료시 씬 이동 및 Destroy
        if(stateInScene == 4) {
            timeSpan += Time.deltaTime;
            if(timeSpan > timeLeft) {
                SceneManager.LoadScene("1_Hall");
                Destroy(this.gameObject);
                Destroy(this);
            }
        }
    } //tutorial

}