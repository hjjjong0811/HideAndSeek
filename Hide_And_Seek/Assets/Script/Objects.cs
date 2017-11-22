using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Object_State
{
    too_far = -1,
    Object_back = 0,
    Object_front = 1
}
public class Objects : MonoBehaviour
{
    string[] object_sorting = new string[] { "Object_back", "Object_front" };
    List<SpriteRenderer> sr_list = new List<SpriteRenderer>();

    // Use this for initialization
    void Start()
    {
        Transform[] child_objects = GetComponentsInChildren<Transform>();
        foreach (Transform t in child_objects)
        {
            if (t.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                sr_list.Add(t.gameObject.GetComponent<SpriteRenderer>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        check_back_front();
    }

    void change_childs_back_fround(string order)
    {
        foreach (SpriteRenderer sr in sr_list)
        {
            sr.sortingLayerName = order;
        }
    }

    void check_back_front()
    {
        Object_State obj_loc = Player.check_up_down(this.gameObject.name);

        if (obj_loc == Object_State.too_far) return;
        change_childs_back_fround(object_sorting[(int)obj_loc]);
    }
}
