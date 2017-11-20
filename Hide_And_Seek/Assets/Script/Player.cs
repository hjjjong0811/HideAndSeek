using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static GameObject Player_obj;//호빈추가
    public readonly float Speed_walk = 1, Speed_run = 2.5f, Hp_max = 300;
    public readonly int Ani_Idle = 0, Ani_Walk = 1, Ani_Run = 2;

    public float Hp, Speed;
    public bool Tire;

    public Animator Animator;
    public Move move;

    public GameObject Flash_Prefab;
    public FlashLight Flash;

    public GameObject Inventory_Prefab; //inventory open
    public bool isOpenInven = false;

	// Use this for initialization
	void Start () {

        /*정원추가*/
        GameObject prefab = Resources.Load("Prefabs/Canvas_UI") as GameObject;
        GameObject GameUI = MonoBehaviour.Instantiate(prefab) as GameObject;
        GameUI.name = "GameUI";

        Player_obj = this.gameObject;//호빈추가
        Hp = Hp_max;
        Tire = false;
        Animator = GetComponent<Animator>();
        move = GetComponent<Move>();
        Speed = Speed_walk;

        GameObject f = Instantiate(Flash_Prefab);
        Flash = f.GetComponent<FlashLight>();
        Flash.LinkUser(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
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

        //test(inventory open)
        if (Input.GetKeyDown(KeyCode.I)) {
            if (isOpenInven) {
                GameObject.Destroy(GameObject.Find("TTTTEST"));
                isOpenInven = false;
            }else {
                GameObject test = Instantiate(Inventory_Prefab);
                test.name = "TTTTEST";
                isOpenInven = true;
            }
        }
    }

    void movement() {
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

    void action() {
        GameObject nearObject = findNearObject();
        if(nearObject != null) {
            //gameObject.SendMessage("action");
            Debug.Log(nearObject.name + " Player_action");
        }
    }

    void action_item() {
        GameObject nearObject = findNearObject();
        if (nearObject != null) {
            //gameObject.SendMessage("action");
            Debug.Log(nearObject.name +" Player_action");
        }
    }

    GameObject findNearObject() {
        //오브젝트 검사 범위 지정
        Vector2 examdistance = new Vector2(-0.01513481f * transform.localScale.x, -0.7239494f);
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

    void heal() {
        Animator.speed = Speed_walk;
        Tire = false;
    }

    void setLight(bool value) {

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
