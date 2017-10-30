using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_test : MonoBehaviour {
    public static GameObject enemy;
    public static Vector3 enemy_pos;
    public static float move_speed = 0.05f;

	// Use this for initialization
	void Start () {
        enemy = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        enemy_pos = enemy.transform.position;

        key_board_move();
	}

    void key_board_move()
    {
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            enemy.transform.position = new Vector3(enemy_pos.x, enemy_pos.y + move_speed, enemy_pos.z);
            //enemy.GetComponent<Rigidbody2D>().MovePosition(transform.position + transform.forward * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            enemy.transform.position = new Vector3(enemy_pos.x, enemy_pos.y - move_speed, enemy_pos.z);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            enemy.transform.position = new Vector3(enemy_pos.x - move_speed, enemy_pos.y, enemy_pos.z);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            enemy.transform.position = new Vector3(enemy_pos.x + move_speed, enemy_pos.y, enemy_pos.z);
        }
    }
}
