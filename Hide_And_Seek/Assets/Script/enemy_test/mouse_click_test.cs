using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_click_test : MonoBehaviour {
    public Camera Camera;
    Vector3 end_pos; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update()
    {
        //마우스 따라 가게 만들기
        end_pos = Camera.ScreenToWorldPoint(Input.mousePosition);
        end_pos.z = 0f;
        enemy_test.enemy.GetComponent<enemy_test>().make_enemy_move(end_pos);
	}
    void OnMouseDown()
    {
        /*
        //마우스 클릭시 해당 위치로 이동시키기
        end_pos = Camera.ScreenToWorldPoint(Input.mousePosition);
        end_pos.z = 0f;
        enemy_test.enemy.GetComponent<enemy_test>().make_enemy_move(end_pos);
        */
    }

}


