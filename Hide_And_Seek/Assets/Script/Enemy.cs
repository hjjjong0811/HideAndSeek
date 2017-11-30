using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static GameObject _enemy;
    private static readonly int CHASING_START_DISTANCE = 3;//같은방에서 Enemy~Player spot 거리차이가 이 변수값 이하면 enemy가 쫓아옴
    public static readonly Vector3 ENEMY_INIT_LOC = new Vector3(-100f, -100f, 0f); //enemy 활동안할때 안보이게 치워놓을 위치
    enum Enemy_State { in_dest, going_hall, going_dest, in_hall };//[normal 상태] 내부상태
    private static readonly float[] _enemy_stay_time = new float[] { 3f, 1f, 2f, 0f };//[normal 상태] 내부상태에 따른 활동시간
    public float _enemy_speed = 1f;//[chasing 상태] 플레이어 쫓아가는 속도_(test : 일단 public -> 나중에 private static readonly)

    //[아저씨 상태 변수]
    public static bool _enemy_working;//아저씨 발동상태
    private static bool _f_normal_t_chasing;//normal 상태(false) 인지 chasing 상태(true) 인지 구분해줌
    private static ISpot _enemy_spot;//아저씨 위치
    private static ISpot _enemy_last_spot;//아저씨 이전 위치
    //[normal 상태]
    private static Enemy_State _enemy_state;//현재 내부상태
    //[chasing 상태]
    private static Room _enemy_dest;
    private static Route _enemy_route;
    private static bool _enemy_looking;//[normal상태] 둘러보기 세마포어용 변수
    Vector3 _enemy_pos;
    Vector3 _player_pos;

    // Use this for initialization
    void Start()
    {
        _enemy = this.gameObject;
        //test
        _enemy_working = true;
        _f_normal_t_chasing = false;
        //_enemy_spot = new ISpot(Room.Wine_0, 0);
        _enemy_spot = new ISpot(Room.Hall_1, 1);//test
        //_enemy_last_spot = new ISpot(Room.Wine_0, 0);
        _enemy_last_spot = new ISpot(Room.Hall_1, 1);//test
        _enemy_state = Enemy_State.going_hall;
        _enemy_dest = Room.None;
        _enemy_route = null;
        _enemy_looking = false;
        _enemy.transform.position = ENEMY_INIT_LOC;

        DontDestroyOnLoad(this.gameObject);//test
    }

    // Update is called once per frame
    void Update()
    {
        if (!_enemy_working) return;

        _enemy_pos = _enemy.transform.position;

        if (_f_normal_t_chasing)
        {
            do_chasing();
        }
        else
        {
            do_normal();
        }
    }

    static void change_state()
    {
        switch (_f_normal_t_chasing)
        {
            case false://normal -> chasing
                //i) 아저씨가 같은방에서부터 chasing상태되는 경우 -> spot위치에서부터 쫓아다님
                if (_enemy_spot._room == _enemy_last_spot._room)
                {
                    GameObject spot = null;
                    GameObject[] spot_objects = GameObject.FindGameObjectsWithTag("Spot");
                    for (int i = 0; i < spot_objects.Length; i++)
                    {
                        if (int.Parse(spot_objects[i].name) == _enemy_spot._spot)
                        {
                            spot = spot_objects[i];
                            break;
                        }
                    }
                    _enemy.transform.position = spot.transform.position;
                }
                //ii) 아저씨가 포탈타고와서 chasing상태되는 경우 -> portal위치에서부터 쫓아다님
                else
                {
                    _enemy.transform.position = Scene_Manager.getInstance()._get_portal_loc(_enemy_last_spot._room, _enemy_spot._room);
                }
                _f_normal_t_chasing = true;
                break;

            case true://chasing -> normal
                _f_normal_t_chasing = false;
                break;
        }
    }
    /// <summary>
    /// 1. 플레이어 찾는 상태
    ///  -> 플레이어와 같은 방에 있고, 플레이어와 spot차이값이 CHASING_START_DISTANCE 이하면, chasing상태로 바뀜
    ///  -> 찾는동안에는 활동시간 후, 다음 spot으로 이동
    /// </summary>
    void do_normal()
    {
        if (!_enemy_looking)
        {
            //Debug.Log("아저씨 상태 : " + _enemy_state);//test
            StartCoroutine(looking_around(_enemy_stay_time[(int)_enemy_state]));
        }

        //chasing 상태로 바꾸는 조건
        if (check_player_enemey_distance() <= CHASING_START_DISTANCE && _enemy_spot._room == Player.get_player_spot()._room)
        {
            change_state();
        }

        //소리나면 route, dest바꾸기
    }
    IEnumerator looking_around(float f)
    {
        _enemy_looking = true;//세마포어 설정
        yield return new WaitForSeconds(f);

        //1. in_hall
        if (_enemy_state == Enemy_State.in_hall)
        {
            _enemy_dest = random_destination();
            _enemy_route = look_around_route(_enemy_spot, _enemy_dest)._next;
            _enemy_state = Enemy_State.going_dest;
            //Debug.Log(">>>" + "랜덤목적지 : " + _enemy_dest);//test
        }
        //2. going_dest
        else if (_enemy_state == Enemy_State.going_dest)
        {
            if (_enemy_spot._room != _enemy_dest)
            {
                move_next_route();
            }
            if (_enemy_spot._room == _enemy_dest)
            {
                _enemy_state = Enemy_State.in_dest;
                //Debug.Log(">>>" + "목적지 도착");//test
            }
        }
        //3. in_dest
        else if (_enemy_state == Enemy_State.in_dest)
        {
            //Debug.Log(">>>" + "목적지 둘러보기");//test
            if (_enemy_route == null)
            {
                int i = 0;
                if ((int)_enemy_spot._room <= Scene_Manager.MAX_FLOOR1_IDX)//1층이면
                { _enemy_route = Scene_Manager.getInstance().find_shortest(_enemy_spot, new ISpot(Room.Hall_1, 1), ref i, new List<ISpot>())._next; }
                else if ((int)_enemy_spot._room <= Scene_Manager.MAX_FLOOR2_IDX)//2층이면
                { _enemy_route = Scene_Manager.getInstance().find_shortest(_enemy_spot, new ISpot(Room.Hall_2, 0), ref i, new List<ISpot>())._next; }
                _enemy_state = Enemy_State.going_hall;
                //Debug.Log(">>>" + "목적지 다 둘러봄");//test
            }
            else
            {
                move_next_route();
            }
        }
        //4. going_hall
        else if (_enemy_state == Enemy_State.going_hall)
        {
            if (_enemy_spot._room != Room.Hall_1 && _enemy_spot._room != Room.Hall_2)
            {
                move_next_route();
            }
            if (_enemy_spot._room == Room.Hall_1 || _enemy_spot._room == Room.Hall_2)
            {
                _enemy_route = null;
                _enemy_state = Enemy_State.in_hall;
                //Debug.Log(">>>" + "복도 도착");//test
            }
        }

        _enemy_looking = false;//세마포어 해제
    }
    static void move_next_route()
    {
        _enemy_last_spot = _enemy_spot;//이전 위치 정보 저장
        _enemy_spot = _enemy_route.get_data();
        _enemy_route = _enemy_route._next;
        Debug.Log(">>>" + "이동 : " + _enemy_spot._room + "/" + _enemy_spot._spot);//test
        if (check_player_enemey_distance() <= CHASING_START_DISTANCE && _enemy_spot._room == Player.get_player_spot()._room)
        {
            change_state();
        }
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
        if ((int)Player.get_player_spot()._room <= Scene_Manager.MAX_FLOOR1_IDX)//1층이면
        {
            dest_room = first_floor[Random.Range(0, first_floor.Length)]; //0~9
        }
        else if ((int)Player.get_player_spot()._room <= Scene_Manager.MAX_FLOOR2_IDX)//2층이면
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
        return _f_normal_t_chasing;
    }

    public static int check_player_enemey_distance(){
        int distance = -1;//-1은 Enemey 활동하지 않는중을 의미
        Scene_Manager tmp = Scene_Manager.getInstance();
        if (Player.Player_obj != null && Enemy._enemy_working)
        {
            //Player 위치 tmp_ispot에 가져오기
            /*
            ISpot tmp_ispot = new ISpot(Room.None, -1);
            float tmp_float = 0f;
            Vector3 tmp_vector = new Vector3(0f, 0f, 0f);
            Player.getPlayerData(ref tmp_float, ref tmp_vector, ref tmp_ispot);
            */
            ISpot tmp_ispot = Player.get_player_spot();

            //Enemy 위치 enemy_ispot에 가져오기
            ISpot enemy_ispot = Enemy.get_enemy_spot();

            //Player, Enemy 각각의 위치정보로 거리 계산하기 => "distance변수"에 저장됨
            tmp.find_shortest(enemy_ispot, tmp_ispot, ref distance, new List<ISpot>());
        }
        return distance;
    }
}
