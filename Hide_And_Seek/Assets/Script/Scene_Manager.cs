using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Room
{
    None = -1,
    Wine_0 = 0,
    Bath_1 = 1,
    Dining_1 = 2,
    Front_1 = 3,
    Hall_1 = 4,
    Kitchen_1 = 5,
    Laundry_1 = 6,
    Living_1 = 7,
    Pantry_1 = 8,
    Reception_1 = 9,
    Baby_2 = 10,
    Bath_2 = 11,
    Bed_2 = 12,
    Dress_2 = 13,
    Empty_2 = 14,
    Hall_2 = 15,
    Library_2 = 16,
    Photo_2 = 17,
    Swimming_2 = 18,
    Garret_3 = 19
};

public class Scene_Manager
{
    public static int MAX_ROUTE_LENGTH = 100;//가장 긴 최단거리의 길이 (ex
    public static int MAX_FLOOR1_IDX = 9, MAX_FLOOR2_IDX = 19;
    public static string[] scene_name = new string[]{
    "0_Wine",
    "1_Bath",
    "1_Dining",
    "1_Front",
    "1_Hall",
    "1_Kitchen",
    "1_Laundry",
    "1_Living",
    "1_Pantry",
    "1_Reception",
    "2_Baby",
    "2_Bath",
    "2_Bed",
    "2_Dress",
    "2_Empty",
    "2_Hall",
    "2_Library",
    "2_Photo",
    "2_Swimming",
    "3_Garret"
    };
    int[] spot_num = new int[] { 2, 2, 4, 1, 3, 3, 2, 3, 2, 2, 2, 2, 3, 2, 1, 7, 3, 1, 3, 2 };//맵당 spot갯수
    Map_Graph _map_graph;
    private static Scene_Manager instance = null;
    private static List<Portal> _portal_list = new List<Portal>();

