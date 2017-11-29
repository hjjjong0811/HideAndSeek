using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    Scene_Manager sc;

    // Use this for initialization
    void Start()
    {
        sc = Scene_Manager.getInstance();


    }

    // Update is called once per frame
    void Update()
    {
    }

    void testing()
    {
        //test
        /*
        Map_Graph map = Scene_Manager.getInstance().getMap();
        foreach(var v in sm.getMap().spots){//전체 스폿 리스트
            Debug.Log(v._room + ":" + v.near_spots.Count);
        }
        //Debug.Log(map.find_spot(new ISpot(Room.Baby_2,0)).near_spots.Count);
        */
        /*
        Route r1 = new Route(Room.Hall_2,6);
        Route r2 = new Route(Room.Swimming_2, 2);
        Route r3 = new Route(Room.Swimming_2, 1);
        Route r4 = new Route(Room.Swimming_2, 0);
        r1._next = r2;
        r2._next = r3;
        r3._next = r4;
        //Debug.Log(Route.check_spot_in_route(r1, new ISpot(Room.Hall_2, 6)));
        //Debug.Log(Route.check_spot_in_route(r1, new ISpot(Room.Swimming_2, 2)));
        //Debug.Log(Route.check_spot_in_route(r1, new ISpot(Room.Swimming_2, 1)));
        //Debug.Log(Route.check_spot_in_route(r1, new ISpot(Room.Swimming_2, 0)));
        //Debug.Log(Route.check_spot_in_route(r1, new ISpot(Room.Hall_1, 2)));
        Debug.Log(Route.get_route_length(r1));
        /////////////////////
        List<ISpot> list = new List<ISpot>();
        List<ISpot> list2 = new List<ISpot>(list);
        list.Add(new ISpot(Room.Living_1, 1));
        Debug.Log(list2.Count);
        Debug.Log(list.Count);
        
        int distance = 0;
        Route r = sc.find_shortest(new ISpot(Room.Wine_0, 0), new ISpot(Room.Dress_2, 0), ref distance, new List<ISpot>());
        while (r != null)
        {
            Debug.Log(r.get_data()._room + " / " + r.get_data()._spot);//test
            r = r._next;
        }
         
        //플레이어~적 거리 계산용
        if (Player.Player_obj != null && Enemy._enemy_working)
        {
            //Player 위치 tmp_ispot에 가져오기
            ISpot tmp_ispot = new ISpot(Room.None, -1);
            float tmp_float = 0f;
            Vector3 tmp_vector = new Vector3(0f, 0f, 0f);
            Player.getPlayerData(ref tmp_float, ref tmp_vector, ref tmp_ispot);

            //Enemy 위치 enemy_ispot에 가져오기
            ISpot enemy_ispot = Enemy.get_enemy_spot();

            //Player, Enemy 각각의 위치정보로 거리 계산하기 => "distance변수"에 저장됨
            int distance = 0;
            sc.find_shortest(enemy_ispot, tmp_ispot, ref distance, new List<ISpot>());

            //콘솔 test용(지워도됨)
            //Debug.Log("enemy(" + enemy_ispot._room + "/" + enemy_ispot._spot + ") ~ player(" + tmp_ispot._room + "/" + tmp_ispot._spot + ") : " + distance);
        }
         */
    }
}
