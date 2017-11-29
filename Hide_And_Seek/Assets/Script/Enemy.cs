using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static GameObject _enemy;

    //normal 상태(false) 인지 chasing 상태(true) 인지 구분해줌
    private static bool _enemy_chasing;

    //normal
    enum Enemy_State { in_dest, going_hall, going_dest, in_hall };
    private static float[] _enemy_stay_time = new float[] { 3f, 1f, 2f, 0f };
    private static Enemy_State _enemy_state;
    private static ISpot _enemy_spot;
    private static Room _enemy_dest;
    private static Route _enemy_route;
    private static bool _enemy_looking;

    //chasing
    Vector3 _enemy_pos;
    Vector3 _player_pos;
    float _enemy_speed = 3f;

    // Use this for initialization
    void Start()
    {
        _enemy = this.gameObject;
        _enemy_spot = new ISpot(Room.Hall_1, 1);
        _enemy_chasing = true;//test
        _enemy_state = Enemy_State.in_hall;
        _enemy_route = null;

        //normal
        _enemy_looking = false;
    }

    // Update is called once per frame
    void Update()
    {
        _enemy_pos = _enemy.transform.position;

        if (_enemy_chasing)
        {
            do_chasing();
        }
        else
        {
            do_normal();
        }
    }

    /////////////////////////////////////////////do_normal
    //do_normal : 해당상태에 따른 시간동안 기다리고, 다음장소로 이동
    void do_normal()
    {
        if (!_enemy_looking)
        {
            Debug.Log("아저씨 상태 : " + _enemy_state);//test
            StartCoroutine(looking_around(_enemy_stay_time[(int)_enemy_state]));
        }
    }


    IEnumerator looking_around(float f)
    {
        _enemy_looking = true;
        yield return new WaitForSeconds(f);
        Debug.Log((int)f + "초 기다림");//test

        if (_enemy_state == Enemy_State.in_hall)
        {
            _enemy_dest = random_destination();
            _enemy_route = look_around_route(_enemy_spot, _enemy_dest)._next;
            _enemy_state = Enemy_State.going_dest;
            Debug.Log(">>>" + "랜덤목적지 : " + _enemy_dest);//test
        }
        else if (_enemy_state == Enemy_State.going_dest)
        {
            if (_enemy_spot._room != _enemy_dest)
            {
                move_next_route();
                Debug.Log(">>>" + "이동 : " + _enemy_spot._room + "/" + _enemy_spot._spot);//test
            }
            if (_enemy_spot._room == _enemy_dest)
            {
                _enemy_state = Enemy_State.in_dest;
                Debug.Log(">>>" + "목적지 도착");//test
            }
        }
        else if (_enemy_state == Enemy_State.in_dest)
        {
            Debug.Log(">>>" + "목적지 둘러보기");//test
            if (_enemy_route == null)
            {
                int i = 0;
                if ((int)_enemy_spot._room <= Scene_Manager.MAX_FLOOR1_IDX)//1층이면
                { _enemy_route = Scene_Manager.getInstance().find_shortest(_enemy_spot, new ISpot(Room.Hall_1, 1), ref i, new List<ISpot>())._next; }
                else if ((int)_enemy_spot._room <= Scene_Manager.MAX_FLOOR2_IDX)//2층이면
                { _enemy_route = Scene_Manager.getInstance().find_shortest(_enemy_spot, new ISpot(Room.Hall_2, 0), ref i, new List<ISpot>())._next; }
                _enemy_state = Enemy_State.going_hall;
                Debug.Log(">>>" + "목적지 다 둘러봄");//test
            }
            else
            {
                move_next_route();
                Debug.Log(">>>" + "이동 : " + _enemy_spot._room + "/" + _enemy_spot._spot);//test
            }
        }
        else if (_enemy_state == Enemy_State.going_hall)
        {
            if (_enemy_spot._room != Room.Hall_1 && _enemy_spot._room != Room.Hall_2)
            {
                move_next_route();
                Debug.Log(">>>" + "이동 : " + _enemy_spot._room + "/" + _enemy_spot._spot);//test
            }
            if (_enemy_spot._room == Room.Hall_1 || _enemy_spot._room == Room.Hall_2)
            {
                _enemy_route = null;
                _enemy_state = Enemy_State.in_hall;
                Debug.Log(">>>" + "복도 도착");//test
            }
        }

        _enemy_looking = false;
    }

    //looking_around에서 자주 사용하는 코드 함수로 만들어놓은것
    static void move_next_route()
    {
        _enemy_spot = _enemy_route.get_data();
        _enemy_route = _enemy_route._next;
    }

    /////////////////////////////////////////////do_chasing
    void do_chasing()
    {
        //플레이어 따라다니기
        _player_pos = Player.Player_obj.GetComponent<Player>().get_player_pos();
        float distance = Vector3.Distance(_player_pos, _enemy_pos);
        if (distance > 0.1f) _enemy.transform.Translate((_player_pos - _enemy_pos) * Time.deltaTime / distance * _enemy_speed);
    }

    private static Room random_destination()
    {
        Room[] first_floor = new Room[] { Room.Wine_0, Room.Bath_1, Room.Dining_1, Room.Front_1, Room.Kitchen_1, Room.Laundry_1, Room.Living_1, Room.Pantry_1, Room.Reception_1 };
        Room[] second_floor = new Room[] { Room.Baby_2, Room.Bath_2, Room.Bed_2, Room.Dress_2, Room.Empty_2, Room.Library_2, Room.Photo_2, Room.Swimming_2, Room.Garret_3 };

        Room dest_room = Room.Hall_1;
        if ((int)_enemy_spot._room <= Scene_Manager.MAX_FLOOR1_IDX)//1층이면
        {
            dest_room = first_floor[Random.Range(0, first_floor.Length)]; //0~9
        }
        else if ((int)_enemy_spot._room <= Scene_Manager.MAX_FLOOR2_IDX)//2층이면
        {
            dest_room = second_floor[Random.Range(0, second_floor.Length)];
        }
        return dest_room;
    }

    private static Route look_around_route(ISpot start, Room end)
    {
        Route _route = null;
        int _distance = 0;
        switch (end)
        {
            case Room.Wine_0:
            case Room.Bath_1:
            case Room.Front_1:
            case Room.Laundry_1:
            case Room.Dress_2://
            case Room.Empty_2:
            case Room.Photo_2:
            case Room.Swimming_2:
            case Room.Garret_3:
                _route = Scene_Manager.getInstance().find_shortest(start, new ISpot(end, 0), ref _distance, new List<ISpot>());
                break;
            case Room.Dining_1:
            case Room.Pantry_1:
            case Room.Reception_1:
            case Room.Baby_2:
            case Room.Bath_2://
            case Room.Bed_2://
                _route = Scene_Manager.getInstance().find_shortest(start, new ISpot(end, 1), ref _distance, new List<ISpot>());
                break;
            case Room.Kitchen_1:
            case Room.Living_1://
            case Room.Library_2://
                _route = Scene_Manager.getInstance().find_shortest(start, new ISpot(end, 2), ref _distance, new List<ISpot>());
                break;
        }
        Route r1 = null;
        if (end == Room.Living_1)
        {
            r1 = new Route(new ISpot(Room.Living_1, 1));
            Route r2 = new Route(new ISpot(Room.Living_1, 0));
            r1._next = r2;
        }
        else if (end == Room.Bath_2)
        {
            r1 = new Route(new ISpot(Room.Bath_2, 0));
        }
        else if (end == Room.Bed_2)
        {
            r1 = new Route(new ISpot(Room.Bed_2, 0));
            Route r2 = new Route(new ISpot(Room.Bed_2, 1));
            Route r3 = new Route(new ISpot(Room.Bed_2, 2));
            Route r4 = new Route(new ISpot(Room.Bed_2, 1));
            r1._next = r2;
            r2._next = r3;
            r3._next = r4;
        }
        else if (end == Room.Dress_2)
        {
            r1 = new Route(new ISpot(Room.Dress_2, 1));
        }
        else if (end == Room.Library_2)
        {
            r1 = new Route(new ISpot(Room.Library_2, 1));
            Route r2 = new Route(new ISpot(Room.Library_2, 0));
            r1._next = r2;
        }
        Route.add_route(_route, r1);
        return _route;
    }


    // 다른 스크립트용 함수들
    public static ISpot get_enemy_spot()
    {
        return _enemy_spot;
    }
    public static bool get_enemy_chasing()
    {
        return _enemy_chasing;
    }
}