    private Scene_Manager()
    {
        _map_graph = new Map_Graph();
        for (int i = 0; i < spot_num.Length; i++)
        {
            for (int j = 0; j < spot_num[i]; j++)
            {
                _map_graph.spots.Add(new Spot((Room)i, j));
            }
        }
        //방 안의 spot 이어주기
        _map_graph.spot_link(new ISpot(Room.Wine_0, 0), new ISpot(Room.Wine_0, 1));
        _map_graph.spot_link(new ISpot(Room.Bath_1, 0), new ISpot(Room.Bath_1, 1));
        _map_graph.spot_link(new ISpot(Room.Dining_1, 0), new ISpot(Room.Dining_1, 1));
        _map_graph.spot_link(new ISpot(Room.Dining_1, 0), new ISpot(Room.Dining_1, 2));
        _map_graph.spot_link(new ISpot(Room.Dining_1, 1), new ISpot(Room.Dining_1, 3));
        _map_graph.spot_link(new ISpot(Room.Dining_1, 2), new ISpot(Room.Dining_1, 3));
        _map_graph.spot_link(new ISpot(Room.Hall_1, 0), new ISpot(Room.Hall_1, 1));
        _map_graph.spot_link(new ISpot(Room.Hall_1, 1), new ISpot(Room.Hall_1, 2));
        _map_graph.spot_link(new ISpot(Room.Kitchen_1, 0), new ISpot(Room.Kitchen_1, 1));
        _map_graph.spot_link(new ISpot(Room.Kitchen_1, 0), new ISpot(Room.Kitchen_1, 2));
        _map_graph.spot_link(new ISpot(Room.Laundry_1, 0), new ISpot(Room.Laundry_1, 1));
        _map_graph.spot_link(new ISpot(Room.Living_1, 0), new ISpot(Room.Living_1, 1));
        _map_graph.spot_link(new ISpot(Room.Living_1, 0), new ISpot(Room.Living_1, 2));
        _map_graph.spot_link(new ISpot(Room.Living_1, 1), new ISpot(Room.Living_1, 2));
        _map_graph.spot_link(new ISpot(Room.Pantry_1, 0), new ISpot(Room.Pantry_1, 1));
        _map_graph.spot_link(new ISpot(Room.Reception_1, 0), new ISpot(Room.Reception_1, 1));
        _map_graph.spot_link(new ISpot(Room.Baby_2, 0), new ISpot(Room.Baby_2, 1));
        _map_graph.spot_link(new ISpot(Room.Bath_2, 0), new ISpot(Room.Bath_2, 1));
        _map_graph.spot_link(new ISpot(Room.Bed_2, 0), new ISpot(Room.Bed_2, 1));
        _map_graph.spot_link(new ISpot(Room.Bed_2, 1), new ISpot(Room.Bed_2, 2));
        _map_graph.spot_link(new ISpot(Room.Dress_2, 0), new ISpot(Room.Dress_2, 1));
        _map_graph.spot_link(new ISpot(Room.Hall_2, 0), new ISpot(Room.Hall_2, 1));
        _map_graph.spot_link(new ISpot(Room.Hall_2, 0), new ISpot(Room.Hall_2, 2));
        _map_graph.spot_link(new ISpot(Room.Hall_2, 0), new ISpot(Room.Hall_2, 5));
        _map_graph.spot_link(new ISpot(Room.Hall_2, 1), new ISpot(Room.Hall_2, 6));
        _map_graph.spot_link(new ISpot(Room.Hall_2, 2), new ISpot(Room.Hall_2, 3));
        _map_graph.spot_link(new ISpot(Room.Hall_2, 3), new ISpot(Room.Hall_2, 4));
        _map_graph.spot_link(new ISpot(Room.Hall_2, 5), new ISpot(Room.Hall_2, 6));
        _map_graph.spot_link(new ISpot(Room.Library_2, 0), new ISpot(Room.Library_2, 1));
        _map_graph.spot_link(new ISpot(Room.Library_2, 1), new ISpot(Room.Library_2, 2));
        _map_graph.spot_link(new ISpot(Room.Swimming_2, 0), new ISpot(Room.Swimming_2, 1));
        _map_graph.spot_link(new ISpot(Room.Swimming_2, 1), new ISpot(Room.Swimming_2, 2));
        _map_graph.spot_link(new ISpot(Room.Garret_3, 0), new ISpot(Room.Garret_3, 1));

        //방끼리 이어주기
        _map_graph.spot_link(new ISpot(Room.Wine_0, 1), new ISpot(Room.Living_1, 0));
        _map_graph.spot_link(new ISpot(Room.Hall_1, 0), new ISpot(Room.Laundry_1, 1));
        _map_graph.spot_link(new ISpot(Room.Living_1, 2), new ISpot(Room.Hall_1, 0));
        _map_graph.spot_link(new ISpot(Room.Front_1, 0), new ISpot(Room.Hall_1, 0));
        _map_graph.spot_link(new ISpot(Room.Hall_1, 2), new ISpot(Room.Reception_1, 0));
        _map_graph.spot_link(new ISpot(Room.Hall_1, 2), new ISpot(Room.Dining_1, 2));
        _map_graph.spot_link(new ISpot(Room.Hall_1, 2), new ISpot(Room.Kitchen_1, 1));
        _map_graph.spot_link(new ISpot(Room.Kitchen_1, 0), new ISpot(Room.Pantry_1, 0));
        _map_graph.spot_link(new ISpot(Room.Bath_1, 1), new ISpot(Room.Laundry_1, 1));
        _map_graph.spot_link(new ISpot(Room.Hall_1, 1), new ISpot(Room.Hall_2, 1));
        _map_graph.spot_link(new ISpot(Room.Garret_3, 1), new ISpot(Room.Hall_2, 1));
        _map_graph.spot_link(new ISpot(Room.Library_2, 2), new ISpot(Room.Hall_2, 1));
        _map_graph.spot_link(new ISpot(Room.Photo_2, 0), new ISpot(Room.Library_2, 1));
        _map_graph.spot_link(new ISpot(Room.Library_2, 0), new ISpot(Room.Hall_2, 4));
        _map_graph.spot_link(new ISpot(Room.Empty_2, 0), new ISpot(Room.Hall_2, 4));
        _map_graph.spot_link(new ISpot(Room.Bath_2, 1), new ISpot(Room.Hall_2, 2));
        _map_graph.spot_link(new ISpot(Room.Hall_2, 0), new ISpot(Room.Bed_2, 1));
        _map_graph.spot_link(new ISpot(Room.Baby_2, 1), new ISpot(Room.Dress_2, 0));
        _map_graph.spot_link(new ISpot(Room.Hall_2, 5), new ISpot(Room.Baby_2, 0));
        _map_graph.spot_link(new ISpot(Room.Hall_2, 6), new ISpot(Room.Swimming_2, 2));
        _map_graph.spot_link(new ISpot(Room.Bath_2, 0), new ISpot(Room.Bed_2, 0));
        _map_graph.spot_link(new ISpot(Room.Bed_2, 2), new ISpot(Room.Dress_2, 1));

        //포탈
        _portal_list.Add(new Portal(1, Room.Wine_0, Room.Living_1));
        _portal_list.Add(new Portal(2, Room.Hall_1, Room.Front_1));
        _portal_list.Add(new Portal(3, Room.Hall_1, Room.Living_1));
        _portal_list.Add(new Portal(4, Room.Bath_1, Room.Laundry_1));
        _portal_list.Add(new Portal(5, Room.Hall_1, Room.Laundry_1));
        _portal_list.Add(new Portal(6, Room.Hall_1, Room.Kitchen_1));
        _portal_list.Add(new Portal(7, Room.Pantry_1, Room.Kitchen_1));
        _portal_list.Add(new Portal(8, Room.Hall_1, Room.Dining_1));
        _portal_list.Add(new Portal(9, Room.Hall_1, Room.Reception_1));
        _portal_list.Add(new Portal(10, Room.Hall_1, Room.Hall_2));
        _portal_list.Add(new Portal(11, Room.Hall_2, Room.Bath_2));
        _portal_list.Add(new Portal(12, Room.Bath_2, Room.Bed_2));
        _portal_list.Add(new Portal(13, Room.Hall_2, Room.Bed_2));
        _portal_list.Add(new Portal(14, Room.Dress_2, Room.Bed_2));
        _portal_list.Add(new Portal(15, Room.Dress_2, Room.Baby_2));
        _portal_list.Add(new Portal(16, Room.Hall_2, Room.Baby_2));
        _portal_list.Add(new Portal(17, Room.Hall_2, Room.Swimming_2));
        _portal_list.Add(new Portal(18, Room.Hall_2, Room.Garret_3));
        _portal_list.Add(new Portal(19, Room.Hall_2, Room.Library_2));
        _portal_list.Add(new Portal(20, Room.Photo_2, Room.Library_2));
        _portal_list.Add(new Portal(21, Room.Hall_2, Room.Empty_2));
        _portal_list.Add(new Portal(22, Room.Hall_2, Room.Library_2));
    }
    public static Scene_Manager getInstance()
    {
        if (instance == null)
        {
            instance = new Scene_Manager();
        }
        return instance;
    }
    public Route find_shortest(ISpot start, ISpot end, ref int distance, List<ISpot> _checked)
    {
        _checked.Add(start);
        Route _route = new Route(start);
        List<Spot> _next_spots = _map_graph.find_spot(start).near_spots;

        if (_next_spots.Contains(_map_graph.find_spot(end)))
        {
            //종점과 붙어있을 때
            _route._next = new Route(end);
            distance = 1;
        }
        else
        {
            //종점과 안붙어있을 때
            int min_distance = MAX_ROUTE_LENGTH;
            Route min_route = null;
            foreach (var v in _next_spots)
            {
                //주위 spots들 둘러보기
                bool flag = false;
                foreach (var tmp in _checked)
                {
                    if (tmp._room == v._room && tmp._spot == v._spot)
                    {
                        flag = true;
                        break;
                    }
                }
                //안둘러본 애만 둘러보기
                if (!flag)
                {
                    int d = 0;
                    Route r = find_shortest(v, end, ref d, new List<ISpot>(_checked));
                    if (min_distance > d)
                    {
                        min_route = r;
                        min_distance = d;
                    }
                }
            }
            distance = min_distance + 1;
            _route._next = min_route;
        }
        return _route;
    }
    public Portal _get_portal(int key)
    {
        foreach (var v in _portal_list)
        {
            if (v.get_key() == key) return v;
        }
        return null;
    }
    public Room get_room_info(string s_name)
    {
        Room result = Room.None;
        for (int i = 0; i < scene_name.Length; i++)
        {
            if (s_name == scene_name[i]) result = (Room)i;
        }
        return result;
    }
    public Vector3 _get_portal_loc(Room start, Room end)
    {
        foreach (var v in _portal_list)
        {
            if (scene_name[(int)start] == v.scene_name1 && scene_name[(int)end] == v.scene_name2)
            {
                return new Vector3(v.portal_loc2.x,v.portal_loc2.y,0f);
            }
            else if (scene_name[(int)start] == v.scene_name2 && scene_name[(int)end] == v.scene_name1)
            {
                return new Vector3(v.portal_loc1.x, v.portal_loc1.y, 0f);
            }
        }
        return Enemy.ENEMY_INIT_LOC;
    }
    public Vector3 _get_portal_loc(int portal_key_num, Room end)
    {
        foreach (var v in _portal_list)
        {
            if (v.get_key() == portal_key_num)
            {
                if (v.scene_name1 == scene_name[(int)end]) return v.portal_loc1;
                else if (v.scene_name2 == scene_name[(int)end]) return v.portal_loc2;
            }
        }
        return Enemy.ENEMY_INIT_LOC;
    }
}

