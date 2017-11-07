using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        Scene_Manager sc = Scene_Manager.getInstance();

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
         */


        int distance = 0;
        Route r = sc.find_shortest(new ISpot(Room.Wine_0, 0), new ISpot(Room.Dress_2, 0), ref distance, new List<ISpot>());
        while (r != null)
        {
            Debug.Log(r.get_data()._room + " / " + r.get_data()._spot);//test
            r = r._next;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
