using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Enemy_State { in_dest = 0, going_hall = 1, going_dest = 2, in_hall = 3, finding = 4 };//내부상태
public class Enemy : MonoBehaviour
{
    public static GameObject _enemy = null;
    private static readonly int CHASING_START_DISTANCE = 3;//같은방에서 Enemy~Player spot 거리차이가 이 변수값 이하면 enemy가 쫓아옴
    private static readonly int GAMEOVER_DISTANCE = 1;
    public static readonly Vector3 ENEMY_INIT_LOC = new Vector3(-100f, -100f, 0f); //enemy 활동안할때 안보이게 치워놓을 위치
    private static readonly float CHASING_MOVE_SCENE_TIME = 1f;//[chasing상태] 플레이어가 방이동할때 해당 시간후 포탈에서 튀어나옴
    private static readonly float[] _enemy_stay_time = new float[] { 3f, 1f, 2f, 0f, 1f };//[normal,chasing 상태] 내부상태에 따른 활동시간
    public float _enemy_speed = 1f;//[chasing 상태] 플레이어 쫓아가는 속도_(test : 일단 public -> 나중에 private static readonly)

    //[아저씨 상태 변수]
    private static bool _enemy_working;//아저씨 발동상태
    private static bool _f_normal_t_chasing;//normal 상태(false) 인지 chasing 상태(true) 인지 구분해줌
    private static ISpot _enemy_spot;//아저씨 위치
    private static ISpot _enemy_last_spot;//아저씨 이전 위치
    //[normal 상태]
    private static Enemy_State _enemy_state;//현재 내부상태
    //[chasing 상태]
    private static Room _enemy_dest;
    private static Route _enemy_route;
    private static bool _enemy_looking;//[normal상태] 둘러보기 세마포어용 변수
    //public static bool _enemy_taking_portal;////////////////test
    private static bool _enemy_finding;
    private static float _enemy_finding_time;
    private static Vector3 _enemy_pos;

    void Awake()
    {
        if (Enemy._enemy != null)
        {
            Destroy(this.gameObject);
        }
    }
    // Use this for initialization
    void Start()
    {
        _enemy = this.gameObject;
        _enemy_working = false;
        _f_normal_t_chasing = false;
        _enemy_spot = new ISpot(Room.Wine_0, 0);
        _enemy_last_spot = _enemy_spot;
        _enemy_state = Enemy_State.going_hall;
        _enemy_dest = Room.None;
        int tmp = 0;
        _enemy_route = Scene_Manager.getInstance().find_shortest(_enemy_spot, new ISpot(Room.Hall_1, 1), ref tmp, new List<ISpot>());
        _enemy_looking = false;
        _enemy_finding = false;
        _enemy_finding_time = 0f;
        _enemy.transform.position = ENEMY_INIT_LOC;
        _enemy_pos = ENEMY_INIT_LOC;

        DontDestroyOnLoad(this.gameObject);//test
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("enemy 위치(" + _enemy.transform.position.x + ")");//test

        //일단 오류만 안나게_ 겜매니저 진행도보고 조정중
        if (ScriptManager.getInstance().isPlaying || GameManager.getInstance().isScenePlay)
        {
            return;
        }
        if (GameObject.FindGameObjectWithTag("Player") == null) return;

        //test중
        //Debug.Log("enemy_working : " + _enemy_working);
        //Debug.Log("_f_normal_t_chasing : " + _f_normal_t_chasing);

        if (!_enemy_working) return;
        if (ScriptManager.getInstance().isPlaying) _enemy_working = false;

        _enemy_pos = _enemy.transform.position;
        
        //Enemy에서 볼륨조절하게 되면, 여기에 추가하기!!

        if (_f_normal_t_chasing)
        {
            do_chasing(Time.deltaTime);
        }
        else
        {
            do_normal();
        }
    }

    /// <summary>
    /// normal 상태면, chasing 상태로...
    /// chasing 상태면, normal 상태로...
    /// => 상태 전환해주는 함수!
    /// </summary>
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
                
                _enemy_finding_time = 0f;
                _enemy_looking = false;
                _enemy_finding = false;
                _f_normal_t_chasing = true;
                break;

            case true://chasing -> normal
                _enemy.transform.position = Enemy.ENEMY_INIT_LOC;
                _enemy_last_spot = _enemy_spot;
                _enemy_finding = false;