public class ISpot
{
    public Room _room;//방정보
    public int _spot;//방에서의 위치 정보

    public ISpot(Room r, int s)
    {
        _room = r; _spot = s;
    }
}
public class Spot : ISpot
{
    public List<Spot> near_spots;
    public Spot(Room r, int s)
        : base(r, s)
    {
        near_spots = new List<Spot>();
    }
}
public class Route
{
    private ISpot _data;
    public Route _next = null; //null값 : 끝의미

    //contructor, get, set
    public Route(ISpot s)
    {
        _data = s;
    }
    public ISpot get_data()
    {
        return _data;
    }

    public static Route add_route(Route r1, Route r2)
    {
        if (r1._next == null) return null;

        Route tmp = r1._next;
        while (tmp._next != null)
        {
            tmp = tmp._next;
        }
        tmp._next = r2;
        return r1;
    }

}
public class Map_Graph
{
    public List<Spot> spots;
    public Map_Graph()
    {
        spots = new List<Spot>();
    }
    //리스트에서 찾기 : ISpot -> Spot 해주는 함수
    public Spot find_spot(ISpot s)
    {
        var v = spots.FindAll(x => x._room == s._room && x._spot == s._spot);
        return v[0];
    }
    //두 스폿 이어주는 함수
    public void spot_link(ISpot s1, ISpot s2)
    {
        Spot v1 = find_spot(s1);
        Spot v2 = find_spot(s2);

        v1.near_spots.Add(v2);
        v2.near_spots.Add(v1);
    }
}
