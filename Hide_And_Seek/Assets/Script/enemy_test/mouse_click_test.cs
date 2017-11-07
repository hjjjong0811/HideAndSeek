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
	void Update () {
        if (enemy_test.moving)
        {
            Vector3 start_pos = enemy_test.enemy_pos;
            if (start_pos == end_pos) enemy_test.moving = false;
            //enemy_test.enemy.transform.position = Vector3.Lerp(start_pos, end_pos, 0.05f);
            //enemy_test.enemy.transform.Translate(end_pos*0.02f);
            enemy_test.enemy.GetComponent<Rigidbody2D>().MovePosition(end_pos/Vector3.Distance(start_pos,end_pos));
        }
	}
    void OnMouseDown()
    {
        //enemy_test.enemy.transform.Translate(Camera.ScreenToWorldPoint(Input.mousePosition));
        end_pos = Camera.ScreenToWorldPoint(Input.mousePosition);
        end_pos.z = 0f;
        enemy_test.destination = end_pos;
        enemy_test.moving = true;
    }
    /*
    IEnumerator move()
    {
        for(int i=0;i<1000;i++){
            yield return new WaitForSeconds(0.1f);
            Vector3 start_pos = enemy_test.enemy_pos;
            Vector3 end_pos = Camera.ScreenToWorldPoint(Input.mousePosition);
            end_pos.z = 0f;
            if (start_pos == end_pos) break;
            enemy_test.enemy.transform.position = Vector3.Lerp(start_pos, end_pos, 0.05f);
        }
    }*/
}


