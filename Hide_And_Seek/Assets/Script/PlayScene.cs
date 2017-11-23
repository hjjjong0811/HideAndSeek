using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScene {
    private static PlayScene instance;

    private PlayScene() {}

    public static PlayScene getInstance() {
        if(instance == null) {
            instance = new PlayScene();
        }
        return instance;
    }

    public enum numScene {
        tutorial = 0,       //도착
        hide_1_end = 1,     //1차끝 혼숨제안
        hide_2_ready = 2,   //2차 준비완료(소주)
        ringPhone = 3,      //문자오고 전화거는씬
        ending_exit = 4    //탈출엔딩(?)
    }

    /// <summary>
    /// 씬 재생해주는 메서드
    /// </summary>
    /// <param name="sceneNumber">enum으로 씬번호 전달</param>
    public void playScene(numScene sceneNumber) {
        switch (sceneNumber) {
            case numScene.tutorial:
                GameObject sc_pre = Resources.Load("Prefabs/SceneController") as GameObject;
                GameObject sc = MonoBehaviour.Instantiate(sc_pre);
                sc.GetComponent<PlaySceneController>().setScene(numScene.tutorial);
                
                break;
            case numScene.hide_1_end:
                break;
            case numScene.hide_2_ready:
                break;
            case numScene.ringPhone:
                break;
            case numScene.ending_exit:
                break;
        }
    }


    
}
