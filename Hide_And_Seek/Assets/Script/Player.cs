using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public readonly float Speed_walk = 1, Speed_run = 3, Hp_max = 300;

    public float Hp, Speed;
    public bool Tire;

    public Animator Animator;
    public Move move;

	// Use this for initialization
	void Start () {
        Hp = Hp_max;
        Speed = Speed_walk;
        Tire = false;
        Animator = GetComponent<Animator>();
        move = GetComponent<Move>();
	}
	
	// Update is called once per frame
	void Update () {
        //Animator.SetInteger("human_state", 0);

        Hp = (Hp >= Hp_max) ? Hp_max : Hp + (15f * Time.deltaTime); //시간에따른 hp회복
        Speed = Speed_walk;

        //지치지 않아야 이동, 액션 가능
        if (!Tire) {
            Animator.speed = Speed;
            movement();
            if (Input.GetButtonDown("Action")) {
                action();
            }
        }

        if (Hp <= 0) {
            Tire = true;
            Animator.speed = Speed;
            Invoke("heal", 2.0f);
        }
    }

    void movement() {
        //둘다입력없는 경우 움직이지않음
        if (move.Horizontal == 0 && move.Vertical == 0) {
            return;
        }

        //달리는 경우 체력감소
        if (move.Run) {
            Speed = Speed_run;
            Hp -= 2;
        }
        //이동
        transform.Translate(Vector3.right * Speed * move.Horizontal * Time.deltaTime);
        transform.Translate(Vector3.up * Speed * move.Vertical * Time.deltaTime);

        //애니메이션
        Animator.SetInteger("human_state", 1);
        Animator.speed = Speed;

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
        GameObject gameobject = findNearObject();
        if(gameObject != null) {
            //gameObject.SendMessage("action");
            Debug.Log(gameObject.name);
        }
    }

    void action_item() {
        GameObject gameobject = findNearObject();
        if (gameObject != null) {
            //gameObject.SendMessage("action");
            Debug.Log(gameObject.name);
        }
    }

    GameObject findNearObject() {
        //오브젝트 검사 범위 지정
        Vector2 examdistance = new Vector2(-0.01513481f * transform.localScale.x, -0.7239494f);
        Vector2 examPosition = transform.position;
        examPosition += examdistance;
        Collider2D[] objects = Physics2D.OverlapBoxAll(examPosition, new Vector2(0.1f, 0.1f), 0, 1 << LayerMask.NameToLayer("Object"));

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
}
