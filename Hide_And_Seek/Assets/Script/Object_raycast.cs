using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_raycast : MonoBehaviour
{
    static string[] object_sorting = new string[] { "Object_back", "Object_front" };
    List<SpriteRenderer> sr_list = new List<SpriteRenderer>();

	// Use this for initialization
	void Start () {
        Transform[] child_objects = this.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform t in child_objects)
        {
            if (t.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                sr_list.Add(t.gameObject.GetComponent<SpriteRenderer>());
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(check_player_exist());//test
        if(!check_player_exist()) return;

        change_childs();
	}

    void change_childs()
    {
        Object_State obj_loc = Player.check_up_down(this.gameObject.name);

        if (obj_loc == Object_State.too_far) return;
        change_childs_back_fround(object_sorting[(int)obj_loc]);
    }
    void change_childs_back_fround(string order)
    {
        foreach (SpriteRenderer sr in sr_list)
        {
            sr.sortingLayerName = order;
        }
    }

    bool check_player_exist()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            //Debug.Log("플레이어 없음");//test
            return false;
        }
        else return true;
    }
}
