using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneController : MonoBehaviour {
    public GameObject pre_player;
    //스토리 진행에 필요한 프리팹
    public GameObject pre_ch_hj, pre_ch_hb, pre_ch_jy, pre_ch_su, pre_ch_ps, pre_ch_mainch;
    public GameObject pre_light_direc;
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
        yield return new WaitForSeconds(0.01f);

        //대사 진행
        //ScriptManager 펜션도착 대사 진행 요청

        //while(ScriptManager 대사 요청 완료 확인)

        //바베큐장 씬 로드
        GameObject ch_main = Instantiate(pre_ch_mainch);
        GameObject ch_hyojung = Instantiate(pre_ch_hj);
        GameObject ch_habin = Instantiate(pre_ch_hb);
        GameObject ch_jungyeon = Instantiate(pre_ch_jy);
        GameObject ch_suun = Instantiate(pre_ch_su);
        GameObject ch_pension = Instantiate(pre_ch_ps);
        GameObject light_obj = Instantiate(pre_light_direc);
        yield return new WaitForSeconds(0.01f);                 //instantiate후 지연

        Light_Directional light = light_obj.GetComponent<Light_Directional>();
        light.fadeIn(2.0f);   //페이드인
        yield return new WaitForSeconds(1f);    //대사 전 딜레이

        //ScriptManager 바베큐장 대사 진행 요청

        //while(ScriptManager 대사 요청 완료 확인)

        //펜션 내부로 이동
        myChar = new List<MoveWayPoint>();
        myChar.Add(ch_main.GetComponent<MoveWayPoint>());
        myChar.Add(ch_habin.GetComponent<MoveWayPoint>());
        myChar.Add(ch_hyojung.GetComponent<MoveWayPoint>());
        myChar.Add(ch_jungyeon.GetComponent<MoveWayPoint>());
        myChar.Add(ch_suun.GetComponent<MoveWayPoint>());

        for (int i = 0; i < myChar.Count; i++) {
            myChar[i].move(new Vector2[] { new Vector2(1, -1) }, MoveWayPoint.Speed_run);
        }
        //모든 캐릭터 이동 끝나는지 확인
        int check = 0;
        while(check != myChar.Count) {
            check = 0;
            for (int i = 0; i < myChar.Count; i++) {
                if (myChar[i].isIdle()) {
                    check++;
                }
            }
            yield return new WaitForSeconds(0.5f);  //0.5초마다 확인
        }
        myChar.Clear();
        myChar = null;
        light.fadeOut(3.0f);

        //숨바곡질 시작 씬 로드
        SceneManager.LoadScene("1_Hall");
        yield return new WaitForSeconds(0.01f);

        ch_main = Instantiate(pre_ch_mainch);
        ch_hyojung = Instantiate(pre_ch_hj);
        ch_habin = Instantiate(pre_ch_hb);
        ch_jungyeon = Instantiate(pre_ch_jy);
        ch_suun = Instantiate(pre_ch_su);
        light_obj = Instantiate(pre_light_direc);
        yield return new WaitForSeconds(0.01f);         //instantiate후 지연

        Vector3 pos = new Vector3(-4f, -2.5f, 0f);      //위치 설정
        ch_main.transform.position = pos;
        ch_habin.transform.position = pos;
        ch_hyojung.transform.position = pos;
        ch_suun.transform.position = pos;
        ch_jungyeon.transform.position = pos;

        light = light_obj.GetComponent<Light_Directional>();
        light.fadeIn(2.0f);        //페이드인
        yield return new WaitForSeconds(1f);    //대사 전 딜레이

        //ScriptManager 1차숨바꼭질시작 대사 진행 요청

        //while(ScriptManager 대사 요청 완료 확인)

        //다들 숨으러감(술래 제외, 술래는 누구?)
        myChar = new List<MoveWayPoint>();
        myChar.Add(ch_main.GetComponent<MoveWayPoint>());
        myChar.Add(ch_habin.GetComponent<MoveWayPoint>());
        myChar.Add(ch_hyojung.GetComponent<MoveWayPoint>());
        myChar.Add(ch_jungyeon.GetComponent<MoveWayPoint>());
        myChar.Add(ch_suun.GetComponent<MoveWayPoint>());

        for (int i = 0; i < myChar.Count; i++) {
            myChar[i].move(new Vector2[] { new Vector2(0.5f, 0) }, MoveWayPoint.Speed_run);
        }
        //모든 캐릭터 이동 끝나는지 확인
        check = 0;
        while (check != myChar.Count) {
            check = 0;
            for (int i = 0; i < myChar.Count; i++) {
                if (myChar[i].isIdle()) {
                    check++;
                }
            }
            yield return new WaitForSeconds(0.5f);  //0.5초마다 확인
        }

        //이제 진행끝 숨바꼭질시작.//씬 로드
        SceneManager.LoadScene("1_Hall");
        yield return new WaitForSeconds(0.01f);

        GameObject player = Instantiate(pre_player);
        yield return new WaitForSeconds(0.01f);
        player.transform.position = new Vector3(0f, 0f, 0f);

        yield break;
    }
    
}