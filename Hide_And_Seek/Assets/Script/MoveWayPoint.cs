using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWayPoint : MonoBehaviour {
    public readonly float Speed_walk = 1, Speed_run = 2.5f;         //for animation
    public readonly int Ani_Idle = 0, Ani_Walk = 1, Ani_Run = 2;

    public bool isCharacter;    //isCharacter?, is Setted at Prefab inspector, And if true set Animator
    public Animator animator;

    public Vector2[] wayPoint;      //이동포인트
    public int pointIndex;
    public float speed;

	// Use this for initialization
	void Start () {
        if(isCharacter) animator = GetComponent<Animator>();
        pointIndex = 0;

        //test
        move(new Vector2[] {new Vector2(0.8f,-1.68f), new Vector2(2.69f,-0.1f) }, Speed_walk);
	}

    private void Update() {
        if (isCharacter) {  //캐릭터인경우 애니메이션 초기화
            animator.SetInteger("State", Ani_Idle);
            animator.speed = Speed_walk;
        }
        //목표 웨이포인트 존재시
        if(wayPoint != null) {
            //이동
            transform.position = Vector2.MoveTowards(transform.position, wayPoint[pointIndex], speed * Time.deltaTime);
            //캐릭터면 애니메이션
            if (isCharacter) setAni(wayPoint[pointIndex]);
            //목표지점 하나 이동완료
            if (wayPoint[pointIndex].Equals((Vector2)transform.position)) {
                pointIndex++;   //다음목표로
                //모든지점 완료?
                if (pointIndex == wayPoint.Length) {
                    wayPoint = null;    //이동종료
                    pointIndex = 0;
                }
            }

        }
    }

    private void setAni(Vector2 target) {
        animator.SetBool("Back", false);

        //속도별 애니메이션
        if (speed == Speed_run) {
            animator.SetInteger("State", Ani_Run);
            animator.speed = Speed_run;
        }else { animator.SetInteger("State", Ani_Walk); }

        //수평 이동값 계산
        float horizontal = (target - (Vector2)transform.position).normalized.x;
        //위로가는경우 뒷모습
        if (horizontal < 0.4 && horizontal > -0.4) {
            animator.SetBool("Back", true);
        }
        //좌우반전
        if (horizontal > 0) {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        } else if (horizontal < 0) {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }


    }

    //이동 함수. 이동하고싶은 점 배열로 줘
    public void move(Vector2[] pWayPoints, float pSpeed) {
        wayPoint = null;
        pointIndex = 0;
        wayPoint = pWayPoints;
        this.speed = pSpeed;
    }
}
