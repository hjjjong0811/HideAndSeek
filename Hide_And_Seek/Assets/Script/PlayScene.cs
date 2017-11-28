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

    /// <summary>
    /// 정원추가(엔딩)
    /// </summary>
    public enum numScene {

        Invalid_Obj = -1, // 잘못된 오브젝트사용
        JeongYeon = -2, // 정연엔딩
        ending_exit = -3, // 탈출엔딩
        suspectDoll = -4, // 인형인줄
        suspectKim = -5, // 아저씨인거 알게됨



        tutorial = 0,       //도착
        hide_1_end = 1,     //1차끝 혼숨제안
        hide_2_ready = 2,   //2차 준비완료(소주)
        ringPhone = 3,      //문자오고 전화거는씬
       // ending_exit = 4    //탈출엔딩(?)
    }

 


    /// <summary>
    /// 씬 재생해주는 메서드
    /// </summary>
    /// <param name="sceneNumber">enum으로 씬번호 전달</param>
    public void playScene(numScene sceneNumber) {
        GameObject sc_pre = Resources.Load("Prefabs/SceneController") as GameObject;
        GameObject sc = MonoBehaviour.Instantiate(sc_pre);
        switch (sceneNumber) {
            case numScene.tutorial:
                sc.GetComponent<PlaySceneController>().setScene(numScene.tutorial);
                break;
            case numScene.hide_1_end:
                sc.GetComponent<PlaySceneController>().setScene(numScene.hide_1_end);
                break;
            case numScene.hide_2_ready:
                sc.GetComponent<PlaySceneController>().setScene(numScene.hide_2_ready);
                break;
            case numScene.ringPhone:
                sc.GetComponent<PlaySceneController>().setScene(numScene.ringPhone);
                break;

                //정원엔딩추가
            case numScene.Invalid_Obj:
                sc.GetComponent<PlaySceneController>().setScene(numScene.Invalid_Obj);
                break;
            case numScene.JeongYeon:
                sc.GetComponent<PlaySceneController>().setScene(numScene.JeongYeon);
                break;
            case numScene.ending_exit:
                 sc.GetComponent<PlaySceneController>().setScene(numScene.ending_exit);
                break;
            case numScene.suspectDoll:
                sc.GetComponent<PlaySceneController>().setScene(numScene.suspectDoll);
                break;
            case numScene.suspectKim:
                sc.GetComponent<PlaySceneController>().setScene(numScene.suspectKim);
                break;

        }

    }


    
}