                int tmp_i = 0;
                if ((int)Player.get_player_spot()._room <= Scene_Manager.MAX_FLOOR1_IDX)//1층이면
                { _enemy_route = Scene_Manager.getInstance().find_shortest(_enemy_spot, new ISpot(Room.Hall_1, 1), ref tmp_i, new List<ISpot>())._next; }
                else if ((int)Player.get_player_spot()._room <= Scene_Manager.MAX_FLOOR2_IDX)//2층이면
                { _enemy_route = Scene_Manager.getInstance().find_shortest(_enemy_spot, new ISpot(Room.Hall_2, 0), ref tmp_i, new List<ISpot>())._next; }
                _enemy_state = Enemy_State.going_hall;
                _f_normal_t_chasing = false;
                break;
        }
    }


    /////////////////////////////////////////////do_normal
    void do_normal()
    {
        if (!_enemy_looking)
        {
            StartCoroutine(looking_around(_enemy_stay_time[(int)_enemy_state]));
        }
        //Debug.Log("do_normal!");//test

        //chasing 상태로 바꾸는 조건
        if (check_player_enemey_distance() <= CHASING_START_DISTANCE && check_in_same_room() && !Player.hiding)
        {
            change_state();
            //Debug.Log("change_state");//test
        }

        //소리나면 route, dest바꾸기
    }
    IEnumerator looking_around(float f)
    {
        _enemy_looking = true;//세마포어 설정
        yield return new WaitForSeconds(f);
        //Debug.Log("아저씨 상태 : " + _enemy_state);//test

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
                if ((int)Player.get_player_spot()._room <= Scene_Manager.MAX_FLOOR1_IDX)//1층이면
                { _enemy_route = Scene_Manager.getInstance().find_shortest(_enemy_spot, new ISpot(Room.Hall_1, 1), ref i, new List<ISpot>())._next; }
                else if ((int)Player.get_player_spot()._room <= Scene_Manager.MAX_FLOOR2_IDX)//2층이면
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
        if (check_player_enemey_distance() <= CHASING_START_DISTANCE && check_in_same_room() && !Player.hiding)
        {
            change_state();
        }
    }

    /////////////////////////////////////////////do_chasing
    void do_chasing(float spent_time)
    {
        //Debug.Log("chasing!"+_enemy_spot._room+" / 플레이어 : "+Player.get_player_spot()._room);//test
        if (check_in_same_room()) //chasing하고있고, player&enemy 같은방인 상태
        {
            _enemy_finding_time = 0f;
            //Debug.Log("플레이어 따라다니는중!");
            //플레이어 따라다니기
            Vector3 _player_pos = Player.Player_obj.transform.position;
            float distance = Vector3.Distance(_player_pos, _enemy_pos);
            if (distance < Enemy.GAMEOVER_DISTANCE) Debug.Log("게임오버");//test
            if (distance > 0.1f) _enemy.transform.Translate((_player_pos - _enemy_pos) * Time.deltaTime / distance * _enemy_speed);

            //가장 가까운 Spot확인하기  = 플레이어 쫓아다니면서 ISpot정보 갱신하기
            GameObject[] spots = GameObject.FindGameObjectsWithTag("Spot");
            float nearest_distance = 10000f;
            GameObject nearest_spot = null;
            for (int i = 0; i < spots.Length; i++)
            {
                if (Vector3.Distance(_enemy_pos, spots[i].transform.position) <= nearest_distance)
                {
                    nearest_distance = Vector3.Distance(_enemy_pos, spots[i].transform.position);
                    nearest_spot = spots[i];
                }
            }
            _enemy_last_spot = _enemy_spot;
            _enemy_spot = new ISpot(Scene_Manager.getInstance().get_room_info(SceneManager.GetActiveScene().name), int.Parse(nearest_spot.name));
        }
        else //chasing하고있는데, player&enemy 다른방인 상태
        {
            _enemy_finding_time += spent_time;
            if (_enemy_finding_time > 10f) //10초이상 플레이어 안보이면, normal상태로 돌아감
            {
                change_state();
                return;
            }

            if (!_enemy_finding)
            {
                //1초 후, 다음포탈타기
                StartCoroutine(finding_player());
            }
        }

    }

    static IEnumerator finding_player()
    {
        _enemy_finding = true;

        //Debug.Log("포탈앞에서 1초기다리기");
        yield return new WaitForSeconds(_enemy_stay_time[(int)Enemy_State.finding]);
        //플레이어 스크립트 생성중인데 포탈타서 나타나야하는경우_ 잠시 기다리기

        yield return new WaitWhile(() => ScriptManager.getInstance().isPlaying || GameManager.getInstance().isScenePlay);

        _enemy_finding_time += _enemy_stay_time[(int)Enemy_State.finding];
        _enemy_spot._room = Player.get_player_spot()._room;
        //Debug.Log("아저씨 위치 이동");
        _enemy.transform.position = Scene_Manager.getInstance()._get_portal_loc(Player.Player_Last_Portal_num, Player.get_player_spot()._room);

        if (_enemy_finding_time > 10f)
        {
            change_state();
        }
        _enemy_finding = false;
    }

    /// <summary>
    /// 플레이어랑 적이랑 같은 방인지 체크하는 함수
    /// </summary>
    /// <returns></returns>
    public static bool check_in_same_room()
    {
        if (Player.get_player_spot()._room == _enemy_spot._room) return true;
        else return false;
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

    public static Route look_around_route(ISpot start, Room end)
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
    public static bool get_enemy_working()
    {
        return _enemy_working;
    }

    public static int check_player_enemey_distance()
    {
        int distance = 100;//-1은 Enemey 활동하지 않는중을 의미
        Scene_Manager tmp = Scene_Manager.getInstance();
        if (Player.Player_obj != null && Enemy._enemy_working)
        {
            ISpot tmp_ispot = Player.get_player_spot();

            //Enemy 위치 enemy_ispot에 가져오기
            ISpot enemy_ispot = Enemy.get_enemy_spot();

            //Player, Enemy 각각의 위치정보로 거리 계산하기 => "distance변수"에 저장됨
            tmp.find_shortest(enemy_ispot, tmp_ispot, ref distance, new List<ISpot>());
        }
        return distance;
    }

    public static void player_start_hiding(ISpot player_hiding_spot)
    {
        if (_f_normal_t_chasing)
        {
            if (check_in_same_room() && check_player_enemey_distance() < CHASING_START_DISTANCE)//아저씨랑 같은방에서 가까울때 숨으면
            {
                Debug.Log("게임오버");//test
            }
            else
            {
                go_straight(player_hiding_spot);
            }
        }
    }

    public static void go_straight(ISpot spot)
    {
        _enemy_dest = spot._room;
        _f_normal_t_chasing = false;
        _enemy_route = look_around_route(_enemy_spot, _enemy_dest)._next;
        _enemy_state = Enemy_State.going_dest;
        _enemy_looking = false;
        _enemy_finding = false;
    }

    public static void start_enemy_working()
    {
        if (_enemy == null)
        {
            Debug.Log("일어날 수 없는 오류!");
            return;
        }
        if (!_enemy_working)
        {
            //Debug.Log("이미 Enemy working하는 중임!!!");//test
        }
       
        _enemy.transform.position = _enemy_pos;
        _enemy_working = true;
    }
    public static void end_enemy_working()
    {
        Debug.Log("end_enemy_working()");//test
        if (!_enemy_working)
        {
            //Debug.Log("이미 Enemy working 안하는 중임!!!");//test
        }
        _enemy_working = false;
        _enemy.transform.position = ENEMY_INIT_LOC;
    }


    /// <summary>
    /// 세이브파일 데이터 가져오는 함수
    /// </summary>
    /// <param name="result"></param>
    public static void enemy_bring_data(Enemy_Data result){

        //Enemy._enemy = this.gameObject;
        Enemy._enemy_working = result._enemy_working;
        Enemy._f_normal_t_chasing = false;
        Enemy._enemy_spot = result._enemy_spot;
        Enemy._enemy_last_spot = Enemy._enemy_spot;
        Enemy._enemy_state = result._enemy_state;
        Enemy._enemy_dest = result._enemy_dest;
        //Enemy._enemy_route = result._enemy_route;
        Enemy._enemy_looking = false;
        Enemy._enemy_finding = false;
        Enemy._enemy_finding_time = 0f;
        Enemy._enemy_pos = Enemy.ENEMY_INIT_LOC;

        Enemy._enemy_route = null;
        if(result._enemy_route_length==0){
            return;
        }
        else if(result._enemy_route_length==1){
            Enemy._enemy_route = new Route(result._enemy_route_array[0]);
        }
        else
        {
            Route[] tmp_route = new Route[result._enemy_route_length];
            for (int i = 0; i < result._enemy_route_length; i++)
            {
                tmp_route[i] = new Route(result._enemy_route_array[i]);
            }
            for (int i = 1; i < tmp_route.Length; i++)
            {
                tmp_route[i - 1]._next = tmp_route[i];
            }
            Enemy._enemy_route = tmp_route[0];
        }
    }
    /// <summary>
    /// 세이브파일 저장용 데이터 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public static Enemy_Data enemy_save_data()
    {
        Enemy_Data result = new Enemy_Data();

        result._enemy_working = Enemy._enemy_working;
        result._enemy_spot = Enemy._enemy_spot;
        result._enemy_state = Enemy._enemy_state;
        result._enemy_dest = Enemy._enemy_dest;
        //result._enemy_route = Enemy._enemy_route;
        while (_enemy_route != null)
        {
            result._enemy_route_array[result._enemy_route_length++] = _enemy_route.get_data();
            _enemy_route = _enemy_route._next;
        }
        return result;
    }
}

public class Enemy_Data
{
    public bool _enemy_working;//아저씨 발동상태
    public ISpot _enemy_spot;//아저씨 위치
    public Enemy_State _enemy_state;//현재 내부상태
    public Room _enemy_dest;
    //public Route _enemy_route;
    public int _enemy_route_length = 0;//루트 길이(_enemy_route_array 배열 길이)
    public ISpot[] _enemy_route_array = new ISpot[100];//루트 순서대로 ISpot데이터만 배열로 저장
}
