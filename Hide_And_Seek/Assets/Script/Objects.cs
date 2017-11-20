using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    string[] object_sorting = new string[] { "Object_back", "Object_front" };
    GameObject[] child_objects = new GameObject[] { };
    int child_objects_num;

    // Use this for initialization
    void Start()
    {
        child_objects_num = 0;
    }

    // Update is called once per frame
    void Update()
    {
        check_back_front();
    }

    void check_back_front()
    {
        if (this.transform.position.y > Player.Player_obj.GetComponent<Player>().get_player_pos().y)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = object_sorting[0];
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = object_sorting[1];
        }
    }
}
