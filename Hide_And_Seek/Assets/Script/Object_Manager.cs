using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Object_State
{
    too_far = -1,
    Object_back = 0,
    Object_front = 1
}
public class Object_Manager{

	
}

public interface IObject
{
    void for_start();
    void for_update();
}
public class Portal : IObject
{
    int key_num;
    Room destination = Room.Living_1;

    public Portal(int key_num)
    {
        this.key_num = key_num;
    }
}
public class Object : IObject
{
    GameObject obj;
    int key_num = 1;
    static string[] object_sorting = new string[] { "Object_back", "Object_front" };
    List<SpriteRenderer> sr_list = new List<SpriteRenderer>();

    public Object(int key_num, GameObject obj)
    {
        this.key_num = key_num;
        this.obj = obj;
    }

    //update()용 함수
    void for_update()
    {
        Object_State obj_loc = Player.check_up_down(obj.name);

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

    //start()용 함수
    void for_start()
    {
        Transform[] child_objects = obj.GetComponentsInChildren<Transform>();
        foreach (Transform t in child_objects)
        {
            if (t.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                sr_list.Add(t.gameObject.GetComponent<SpriteRenderer>());
            }
        }
    }
}