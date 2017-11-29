﻿using System.Collections;
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
    public Vector2 portal_loc1, portal_loc2;

    public Portal(int key_num, Room r1, Room r2)
    {
        this.key_num = key_num;
        scene_name1 = Scene_Manager.scene_name[(int)r1];
        scene_name2 = Scene_Manager.scene_name[(int)r2];
        switch (key_num)
        {
            case 1:
                portal_loc1 = new Vector2(-2.92f, -1.61f);
                portal_loc2 = new Vector2(-0.42f, -0.69f);
                break;
            case 2:
                portal_loc1 = new Vector2(-2.44f, -2.27f);
                portal_loc2 = new Vector2(-1.62f, -0.51f);
                break;
            case 3:
                portal_loc1 = new Vector2(-4.54f, -2.91f);
                portal_loc2 = new Vector2(3.42f, -1.22f);
                break;
            case 4:
                portal_loc1 = new Vector2(1.01f, -2.73f);
                portal_loc2 = new Vector2(3.18f, -1.8f);
                break;
            case 5:
                portal_loc1 = new Vector2(-5.16f, -1.93f);
                portal_loc2 = new Vector2(1.47f, -2.58f);
                break;
            case 6:
                portal_loc1 = new Vector2(2.33f, 1.22f);
                portal_loc2 = new Vector2(-1.03f, -2.23f);
                break;
            case 7:
                portal_loc1 = new Vector2(-0.38f, -2.5f);
                portal_loc2 = new Vector2(-2.72f, -0.36f);
                break;
            case 8:
                portal_loc1 = new Vector2(3.84f, 1.29f);
                portal_loc2 = new Vector2(0.23f, -2.82f);
                break;
            case 9:
                portal_loc1 = new Vector2(2.49f, 0.06f);
                portal_loc2 = new Vector2(-3.28f, -1.82f);
                break;
            case 10:
                portal_loc1 = new Vector2(-0.57f, -1.2f);
                portal_loc2 = new Vector2(3.08f, 0.01f);
                break;
            case 11:
                portal_loc1 = new Vector2(-2.5f, -0.7f);
                portal_loc2 = new Vector2(-0.33f, -2.3f);
                break;
            case 12:
                portal_loc1 = new Vector2(-2.34f, -1.57f);
                portal_loc2 = new Vector2(1.51f, 0.32f);
                break;
            case 13:
                portal_loc1 = new Vector2(-0.72f, -1.56f);
                portal_loc2 = new Vector2(1.62f, -0.96f);
                break;
            case 14:
                portal_loc1 = new Vector2(1.65f, -1.04f);
                portal_loc2 = new Vector2(-1.33f, -2.28f);
                break;
            case 15:
                portal_loc1 = new Vector2(-0.91f, -1.95f);
                portal_loc2 = new Vector2(0.18f, -0.62f);
                break;
            case 16:
                portal_loc1 = new Vector2(5.06f, -3.64f);
                portal_loc2 = new Vector2(2.92f, -1.01f);
                break;
            case 17:
                portal_loc1 = new Vector2(6.5f, -1.13f);
                portal_loc2 = new Vector2(-1.91f, -4.3f);
                break;
            case 18:
                portal_loc1 = new Vector2(1.47f, 1.26f);
                portal_loc2 = new Vector2(2.69f, -0.69f);
                break;
            case 19:
                portal_loc1 = new Vector2(-5.99f, 2.3f);
                portal_loc2 = new Vector2(3.89f, -0.92f);
                break;
            case 20:
                portal_loc1 = new Vector2(-0.7f, -0.99f);
                portal_loc2 = new Vector2(-0.41f, -0.11f);
                break;
            case 21:
                portal_loc1 = new Vector2(-7.76f, 2.16f);
                portal_loc2 = new Vector2(1.24f, -0.9f);
                break;
            case 22:
                portal_loc1 = new Vector2(0.55f, 0.9f);
                portal_loc2 = new Vector2(-2.43f, 0.56f);
                break;
        }
    }

    public void action()
    {
        if (SceneManager.GetActiveScene().name == scene_name1)
        {
            SceneManager.LoadScene(scene_name2);
            if (Player.Player_obj != null) Player.Player_obj.GetComponent<Player>().set_player_pos(new Vector3(portal_loc2.x, portal_loc2.y, 0f));
        }
        else
        {
            SceneManager.LoadScene(scene_name1);
            if (Player.Player_obj != null) Player.Player_obj.GetComponent<Player>().set_player_pos(new Vector3(portal_loc1.x, portal_loc1.y, 0f));
        }
        
    }
    public void for_start()
    {
    }

    public void for_update()
    {
    }

    public int get_key()
    {
        return key_num;
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
        //Debug.Log("Thing");
    }

    ///////////[오브젝트 앞뒤 정렬용 start, update함수]
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