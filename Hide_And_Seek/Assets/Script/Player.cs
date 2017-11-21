﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public enum PlayerPrefsIndex {hp = 0, x = 1, y = 2, z = 3, room = 4, spot = 5 };
    public readonly string[] PlayerPrefsKey = {"Player_hp", "Player_x", "Player_y", "Player_z",
        "Player_Pos_room", "Player_Pos_spot"};

    public static GameObject Player_obj;//호빈추가
    public readonly float Speed_walk = 1, Speed_run = 2.5f, Hp_max = 300;
    public readonly int Ani_Idle = 0, Ani_Walk = 1, Ani_Run = 2;

    public float Hp, Speed;
    public bool Tire;
    public ISpot SpotInfo;

    public Animator Animator;
    public Move move;

    public GameObject Flash_Prefab;
    public FlashLight Flash = null;

    private void Awake() {
        //게임중정보 초기화
        Hp = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.hp], Hp_max);
        Vector3 pos = new Vector3();
        pos.x = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], 0);
        pos.y = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], 0);
        pos.z = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], 0);

        //임시
        int room = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.room], 0);
        int spot = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.spot], 0);
        SpotInfo = new ISpot((Room)room, spot);
    }

    /// <summary>
    /// 게임 Load시 플레이어 정보 설정
    /// </summary>
    /// <param name="pHp">체력</param>
    /// <param name="pPosition">위치정보</param>
    /// <param name="pSpot">현재 씬 정보</param>
    public void Init(float pHp, Vector3 pPosition, ISpot pSpot) {
        this.Hp = pHp;
        this.transform.position = pPosition;
        this.SpotInfo._room = pSpot._room;
        this.SpotInfo._spot = pSpot._spot;
    }

    //프로그램 종료시 임시데이터 삭제
    private void OnApplicationQuit() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Delete");
    }

    private void Start () {
        /*정원추가*/
        GameObject prefab = Resources.Load("Prefabs/Canvas_UI") as GameObject;
        GameObject GameUI = MonoBehaviour.Instantiate(prefab) as GameObject;
        GameUI.name = "GameUI";

        Player_obj = this.gameObject;//호빈추가
        Tire = false;
        Animator = GetComponent<Animator>();
        move = GetComponent<Move>();
        Speed = Speed_walk;

        //Flash 생성 및 초기화
        GameObject f = Instantiate(Flash_Prefab);
        f.name = "Flash";
        Flash = f.GetComponent<FlashLight>();
        Flash.Init(this.gameObject);
	}

    private void OnDestroy() {
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], this.transform.position.x);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], this.transform.position.y);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], this.transform.position.z);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.hp], this.Hp);
        PlayerPrefs.SetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.room], (int)this.SpotInfo._room);
        PlayerPrefs.SetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.spot], this.SpotInfo._spot);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    private void Update () {
        Animator.SetInteger("State", Ani_Idle);
        Speed = Speed_walk;
        Hp = (Hp >= Hp_max) ? Hp_max : Hp + (15f * Time.deltaTime); //시간에따른 hp회복
        Animator.SetBool("Back", false);

        //지치지 않아야 이동, 액션 가능
        if (!Tire) {
            movement();
            Animator.speed = Speed;
            if (Input.GetButtonDown("Action")) {
                action();
            }
        }

        if (Hp <= 0) {
            Tire = true;
            Animator.speed = Speed_run;
            Animator.SetInteger("State", Ani_Idle);
            Invoke("heal", 2.0f);
        }

        //손전등
        if (Input.GetButtonDown("Flash")) {
            Flash.setLight(!Flash.getIsLighted());
        }
        //인벤토리
        if (Input.GetButtonDown("Inventory")) {
            GameObject.Find("GameUI").GetComponent<GameUIManager>().Btn_Inven();
        }
        
    }

    private void movement() {
        //둘다입력없는 경우 움직이지않음
        if (move.Horizontal == 0 && move.Vertical == 0) {
            return;
        }

        Animator.SetInteger("State",Ani_Walk);
        //달리는 경우 체력감소, 달리기모션
        if (move.Run) {
            Hp -= 2;
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
        if(nearObject != null) {
            Debug.Log(nearObject.name + " Player_action");
            nearObject.SendMessage("action");
        }
    }

    private void action_item() {
        int itemKey = Inventory.getInstance().curEquipItem;
        if (itemKey == -1) return;
        GameObject nearObject = findNearObject();
        if (nearObject != null) {
            //nearObject.SendMessage("action", itemKey);
            Debug.Log(nearObject.name +" Player_action");
        }
    }

    private GameObject findNearObject() {
        //오브젝트 검사 범위 지정
        Vector2 examdistance = new Vector2(-0.04468793f * transform.localScale.x, 0.006384373f);
        Vector2 examPosition = transform.position;
        examPosition += examdistance;
        Collider2D[] objects = Physics2D.OverlapBoxAll(examPosition, new Vector2(0.1f, 0.1f), 
            0, 1<< LayerMask.NameToLayer("Object"));   //Layer이름 Object인 경우만 조사

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

    public void setLight(bool value) {
        Flash.setLight(value);
    }

    public float getFlashBattery() {
        return Flash.getBattery();
    }

    //호빈추가
    public void set_player_pos(Vector3 v)
    {
        this.transform.position = v;
    }
    public Vector3 get_player_pos()
    {
        return this.transform.position;
    }
}
