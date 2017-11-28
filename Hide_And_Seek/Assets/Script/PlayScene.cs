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
        habin_nosalt = 4,       //소금가져와
        habin_havesalt = 5,     //정연가져와
        no_jy = 6,      //정연없어
        no_hb = 7,      //하빈없네
        hb_die = 8,     //주거써
        break_cabinet = 9,  //장식장부심
        after_break = 10,   //장식장 부시고난 후 숨은경우
        jy_die = 11,    //정연주거써?
        
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
            case numScene.habin_nosalt:
                sc.GetComponent<PlaySceneController>().setScene(numScene.habin_nosalt);
                break;
            case numScene.habin_havesalt:
                sc.GetComponent<PlaySceneController>().setScene(numScene.habin_havesalt);
                break;
            case numScene.no_jy:
                sc.GetComponent<PlaySceneController>().setScene(numScene.no_jy);
                break;
            case numScene.no_hb:
                sc.GetComponent<PlaySceneController>().setScene(numScene.no_hb);
                break;
            case numScene.hb_die:
                sc.GetComponent<PlaySceneController>().setScene(numScene.hb_die);
                break;
            case numScene.break_cabinet:
                sc.GetComponent<PlaySceneController>().setScene(numScene.break_cabinet);
                break;
            case numScene.after_break:
                sc.GetComponent<PlaySceneController>().setScene(numScene.after_break);
                break;
            case numScene.jy_die:
                sc.GetComponent<PlaySceneController>().setScene(numScene.jy_die);
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
