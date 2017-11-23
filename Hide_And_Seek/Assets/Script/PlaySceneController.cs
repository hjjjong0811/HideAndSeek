using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneController : MonoBehaviour {
    //진행에 필요한 프리팹
    public GameObject hj_pre, hb_pre, jy_pre, su_pre, ps_pre, mainch_pre, light_direc_pre;
    private List<MoveWayPoint> myChar;    //재생중 필요한 캐릭터오브젝트
    
    delegate IEnumerator del_type_test();
    del_type_test play;
    
    
    public void setScene(PlayScene.numScene n) {
        if (n == PlayScene.numScene.tutorial) play = playTutorial;
    }
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start() {
        StartCoroutine(play());
    }

    private IEnumerator playTutorial() {
        //씬 로드
        SceneManager.LoadScene("S_BBQ");
        yield return new WaitForSeconds(0.1f);

        //대사 진행
        //ScriptManager 펜션도착 대사 진행 요청

        //while(ScriptManager 대사 요청 완료 확인)

        //바베큐장 씬 로드
        GameObject test = Instantiate(mainch_pre);
        yield return new WaitForSeconds(0.01f); //instantiate후 지연

        test.GetComponent<MoveWayPoint>().move(new Vector2[] { new Vector2(1, -1) }, MoveWayPoint.Speed_run);
        myChar = new List<MoveWayPoint>();
        myChar.Add(test.GetComponent<MoveWayPoint>());

        GameObject light = Instantiate(light_direc_pre);
        light.name = "MyLight";
        yield return new WaitForSeconds(0.01f); //instantiate후 지연
        light.GetComponent<Light_Directional>().setLight(false);
        Debug.Log("test3");
        //light.GetComponent<Light_Directional>().fadeIn(2.0f);
        yield break;
    }
    
            //test.GetComponent<MoveWayPoint>().move(new Vector2[] { new Vector2(1, -1) }, MoveWayPoint.Speed_run);
            //myChar = new List<MoveWayPoint>();
            //myChar.Add(test.GetComponent<MoveWayPoint>());
            
            ////모든 캐릭터 이동 끝남
            //int check = 0;
            //for (int i = 0; i < myChar.Count; i++) {
            //    if (myChar[i].isIdle()) {
            //        check++;
            //    }
            //}
            //if(check == myChar.Count) {
            //    stateInScene = 3;
            //}

}