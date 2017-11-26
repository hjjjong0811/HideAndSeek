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

    private bool isWaitScript;
    
    public void setScene(PlayScene.numScene n) {
        if (n == PlayScene.numScene.tutorial) play = playTutorial;
        else if (n == PlayScene.numScene.hide_1_end) play = playHide1;
        else if (n == PlayScene.numScene.hide_2_ready) play = playHide2Ready;
        else if (n == PlayScene.numScene.ringPhone) play = ringPhone;
        else if (n == PlayScene.numScene.ending_exit) play = end_1;
    }
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start() {
        StartCoroutine(play());
    }

    public void wake() {
        isWaitScript = false;
    }

    private IEnumerator playTutorial() {
        //씬 로드
        SceneManager.LoadScene("S_BBQ");
        yield return new WaitForSeconds(2f);

        //대사 진행
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 1, 2, 3, 4, 5 }, wake);
        yield return new WaitUntil(()=>!isWaitScript);

        //바베큐장 씬 로드
        GameObject ch_main = Instantiate(pre_ch_mainch);
        GameObject ch_hyojung = Instantiate(pre_ch_hj);
        GameObject ch_habin = Instantiate(pre_ch_hb);
        GameObject ch_jungyeon = Instantiate(pre_ch_jy);
        GameObject ch_suun = Instantiate(pre_ch_su);
        GameObject ch_pension = Instantiate(pre_ch_ps);
        GameObject light_obj = Instantiate(pre_light_direc);
        yield return new WaitForSeconds(0.001f);                 //instantiate후 지연

        //위치 설정
        ch_main.transform.position = new Vector3(3.25f, -1.64f, 0);
        ch_habin.transform.position = new Vector3(-0.8f, 0.35f, 0);
        ch_hyojung.transform.position = new Vector3(0.1f, -1.93f, 0);
        ch_suun.transform.position = new Vector3(1.873192f, 0.2435494f, 0);
        ch_jungyeon.transform.position = new Vector3(-1.382864f, -1.384479f, 0);
        ch_pension.transform.position = new Vector3(-3.23f, -0.12f, 0);

        Light_Directional light = light_obj.GetComponent<Light_Directional>();
        light.fadeIn(2.0f);   //페이드인
        yield return new WaitForSeconds(3f);    //대사 전 딜레이

        //ScriptManager 바베큐장 대사 진행 요청
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 6, 7}, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //펜션 내부로 이동
        myChar = new List<MoveWayPoint>();
        myChar.Add(ch_main.GetComponent<MoveWayPoint>());
        myChar.Add(ch_habin.GetComponent<MoveWayPoint>());
        myChar.Add(ch_hyojung.GetComponent<MoveWayPoint>());
        myChar.Add(ch_jungyeon.GetComponent<MoveWayPoint>());
        myChar.Add(ch_suun.GetComponent<MoveWayPoint>());

        ch_suun.GetComponent<MoveWayPoint>().move(
            new Vector2[] { new Vector2(-0.19f, 1.27f), new Vector2(-20, -1) }, MoveWayPoint.Speed_walk);
        ch_habin.GetComponent<MoveWayPoint>().move(
            new Vector2[] { new Vector2(-2.68f, 1.1f), new Vector2(-20, -1) }, MoveWayPoint.Speed_walk);
        ch_main.GetComponent<MoveWayPoint>().move(
            new Vector2[] { new Vector2(0.86f, -3.14f), new Vector2(-20, -1) }, MoveWayPoint.Speed_walk);
        ch_hyojung.GetComponent<MoveWayPoint>().move(
            new Vector2[] {new Vector2(-20, -1) }, MoveWayPoint.Speed_walk);
        ch_jungyeon.GetComponent<MoveWayPoint>().move(
            new Vector2[] { new Vector2(-20, -1) }, MoveWayPoint.Speed_walk);

        yield return new WaitForSeconds(3f);    //3초지연
        light.fadeOut(2.0f);
        yield return new WaitForSeconds(2f);    //2초지연

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
        int check = 0;
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
        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("1_Hall");

        yield break;
    }

    private IEnumerator playHide1() {
        GameManager.getInstance().isScenePlay = false;
        yield break;
    }

    private IEnumerator playHide2Ready() {
        GameManager.getInstance().isScenePlay = false;
        yield break;
    }

    private IEnumerator ringPhone() {
        GameManager.getInstance().isScenePlay = false;
        yield break;
    }

    private IEnumerator end_1() {
        GameManager.getInstance().isScenePlay = false;
        yield break;
    }
}