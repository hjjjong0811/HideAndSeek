using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public enum PlayerPrefsIndex { hp = 0, x = 1, y = 2, z = 3, room = 4, spot = 5 };
    public static string[] PlayerPrefsKey = {"Player_hp", "Player_x", "Player_y", "Player_z",
        "Player_Pos_room", "Player_Pos_spot"};

    public static GameObject Player_obj;//호빈추가
    public const float Speed_walk = 1, Speed_run = 2.5f, Hp_max = 300;
    public const int Ani_Idle = 0, Ani_Walk = 1, Ani_Run = 2;

    public float Hp, Speed;
    public bool Tire;
    public static bool hiding;//호빈추가
    public GameObject Hiding_UI_Prefab;//호빈추가
    private static GameObject Hiding_UI_Obj;//호빈추가
    public ISpot SpotInfo;
    public static int Player_Last_Portal_num;//호빈추가_ 플레이어가 가장최근에 탄 포탈번호

    public Animator Animator;
    public Move move;

    public GameObject Flash_Prefab;
    /// <summary>
    /// Flash->setLight, getBattery, Flashed...etc
    /// </summary>
    public FlashLight Flash = null;

    public GameObject Light_Prefab;
    public Light_Directional Light = null;


    //호빈추가_오브젝트 앞뒤구분용
    static RaycastHit2D[] hits_up = new RaycastHit2D[] { },
                            hits_down = new RaycastHit2D[] { };

    private void Awake() {
        //게임중정보 초기화
        Hp = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.hp], Hp_max);

        Vector3 pos = new Vector3();
        pos.x = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], 0);
        pos.y = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], 0);
        pos.z = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], 0);
        this.transform.position = pos;

        //임시
        int room = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.room], 0);
        int spot = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.spot], 0);
        SpotInfo = new ISpot((Room)room, spot);

        //Debug.Log(transform.position.x + ", " + transform.position.y + "Awake");
    }

    /// <summary>
    /// 게임 Load시 플레이어 정보 설정
    /// </summary>
    /// <param name="pHp">체력</param>
    /// <param name="pPosition">위치정보</param>
    /// <param name="pSpot">위치한 씬 정보</param>
    public static void Init(float pHp, Vector3 pPosition, ISpot pSpot) {

        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], pPosition.x);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], pPosition.y);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], pPosition.z);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.hp], pHp);
        PlayerPrefs.SetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.room], (int)pSpot._room);
        PlayerPrefs.SetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.spot], pSpot._spot);
        PlayerPrefs.Save();
        GameObject pl = GameObject.Find("Player");
        if (pl != null) {
            pl.transform.position = pPosition;
            pl.GetComponent<Player>().SpotInfo = pSpot;
            pl.GetComponent<Player>().Hp = pHp;
        }
    }

    /// <summary>
    /// 게임 Save시 플레이어 정보 호출, ref키워드에 주의
    /// </summary>
    /// <param name="pHp">체력</param>
    /// <param name="pPosition">위치정보</param>
    /// <param name="pSpot">위치한 씬 정보</param>
    public static void getPlayerData(ref float pHp, ref Vector3 pPosition, ref ISpot pSpot) {
        GameObject pl = GameObject.Find("Player");
        if(pl != null) {
            Player player = pl.GetComponent<Player>();
            pHp = player.Hp;
            pPosition = Player.get_player_pos();
            pSpot = player.SpotInfo;
        } else {
            pHp = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.hp], Hp_max);
            pPosition.x = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], 0);
            pPosition.y = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], 0);
            pPosition.z = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], 0);
            int room = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.room], 0);
            int spot = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.spot], 0);
            pSpot = new ISpot((Room)room, spot);
        }
    }

    private void Start() {
        SpotInfo._room = Scene_Manager.getInstance().get_room_info(SceneManager.GetActiveScene().name);
        int chapter = GameManager.getInstance().GetMainChapter();

        if(GameManager.getInstance().isScenePlay) {
            Destroy(this.gameObject);
            Destroy(this);
            return;
        }

        Player_obj = this.gameObject;//호빈추가
        Tire = false;
        hiding = false;//호빈추가
        Animator = GetComponent<Animator>();
        move = GetComponent<Move>();
        Speed = Speed_walk;

        if (chapter < 4) {
            //Light 생성 및 초기화
            GameObject f = Instantiate(Light_Prefab);
            f.name = "Light";
            Light = f.GetComponent<Light_Directional>();
        } else {
            //Flash 생성 및 초기화
            GameObject f = Instantiate(Flash_Prefab);
            f.name = "Flash";
            Flash = f.GetComponent<FlashLight>();
            Flash.LinkUser(this.gameObject);
        }

    }

    private void OnDestroy() {
        Debug.Log("destroy");
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], this.transform.position.x);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], this.transform.position.y);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], this.transform.position.z);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.hp], this.Hp);
        PlayerPrefs.SetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.room], (int)this.SpotInfo._room);
        PlayerPrefs.SetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.spot], this.SpotInfo._spot);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    private void Update() {
        //씬이나 스크립트 재생중 움직임 불가
        if (ScriptManager.getInstance().isPlaying || GameManager.getInstance().isScenePlay) {
            Animator.SetInteger("State", Ani_Idle);
            Animator.speed = Speed_walk;
            return;
        }
        //호빈추가_ 숨기해제
        if (hiding)
        {
            if (Input.GetButtonDown("Action"))//hiding상태에서 Action키 누르면
            {
                //아저씨위치 조정
                Enemy._enemy.transform.position = Enemy.ENEMY_INIT_LOC;
                Debug.Log("아저씨위치 초기화!");//test

                //Debug.Log("hide 취소");//test
                hiding = false;
                Destroy(Hiding_UI_Obj);
            }
            return;
        }

        Animator.SetInteger("State", Ani_Idle);
        Speed = Speed_walk;
        Hp = (Hp >= Hp_max) ? Hp_max : Hp + (30f * Time.deltaTime); //시간에따른 hp회복
        Animator.SetBool("Back", false);

        //지치지 않아야 이동, 액션 가능
        if (!Tire) {
            movement();
            Animator.speed = Speed;
            if (Input.GetButtonDown("Action")) {
                //호빈_ 숨을수 있는 오브젝트인지 GameObject에서 확인하고, true면 -> 숨고, false면 -> action되게 나중에 바꾸기
                action();
            }
            if (Input.GetButtonDown("UseItem")) {
                action_item();
            }
        }

        if (Hp <= 0) {
            Tire = true;
            Animator.speed = Speed_run;
            Animator.SetInteger("State", Ani_Idle);
            Invoke("heal", 2.0f);
        }

        //손전등
        if (Input.GetButtonDown("Flash") && Flash != null) {
            Flash.setLight(!Flash.getIsLighted());
        }
        //인벤토리
        if (Input.GetButtonDown("Inventory")) {
            GameObject.Find("Canvas_UI").GetComponent<GameUIManager>().Btn_Inven();
        }

        //호빈추가_오브젝트 앞뒤구분용
        Raycasting();
    }

    //호빈추가_오브젝트 앞뒤구분용
    void Raycasting() {
        /*
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].name == "Player")
            {
                hits_up = Physics2D.RaycastAll(this.transform.position, Vector3.up);
                hits_down = Physics2D.RaycastAll(this.transform.position, Vector3.down);
            }
        }
         */
        if (GameObject.Find("Player") == null) return;
        hits_up = Physics2D.RaycastAll(this.transform.position, Vector3.up);
        hits_down = Physics2D.RaycastAll(this.transform.position, Vector3.down);
    }

    //호빈추가
    public static Object_State check_up_down(string s) {
        //if (GameObject.FindGameObjectWithTag("Player") == null) return Object_State.too_far;
        if (GameObject.Find("Player") == null) return Object_State.too_far;
        int i;
        //Debug.Log(hits_up.Length);
        for (i = 0; i < hits_up.Length; i++) {
            if (hits_up[i].collider.name == s) {
                //Debug.Log(hits_up[i].collider.name + " / " + s);
                return Object_State.Object_back;
            }
        }
        for (i = 0; i < hits_down.Length; i++) {
            if (hits_down[i].collider.name == s) {
                //Debug.Log(hits_down[i].collider.name + " / " + s);
                return Object_State.Object_front;
            }
        }
        return Object_State.too_far;
    }

    private void movement() {
        //둘다입력없는 경우 움직이지않음
        if (move.Horizontal == 0 && move.Vertical == 0) {
            return;
        }

        Animator.SetInteger("State", Ani_Walk);
        //달리는 경우 체력감소, 달리기모션
        if (move.Run) {
            Hp -= 1.5f;
            Animator.SetInteger("State", Ani_Run);
            Speed = Speed_run;
            Animator.speed = Speed_run;
        }
        if (move.Vertical > 0 && (move.Horizontal < 0.4 && move.Horizontal > -0.4)) {
            Animator.SetBool("Back", true);
        }

        //이동
        transform.Translate(Vector3.right * Speed * move.Horizontal * Time.deltaTime);
        transform.Translate(Vector3.up * Speed * move.Vertical * Time.deltaTime);


        //좌우반전
        if (move.Horizontal > 0) {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        } else if (move.Horizontal < 0) {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

    }

    private void action() {
        GameObject nearObject = findNearObject();
        if (nearObject != null) {
            //Debug.Log(nearObject.name + " Player_action");
            nearObject.SendMessage("action");
        }
    }

    private void action_item() {
        int itemKey = Inventory.getInstance().curEquipItem;
        if (itemKey == -1) return;
        GameObject nearObject = findNearObject();
        if (nearObject != null) {
            nearObject.SendMessage("useitem", itemKey);
            Debug.Log(nearObject.name + " Player_action");
        }
    }

    private GameObject findNearObject() {
        //오브젝트 검사 범위 지정
        Vector2 examdistance = new Vector2(-0.04468793f * transform.localScale.x, 0.006384373f);
        Vector2 examPosition = transform.position;
        examPosition += examdistance;
        Collider2D[] objects = Physics2D.OverlapBoxAll(examPosition, new Vector2(0.1f, 0.1f),
            0, 1 << LayerMask.NameToLayer("Object"));   //Layer이름 Object인 경우만 조사

        //범위내 오브젝트 X
        if (objects.Length == 0) {
            return null;
        }

        //가장 가까운 오브젝트 조사
        float minDistance = 10;
        int nearObjectIndex = 0;
        for (int i = 0; i < objects.Length; i++) {
            Vector2 heading = objects[i].transform.position - this.transform.position;
            if (minDistance > heading.sqrMagnitude) {
                minDistance = heading.sqrMagnitude;
                nearObjectIndex = i;
            }
        }
        return objects[nearObjectIndex].gameObject;
    }

    private void heal() {
        Animator.speed = Speed_walk;
        Tire = false;
    }

    //호빈추가
    //현정변경 -> GameObject 존재검사 + PlayerPrefs < 안정성때메, static에선 GameObject 다룰때 주의
    public static void set_player_pos(Vector3 v) {
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], v.x);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], v.y);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], v.z);
        PlayerPrefs.Save();
        GameObject pl = GameObject.Find("Player");
        if (pl != null) {
            pl.transform.position = v;
        }
        //Debug.Log(v.x + ", " + v.y + "set");
    }
    public static Vector3 get_player_pos() {
        GameObject go = GameObject.Find("Player");
        if (go != null) {
            return go.transform.position;
        } else {
            Vector3 pos = new Vector3();
            pos.x = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], 0);
            pos.y = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], 0);
            pos.z = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], 0);
            return pos;
        }
    }
    public static ISpot get_player_spot()
    {
        GameObject go = GameObject.Find("Player");
        if(go != null) {
            return Player.Player_obj.GetComponent<Player>().SpotInfo;
        } else {
            int room = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.room], 0);
            int spot = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.spot], 0);
            ISpot ispot = new ISpot((Room)room, spot);
            return ispot;
        }
        
    }

    public void player_hide(){
        Debug.Log("hide...");
        hiding = true;
        GameManager.getInstance().CheckMainChapter();
        Hiding_UI_Obj = Instantiate(Hiding_UI_Prefab);
        Enemy.player_start_hiding(SpotInfo);
    }
}
