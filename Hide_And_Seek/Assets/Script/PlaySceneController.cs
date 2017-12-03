using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneController : MonoBehaviour {
    //스토리 진행에 필요한 프리팹
    private enum char_num { hj = 0, hb = 1, jy = 2, su = 3, ps = 4, main = 5 };
    public GameObject[] pre_char;
    public GameObject pre_light_direc, pre_light_flash;

    private enum sprite_num { doll1 = 0, doll2 = 1};
    public GameObject pre_obj;
    public List<Sprite> pre_sprite;
    //재생중 필요한 오브젝트
    private List<MoveWayPoint> moveWaitChar;
    private GameObject[] obj_char;

    delegate IEnumerator del_type_test();
    del_type_test play;

    private bool isWaitScript;

    public void setScene(PlayScene.numScene n) {
        if (n == PlayScene.numScene.tutorial) play = playTutorial;
        else if (n == PlayScene.numScene.hide_1_end) play = playHide1;
        else if (n == PlayScene.numScene.hide_2_ready) play = playHide2Ready;

        else if (n == PlayScene.numScene.ringPhone) play = ringPhone;

        else if (n == PlayScene.numScene.habin_nosalt) play = nosalt;
        else if (n == PlayScene.numScene.habin_havesalt) play = havesalt;
        else if (n == PlayScene.numScene.no_jy) play = nojy;

        else if (n == PlayScene.numScene.no_hb) play = nohb;
        else if (n == PlayScene.numScene.hb_die) play = hbd;

        else if (n == PlayScene.numScene.break_cabinet) play = breakcabi;
        else if (n == PlayScene.numScene.after_break) play = afterbreak;
        else if (n == PlayScene.numScene.jy_die) play = jyd;

        else if (n == PlayScene.numScene.JeongYeon) play = end_jy;
        else if (n == PlayScene.numScene.Invalid_Obj) play = end_obj;
        else if (n == PlayScene.numScene.suspectDoll) play = end_d;
        else if (n == PlayScene.numScene.suspectKim) play = end_k;
        else if (n == PlayScene.numScene.ending_exit) play = end_exit;
        else if (n == PlayScene.numScene.batteryLack) play = end_battery;
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
        yield return new WaitUntil(()=>!ScriptManager.getInstance().isPlaying);
        obj_char = new GameObject[6];

        //Sound 짹짹 경쾌 새지저귐
        //SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.));

        //씬 로드
        SceneManager.LoadScene("S_BBQ");
        yield return new WaitForSeconds(2f);

        //대사 진행
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 1, 2, 3, 4, 5 }, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //바베큐장 씬 로드
        GameObject.Find("Main Camera").GetComponent<CameraScript>().setPosition(new Vector2(0f, 0f));
        GameObject.Find("Main Camera").GetComponent<CameraScript>().setSize(3);
        for (int i = 0; i < 6; i++) {
            obj_char[i] = Instantiate(pre_char[i]);
            DontDestroyOnLoad(obj_char[i]);
        }
        GameObject light_obj = Instantiate(pre_light_direc);
        yield return new WaitForSeconds(0.001f);                 //instantiate후 지연

        //위치 설정
        GameObject.Find("Main Camera").GetComponent<CameraScript>().setPosition(new Vector2(0f, -0.5f));

        obj_char[(int)char_num.main].transform.position = new Vector3(3.25f, -1.64f, 0);
        obj_char[(int)char_num.hj].transform.position = new Vector3(0.1f, -1.93f, 0);
        obj_char[(int)char_num.hb].transform.position = new Vector3(-0.8f, 0.35f, 0);
        obj_char[(int)char_num.jy].transform.position = new Vector3(-1.382864f, -1.384479f, 0);
        obj_char[(int)char_num.su].transform.position = new Vector3(1.873192f, 0.2435494f, 0);
        obj_char[(int)char_num.ps].transform.position = new Vector3(-3.23f, -0.12f, 0);

        obj_char[(int)char_num.hj].transform.localScale = new Vector3(-1f, 1f, 1f);
        obj_char[(int)char_num.jy].transform.localScale = new Vector3(-1f, 1f, 1f);
        obj_char[(int)char_num.ps].transform.localScale = new Vector3(-1f, 1f, 1f);

        Light_Directional light = light_obj.GetComponent<Light_Directional>();
        light.fadeIn(2.0f);   //페이드인
        yield return new WaitForSeconds(3f);    //대사 전 딜레이

        //ScriptManager 바베큐장 대사 진행 요청
        isWaitScript = true;
        int[] scripts = new int[5];
        for (int i = 0; i <= 4; i++) {
            scripts[i] = i+6;
        }
        ScriptManager.getInstance().showScript(false, scripts, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //Sound 스크립트 중간에 끊고 카메라 찰칵소리
        //SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.));
        isWaitScript = true;
        int[] scripts2 = new int[22];
        for (int i = 0; i <= 21; i++) {
            scripts2[i] = i + 11;
        }
        ScriptManager.getInstance().showScript(false, scripts2, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //펜션 내부로 이동
        moveWaitChar = new List<MoveWayPoint>();
        moveWaitChar.Add(obj_char[(int)char_num.main].GetComponent<MoveWayPoint>());
        moveWaitChar.Add(obj_char[(int)char_num.hb].GetComponent<MoveWayPoint>());
        moveWaitChar.Add(obj_char[(int)char_num.hj].GetComponent<MoveWayPoint>());
        moveWaitChar.Add(obj_char[(int)char_num.jy].GetComponent<MoveWayPoint>());
        moveWaitChar.Add(obj_char[(int)char_num.su].GetComponent<MoveWayPoint>());

        obj_char[(int)char_num.su].GetComponent<MoveWayPoint>().move(
            new Vector2[] { new Vector2(-0.19f, 1.27f), new Vector2(-20, -1) }, MoveWayPoint.Speed_walk);
        obj_char[(int)char_num.hb].GetComponent<MoveWayPoint>().move(
            new Vector2[] { new Vector2(-2.68f, 1.1f), new Vector2(-20, -1) }, MoveWayPoint.Speed_walk);
        obj_char[(int)char_num.main].GetComponent<MoveWayPoint>().move(
            new Vector2[] { new Vector2(0.86f, -3.14f), new Vector2(-20, -1) }, MoveWayPoint.Speed_walk);
        obj_char[(int)char_num.hj].GetComponent<MoveWayPoint>().move(
            new Vector2[] { new Vector2(-20, -1) }, MoveWayPoint.Speed_walk);
        obj_char[(int)char_num.jy].GetComponent<MoveWayPoint>().move(
            new Vector2[] { new Vector2(-20, -1) }, MoveWayPoint.Speed_walk);

        yield return new WaitForSeconds(3f);    //이동 3초 지연
        light.fadeOut(1.0f);
        yield return new WaitForSeconds(1f);    //페이드아웃 지연

        for (int i = 0; i < moveWaitChar.Count; i++) {  //멈춤
            moveWaitChar[i].stop();
        }

        //숨바곡질 시작 씬 로드
        obj_char[(int)char_num.main].transform.position = new Vector3(-2.13f, -0.97f, 0);
        obj_char[(int)char_num.hj].transform.position = new Vector3(1.330915f, -1.546847f, 0);
        obj_char[(int)char_num.hb].transform.position = new Vector3(-0.64f, 0.08f, 0);
        obj_char[(int)char_num.jy].transform.position = new Vector3(-0.05f, -1.1f, 0);
        obj_char[(int)char_num.su].transform.position = new Vector3(1.953645f, -0.4161013f, 0);
        Destroy(obj_char[(int)char_num.ps]);

        obj_char[(int)char_num.hb].transform.localScale = new Vector3(-1f, 1f, 1f);
        obj_char[(int)char_num.main].transform.localScale = new Vector3(-1f, 1f, 1f);

        SceneManager.LoadScene("1_Living");
        yield return new WaitForSeconds(0.001f);

        GameObject.Find("Main Camera").GetComponent<CameraScript>().setPosition(new Vector2(0f, -1f));
        light_obj = Instantiate(pre_light_direc);
        yield return new WaitForSeconds(0.001f);         //instantiate후 지연
        
        light = light_obj.GetComponent<Light_Directional>();
        light.fadeIn(1.0f);        //페이드인
        yield return new WaitForSeconds(2f);    //대사 전 딜레이

        //ScriptManager 1차숨바꼭질시작 대사 진행 요청
        isWaitScript = true;
        scripts = new int[8];
        for (int i = 0; i <= 7; i++) {
            scripts[i] = i+50;
        }
        ScriptManager.getInstance().showScript(false, scripts, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //정적
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("Main Camera").GetComponent<CameraScript>().zoom(new Vector2(-2f, -0.5f), 2);

        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 58, 59, 60 }, wake);
        yield return new WaitUntil(() => !isWaitScript);

        GameObject.Find("Main Camera").GetComponent<CameraScript>().zoom(new Vector2(0f, -1f), 4);

        //Player빼고 전부 숨으러간다
        moveWaitChar.Remove(obj_char[(int)char_num.main].GetComponent<MoveWayPoint>());
        for (int i = 0; i < moveWaitChar.Count; i++) {
            moveWaitChar[i].move(new Vector2[] { new Vector2(3.29f, -1.64f) }, MoveWayPoint.Speed_run);
        }
        //모든 캐릭터 이동 끝나는지 확인
        int check = 0;
        int total = moveWaitChar.Count;
        while (check < total) {
            for (int i = 0; i < moveWaitChar.Count; i++) {
                try {
                    if (moveWaitChar[i].isIdle()) {
                        Destroy(moveWaitChar[i].gameObject);
                        check++;
                    }
                } catch {

                }
            }
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        
        //이제 진행끝 숨바꼭질시작.//씬 로드
        for (int i = 0; i < obj_char.Length; i++) {
            try { Destroy(obj_char[i]); } catch { }
        }

        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("1_Living");
        yield return new WaitForSeconds(0.001f);

        //플레이어 위치설정
        GameObject pl = GameObject.Find("Player");
        pl.transform.position = new Vector3(-2.13f, -0.97f, 0);
        Vector3 scale = pl.transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        pl.transform.localScale = scale;

        Destroy(this.gameObject);
        Destroy(this);
        yield break;
    }
    private IEnumerator playHide1() {
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);
        obj_char = new GameObject[6];

        //Sound 경쾌한브금..?
        //SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.));
        //씬 로드
        SceneManager.LoadScene("1_Living");
        yield return new WaitForSeconds(0.001f);
        
        for (int i = 0; i < 6; i++) {
            if (i == (int)char_num.ps) continue;
            obj_char[i] = Instantiate(pre_char[i]);
        }
        GameObject light_obj = Instantiate(pre_light_direc);
        GameObject.Find("Main Camera").GetComponent<CameraScript>().setPosition(new Vector2(0f, -1f));
        yield return new WaitForSeconds(0.001f);                 //instantiate후 지연
        
        //위치 설정
        obj_char[(int)char_num.main].transform.position = new Vector3(-2.13f, -0.97f, 0);
        obj_char[(int)char_num.hj].transform.position = new Vector3(1.330915f, -1.546847f, 0);
        obj_char[(int)char_num.hb].transform.position = new Vector3(-0.64f, 0.08f, 0);
        obj_char[(int)char_num.jy].transform.position = new Vector3(-0.05f, -1.1f, 0);
        obj_char[(int)char_num.su].transform.position = new Vector3(1.953645f, -0.4161013f, 0);
        Destroy(obj_char[(int)char_num.ps]);

        obj_char[(int)char_num.hb].transform.localScale = new Vector3(-1f, 1f, 1f);
        obj_char[(int)char_num.main].transform.localScale = new Vector3(-1f, 1f, 1f);

        Light_Directional light = light_obj.GetComponent<Light_Directional>();
        light.fadeIn(2.0f);   //페이드인
        yield return new WaitForSeconds(3f);    //대사 전 딜레이

        //대사 진행
        isWaitScript = true;
        int[] scripts = new int[9];
        for (int i = 0; i <= 8; i++) {
            scripts[i] = i + 100;
        }
        ScriptManager.getInstance().showScript(false, scripts, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //인형
        GameObject go = new GameObject();
        yield return new WaitForSeconds(0.001f);
        SpriteRenderer go_sp = go.AddComponent<SpriteRenderer>();
        go_sp.sprite = pre_sprite[(int)sprite_num.doll1];
        go_sp.sortingLayerName = "Object_front";
        go.transform.position = new Vector3(-0.44f, -0.52f, 0f);
        GameObject.Find("Main Camera").GetComponent<CameraScript>().zoom(new Vector2(-0.39f, -0.7f), 1);
        yield return new WaitForSeconds(0.5f);

        //대사 진행
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 109, 110, 111 }, wake);
        yield return new WaitUntil(() => !isWaitScript);
        GameObject.Find("Main Camera").GetComponent<CameraScript>().zoom(new Vector2(0f, -1f), 4);

        //대사 진행, 소주를찾으러
        isWaitScript = true;
        scripts = new int[11];
        for (int i = 0; i <= 10; i++) {
            scripts[i] = i + 112;
        }
        ScriptManager.getInstance().showScript(false, scripts, wake);
        yield return new WaitUntil(() => !isWaitScript);

        MoveWayPoint move = obj_char[(int)char_num.main].GetComponent<MoveWayPoint>();
        move.move(new Vector2[] {new Vector2(0.07f, -0.24f) , new Vector2(3.46f, -1.48f) }, MoveWayPoint.Speed_run);
        yield return new WaitUntil(()=>move.isIdle());

        //씬종료
        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("1_Hall");
        yield return new WaitForSeconds(0.001f);

        //플레이어 위치설정
        GameObject pl = GameObject.Find("Player");
        pl.transform.position = new Vector3(-4.5f, -3f, 0);
        Vector3 scale = pl.transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        pl.transform.localScale = scale;

        pl.GetComponent<Player>().Light.fadeIn(1.0f);

        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }
    private IEnumerator playHide2Ready() {
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);
        Inventory.getInstance().deleteItem(12);
        obj_char = new GameObject[6];

        //씬 로드
        SceneManager.LoadScene("1_Bath");
        yield return new WaitForSeconds(0.001f);

        for (int i = 0; i < 6; i++) {
            if (i == (int)char_num.ps) continue;
            obj_char[i] = Instantiate(pre_char[i]);
        }
        GameObject light_obj = Instantiate(pre_light_direc);
        GameObject doll = Instantiate(pre_obj);

        yield return new WaitForSeconds(0.001f);                 //instantiate후 지연

        GameObject.Find("Main Camera").GetComponent<CameraScript>().setPosition(new Vector2(1.5f, -2.3f));
        SpriteRenderer doll_sp = doll.GetComponent<SpriteRenderer>();
        doll_sp.sprite = pre_sprite[(int)sprite_num.doll1];
        doll_sp.sortingLayerName = "Object_front";
        doll.transform.position = new Vector3(1.41f, -2.09f, 0f);

        //위치 설정
        obj_char[(int)char_num.main].transform.position = new Vector3(0.09f, -2.3f, 0);
        obj_char[(int)char_num.hj].transform.position = new Vector3(2.32f, -2.77f, 0);
        obj_char[(int)char_num.hb].transform.position = new Vector3(1.715f, -1.589f, 0);
        obj_char[(int)char_num.jy].transform.position = new Vector3(0.74f, -1.8f, 0);
        obj_char[(int)char_num.su].transform.position = new Vector3(2.94f, -2.08f, 0);
        Destroy(obj_char[(int)char_num.ps]);

        obj_char[(int)char_num.jy].transform.localScale = new Vector3(-1f, 1f, 1f);
        obj_char[(int)char_num.main].transform.localScale = new Vector3(-1f, 1f, 1f);

        Light_Directional light = light_obj.GetComponent<Light_Directional>();
        light.fadeIn(2.0f);   //페이드인
        yield return new WaitForSeconds(3f);    //대사 전 딜레이

        //대사 진행
        isWaitScript = true;
        int[] scripts = new int[7];
        for (int i = 0; i <= 6; i++) {
            scripts[i] = i + 150;
        }
        ScriptManager.getInstance().showScript(false, scripts, wake);
        yield return new WaitUntil(() => !isWaitScript);

        GameObject.Find("Main Camera").GetComponent<CameraScript>().zoom(new Vector2(1.5f, -2.3f), 1.5f);
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 3; i++) {
            doll_sp.sprite = pre_sprite[(int)sprite_num.doll1];
            yield return new WaitForSeconds(0.5f);
            //Sound 푹
            //SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.));
            isWaitScript = true;
            ScriptManager.getInstance().showScript(false, new int[] { 157 }, wake);
            doll_sp.sprite = pre_sprite[(int)sprite_num.doll2];
            yield return new WaitUntil(() => !isWaitScript);
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 3; i++) {
            yield return new WaitForSeconds(0.5f);
            isWaitScript = true;
            ScriptManager.getInstance().showScript(false, new int[] { 158 }, wake);
            yield return new WaitUntil(() => !isWaitScript);
        }
        light.fadeOut(1.0f);
        yield return new WaitForSeconds(1f);

        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] {160, 161, 162}, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //Sound 비명
        SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.HYOJUNG_KKYAK));

        //Sound BGM 천둥비로 변경
        //SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.));
        SceneManager.LoadScene("1_Reception");

        ////대사 진행
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 200, 201}, wake);
        yield return new WaitUntil(() => !isWaitScript);

        obj_char[(int)char_num.main] = Instantiate(pre_char[(int)char_num.main]);
        FlashLight flash = Instantiate(pre_light_flash).GetComponent<FlashLight>();
        yield return new WaitForSeconds(0.001f);
        obj_char[(int)char_num.main].transform.position = new Vector3(1.26f, 0.04f, 0);
        flash.LinkUser(obj_char[(int)char_num.main]);

        GameObject.Find("Main Camera").GetComponent<CameraScript>().setPosition(new Vector2(-0.5f, -0.5f));
        
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 202 }, wake);
        yield return new WaitUntil(() => !isWaitScript);

        MoveWayPoint move = obj_char[(int)char_num.main].GetComponent<MoveWayPoint>();
        move.move(new Vector2[] { new Vector2(-3.37f, -1.79f) }, MoveWayPoint.Speed_walk);
        yield return new WaitUntil(() => move.isIdle());
        
        //씬종료
        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("1_Hall");
        yield return new WaitForSeconds(0.001f);

        ////플레이어 위치설정
        GameObject pl = GameObject.Find("Player");
        pl.transform.position = new Vector3(2.28f, 0, 0);
        Vector3 scale = pl.transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        pl.transform.localScale = scale;
        
        ScriptManager.getInstance().showScript(false, new int[] { 203 });

        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }

    private IEnumerator ringPhone() {
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);
        obj_char = new GameObject[6];
        GameObject gameUI = GameObject.Find("Canvas_UI");
        if (gameUI != null) gameUI.GetComponent<CanvasGroup>().interactable = false;

        SceneManager.LoadScene("2_Bed");
        yield return new WaitForSeconds(0.001f);

        //필요 오브젝트 불러오기
        obj_char[(int)char_num.main] = Instantiate(pre_char[(int)char_num.main]);
        GameObject fl_obj = Instantiate(pre_light_flash);
        yield return new WaitForSeconds(0.001f);

        FlashLight flash = fl_obj.GetComponent<FlashLight>();
        CameraScript camera = GameObject.Find("Main Camera").GetComponent<CameraScript>();

        //설정
        Vector3 pos = Player.get_player_pos();
        obj_char[(int)char_num.main].transform.position = pos;
        flash.setPosition(pos);
        flash.fadeIn(1.0f);
        yield return new WaitForSeconds(1f);

        //Sound 시체발견
        //SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.));

        //하이라이트 이동
        flash.move(new Vector2(-1.19f, -0.98f));
        camera.zoom(new Vector2(-1.19f, -0.98f), 4f);

        //대사 진행
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 750,751 }, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //하이라이트 이동
        flash.move(pos);
        camera.zoom(pos, 4f);
        yield return new WaitForSeconds(0.5f);

        //대사진행
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 752,753,754,755,756,757 }, wake);
        yield return new WaitUntil(() => !isWaitScript);

        for (int i = 758; i <= 761; i++) {
            //Sound 메시지
            //SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.));
            isWaitScript = true;
            ScriptManager.getInstance().showScript(false, new int[] {i }, wake);
            yield return new WaitUntil(() => !isWaitScript);
        }

        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] {762, 763}, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //Sound 두루루
        SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.CALLING));
        yield return new WaitForSeconds(2f);
        //Sound 띠리링
        SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.BELLSOUND));
        yield return new WaitForSeconds(1f);
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 764,765 }, wake);
        yield return new WaitUntil(() => !isWaitScript);
        //Sound 크큭
        SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.LAUGH_ENEMY));
        ScriptManager.getInstance().showScript(false, new int[] { 766 });

        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("2_Bed");
        gameUI = GameObject.Find("Canvas_UI");
        if (gameUI != null) gameUI.GetComponent<CanvasGroup>().interactable = true;

        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }

    private IEnumerator nosalt() {
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);

        GameObject gameUI = GameObject.Find("Canvas_UI");
        if (gameUI != null) gameUI.GetComponent<CanvasGroup>().interactable = false;

        obj_char = new GameObject[6];
        //씬로드
        SceneManager.LoadScene("2_Hall");
        yield return new WaitForSeconds(0.001f);

        //obj_char[(int)char_num.hb] = Instantiate(pre_char[(int)char_num.hb]);
        obj_char[(int)char_num.main] = Instantiate(pre_char[(int)char_num.main]);
        FlashLight flash = Instantiate(pre_light_flash).GetComponent<FlashLight>();
        yield return new WaitForSeconds(0.001f);

        //위치 설정
        obj_char[(int)char_num.main].transform.position = new Vector3(3f, -0.2f, 0);
        //obj_char[(int)char_num.hb].transform.position = new Vector3(1.68f, -0.42f, 0);
        //obj_char[(int)char_num.hb].transform.localScale = new Vector3(-1f, 1f, 1f);
        flash.LinkUser(obj_char[(int)char_num.main]);
        GameObject.Find("Main Camera").GetComponent<CameraScript>().linkUser(obj_char[(int)char_num.main]);
        
        //대사 진행
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 300, 301, 350, 351, 353 }, wake);
        yield return new WaitUntil(() => !isWaitScript);

        GameObject.Find("Main Camera").GetComponent<CameraScript>().shakeCamera();
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 354,355,356,357 }, wake);
        yield return new WaitUntil(() => !isWaitScript);
        
        //씬종료
        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("1_Hall");
        yield return new WaitForSeconds(0.001f);

        ////플레이어 위치설정
        GameObject pl = GameObject.Find("Player");
        pl.transform.position = new Vector3(2.28f, 0, 0);


        gameUI = GameObject.Find("Canvas_UI");
        if (gameUI != null) gameUI.GetComponent<CanvasGroup>().interactable = true;

        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }
    private IEnumerator havesalt() {
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);

        GameObject gameUI = GameObject.Find("Canvas_UI");
        if (gameUI != null) gameUI.GetComponent<CanvasGroup>().interactable = false;

        obj_char = new GameObject[6];
        //씬로드
        SceneManager.LoadScene("2_Hall");
        yield return new WaitForSeconds(0.001f);

        //obj_char[(int)char_num.hb] = Instantiate(pre_char[(int)char_num.hb]);
        obj_char[(int)char_num.main] = Instantiate(pre_char[(int)char_num.main]);
        FlashLight flash = Instantiate(pre_light_flash).GetComponent<FlashLight>();
        yield return new WaitForSeconds(0.001f);

        //위치 설정
        obj_char[(int)char_num.main].transform.position = new Vector3(3f, -0.2f, 0);
        //obj_char[(int)char_num.hb].transform.position = new Vector3(1.68f, -0.42f, 0);
        //obj_char[(int)char_num.hb].transform.localScale = new Vector3(-1f, 1f, 1f);
        flash.LinkUser(obj_char[(int)char_num.main]);
        GameObject.Find("Main Camera").GetComponent<CameraScript>().linkUser(obj_char[(int)char_num.main]);

        //대사 진행
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] {300,301, 400,401,402,403,404,405 }, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //씬종료
        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("1_Hall");
        yield return new WaitForSeconds(0.001f);

        ////플레이어 위치설정
        GameObject pl = GameObject.Find("Player");
        pl.transform.position = new Vector3(2.28f, 0, 0);


        gameUI = GameObject.Find("Canvas_UI");
        if (gameUI != null) gameUI.GetComponent<CanvasGroup>().interactable = true;

        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }
    private IEnumerator nojy() {
        GameManager.getInstance().isScenePlay = false;
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);

        SceneManager.LoadScene("1_Hall");
        yield return new WaitForSeconds(0.001f);

        ////플레이어 위치설정
        GameObject pl = GameObject.Find("Player");
        pl.transform.position = new Vector3(2.28f, 0, 0);

        //대사 진행
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 450,451 }, wake);

        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }

    private IEnumerator nohb() {
        GameManager.getInstance().isScenePlay = false;
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);

        SceneManager.LoadScene("2_Hall");
        yield return new WaitForSeconds(0.001f);

        ////플레이어 위치설정
        GameObject pl = GameObject.Find("Player");
        pl.transform.position = new Vector3(3f, -0.2f, 0);

        //대사 진행
        ScriptManager.getInstance().showScript(false, new int[] { 500, 501 });

        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }
    private IEnumerator hbd() {
        GameManager.getInstance().isScenePlay = false;
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);

        SceneManager.LoadScene("2_Swimming");
        yield return new WaitForSeconds(0.001f);

        //필요 오브젝트 불러오기
        GameObject pl = GameObject.Find("Player");
        FlashLight flash = GameObject.Find("Flash").GetComponent<FlashLight>();
        CameraScript camera = GameObject.Find("Main Camera").GetComponent<CameraScript>();

        //설정
        pl.transform.position = new Vector3(-1.8f, -4.25f, 0);
        flash.LinkUser(null);
        flash.setPosition(new Vector2(-1.8f, -4.25f));
        flash.fadeIn(1.0f);

        //대사 진행
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 550, 551 }, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //하이라이트 이동
        ScriptManager.getInstance().showScript(false, new int[] { 552 });
        flash.move(new Vector2(-1.9f, -1.47f));
        camera.linkUser(null);
        camera.zoom(new Vector2(-1.9f, -2.35f), 4f);

        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 553,554,555 }, wake);
        yield return new WaitUntil(() => !isWaitScript);
        
        flash.LinkUser(pl);
        camera.linkUser(pl);

        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }

    private IEnumerator breakcabi() {
        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("2_Dress");
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);

        //쨍
        //Sound
        SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.HUTCH_CRASH));
        ScriptManager.getInstance().showScript(false, new int[] {600 });

        //대사진행
        ScriptManager.getInstance().showScript(false, new int[] { 601, 602 });
        
        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }
    private IEnumerator afterbreak() {
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);

        yield return new WaitForSeconds(0.5f);

        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 650, 651 }, wake);
        yield return new WaitUntil(() => !isWaitScript);

        yield return new WaitForSeconds(2f);
        //Sound 웅웅
        SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.WASHER));

        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] {652 }, wake);
        yield return new WaitUntil(() => !isWaitScript);
        
        GameManager.getInstance().isScenePlay = false;
        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }
    private IEnumerator jyd() {
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);

        SceneManager.LoadScene("1_Laundry");
        yield return new WaitForSeconds(0.001f);

        //필요 오브젝트 불러오기
        obj_char = new GameObject[6];
        obj_char[(int)char_num.main] = Instantiate(pre_char[(int)char_num.main]);
        GameObject obj_flash = Instantiate(pre_light_flash);
        yield return new WaitForSeconds(0.001f);
        
        CameraScript camera = GameObject.Find("Main Camera").GetComponent<CameraScript>();

        //설정
        obj_char[(int)char_num.main].transform.position = new Vector3(1.38f, -2.69f, 0);
        FlashLight flash = obj_flash.GetComponent<FlashLight>();
        flash.setPosition(new Vector2(1.38f, -2.69f));
        flash.fadeIn(1.0f);
        yield return new WaitForSeconds(1f);

        //Sound 시체
        //SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.));

        //하이라이트 이동
        flash.move(new Vector2(-0.31f, -1.2f));
        camera.linkUser(null);
        camera.zoom(new Vector2(-0.31f, -1.2f), 4f);

        //대사 진행
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 700}, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //하이라이트 이동
        flash.move(new Vector2(-1f, -0.3f));
        camera.linkUser(null);
        camera.zoom(new Vector2(-1f, -0.3f), 4f);
        yield return new WaitForSeconds(0.5f);

        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 701,702 }, wake);
        yield return new WaitUntil(() => !isWaitScript);

        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("1_Laundry");

        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }

    private IEnumerator end_jy() {
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);
        //Sound 비명
        SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.HYOJUNG_KKYAK));
        GameObject.Find("Main Camera").GetComponent<CameraScript>().shakeCamera();

        yield return new WaitForSeconds(0.5f);
        //Sound 쿵쿵
        SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.WALK));

        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 250, 251 }, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //까매진다 (+피있음조을듯)
        SceneManager.LoadScene("empty");
        yield return new WaitForSeconds(0.1f);
        //Sound 철퍽
        //SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.));
        GameObject.Find("blood_full_1").GetComponent<Animation>().Play();
        yield return new WaitForSeconds(1f);
        //Sound 철퍽
        //SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.));
        GameObject.Find("blood_full_2").GetComponent<Animation>().Play();
        yield return new WaitForSeconds(2f);

        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("UI_Start");

        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }
    private IEnumerator end_obj() {
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);
        GameManager.getInstance().isScenePlay = false;
        yield break;
    }
    private IEnumerator end_d() {
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);
        //Sound 비명
        GameObject.Find("Main Camera").GetComponent<CameraScript>().shakeCamera();
        yield return new WaitForSeconds(0.5f);

        //까매진다 (+피있음조을듯)
        SceneManager.LoadScene("empty");
        yield return new WaitForSeconds(0.5f);
        //Sound 철퍽
        GameObject.Find("blood_full_1").GetComponent<Animation>().Play();
        yield return new WaitForSeconds(1f);
        //Sound 철퍽
        GameObject.Find("blood_full_2").GetComponent<Animation>().Play();
        yield return new WaitForSeconds(2f);

        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("UI_Start");

        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }
    private IEnumerator end_k() {
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);
        //Sound 비명
        GameObject.Find("Main Camera").GetComponent<CameraScript>().shakeCamera();
        yield return new WaitForSeconds(0.5f);

        //까매진다 (+피있음조을듯)
        SceneManager.LoadScene("empty");
        yield return new WaitForSeconds(0.5f);
        //Sound 철퍽
        GameObject.Find("blood_full_1").GetComponent<Animation>().Play();
        yield return new WaitForSeconds(1f);
        //Sound 철퍽
        GameObject.Find("blood_full_2").GetComponent<Animation>().Play();
        yield return new WaitForSeconds(2f);

        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("UI_Start");

        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }
    private IEnumerator end_exit() {
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);
        yield return new WaitForSeconds(0.001f);
        SceneManager.LoadScene("empty");
        yield return new WaitForSeconds(0.001f);

        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 800, 801, 802, 803 }, wake);
        yield return new WaitUntil(() => !isWaitScript);
        
        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("UI_Start");

        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }
    private IEnumerator end_battery() {
        yield return new WaitUntil(() => !ScriptManager.getInstance().isPlaying);
        yield return new WaitForSeconds(0.001f);
        Player pl = GameObject.Find("Player").GetComponent<Player>();
        yield return new WaitForSeconds(0.001f);
        FlashLight flash = pl.Flash;
        yield return new WaitForSeconds(0.001f);
        flash.setLight(true);
        flash.flashedLight(100f);
        yield return new WaitForSeconds(3f);

        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 850}, wake);
        yield return new WaitUntil(() => !isWaitScript);

        flash.setLight(false);
        yield return new WaitForSeconds(0.5f);
        //Sound 종료음
        //SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.));
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 851 }, wake);
        yield return new WaitUntil(() => !isWaitScript);
        //Sound 발소리
        SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.WALK));
        isWaitScript = true;
        ScriptManager.getInstance().showScript(false, new int[] { 852,853 }, wake);
        yield return new WaitUntil(() => !isWaitScript);

        //까매진다
        SceneManager.LoadScene("empty");
        yield return new WaitForSeconds(0.1f);
        //Sound 철퍽
        //SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.));
        GameObject.Find("blood_full_1").GetComponent<Animation>().Play();
        yield return new WaitForSeconds(1f);
        //Sound 철퍽
        //SoundManager.getInstance().playEffect(SoundManager.getInstance().findAudioClip(SOUND_NAME.));
        GameObject.Find("blood_full_2").GetComponent<Animation>().Play();
        yield return new WaitForSeconds(2f);

        GameManager.getInstance().isScenePlay = false;
        SceneManager.LoadScene("UI_Start");

        CancelInvoke();
        Destroy(this.gameObject);
        Destroy(this);

        yield break;
    }
}