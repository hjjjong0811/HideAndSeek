using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_test : MonoBehaviour {
    public static GameObject enemy;
    public static Vector3 enemy_pos;
    public float move_speed = 3f;
    public static bool moving;
    public static Vector3 destination;
    int scene_num, spot_num;

	// Use this for initialization
	void Start () {
        enemy = this.gameObject;
        moving = false;
        destination = enemy_pos;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        enemy_pos = enemy.transform.position;

        //key_board_move(Time.deltaTime);//test
        if (moving)
        {
            enemy_moving();
        }
	}
        
    /*
    //test용
    void key_board_move(float f)
    {
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //enemy.transform.position = new Vector3(enemy_pos.x, enemy_pos.y + move_speed, enemy_pos.z);
            enemy.transform.Translate(Vector3.up * f * move_speed);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //enemy.transform.position = new Vector3(enemy_pos.x, enemy_pos.y - move_speed, enemy_pos.z);
            enemy.transform.Translate(Vector3.down * f * move_speed);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            //enemy.transform.position = new Vector3(enemy_pos.x - move_speed, enemy_pos.y, enemy_pos.z);
            enemy.transform.Translate(Vector3.left * f * move_speed);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //enemy.transform.position = new Vector3(enemy_pos.x + move_speed, enemy_pos.y, enemy_pos.z);
            enemy.transform.Translate(Vector3.right * f * move_speed);
        }
    }
     */

    public void make_enemy_move(Vector3 v)
    {
        enemy_test.destination = v;
        enemy_test.moving = true;
    }

    void enemy_moving()
    {
        float distance = Vector3.Distance(destination, enemy_pos);
        if (distance < 0.1f) moving = false;
        enemy.transform.Translate((destination - enemy_pos) * Time.deltaTime / distance * move_speed);
    }

    void random_destination()
    {
    }

    //현재 몇층인지 반환
    int check_floor()
    {
        if (scene_num >= 0 && scene_num < 10) return 1;
        else if (scene_num >= 11 && scene_num < 20) return 2;
        else return 0;
    }

    void go_to_the_room()
    {

    }
}
