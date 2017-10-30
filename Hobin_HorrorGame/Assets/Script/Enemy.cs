using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    Enemy_State[] states = { new Enemy_Normal(), new Enemy_Chase() };
    //route
    int state_num;
    float gameover_distance;
    Vector3 enemy_position;
    bool chasing;
    bool in_same_map;

	// Use this for initialization
	void Start () {
        state_num = 0;
        gameover_distance = 1f;
        enemy_position = this.gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	}


    //test - 키보드로 아저씨 움직이기
    
    //아저씨 위치정보 반환
    Vector3 get_enemy_position(){
        return enemy_position;
    }

    //게임오버
    void game_over()
    {
    }

    //플레이어~아저씨 거리 계산
    float check_distance()
    {
        Vector3 player_pos = new Vector3();
        return Vector3.Distance(player_pos, enemy_position);
    }

    void check_same_map()
    {
    }
    //아저씨 상태 변경
    void change_state()
    {
    }

    void do_action(){

    }
}
