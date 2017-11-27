using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    void action();
    void for_start();
    void for_update();
}
public class Portal : IObject
{
    int key_num=0;
    string scene_name1, scene_name2;

    public Portal(int key_num, Room r1, Room r2)
    {
        this.key_num = key_num;
        scene_name1 = Scene_Manager.scene_name[(int)r1];
        scene_name2 = Scene_Manager.scene_name[(int)r2];
    }

    public void action()
    {
        if (SceneManager.GetActiveScene().name == scene_name1)
        {
            SceneManager.LoadScene(scene_name2);
        }
        else
        {
            SceneManager.LoadScene(scene_name1);
        }
    }
    public void for_start()
    {
    }

    public void for_update()
    {
        testing();//test
    }

    public int get_key()
    {
        return key_num;
    }

    void testing()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (SceneManager.GetActiveScene().name == scene_name1)
            {
                SceneManager.LoadScene(scene_name2);
            }
            else
            {
                SceneManager.LoadScene(scene_name1);
            }
        }
    }
}
public class Thing : IObject
{
    GameObject obj;
    int key_num=0;
    static string[] object_sorting = new string[] { "Object_back", "Object_front" };
    List<SpriteRenderer> sr_list = new List<SpriteRenderer>();

    public Thing(int key_num, GameObject obj)
    {
        this.key_num = key_num;
        this.obj = obj;
    }

    //player에서 호출하는 함수
    public void action(){
        Debug.Log("Thing");
    }
    //start()용 함수
    public void for_start()
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
    //update()용 함수
    public void for_update()
    {
        change_childs();
    }



    void change_childs()
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
}