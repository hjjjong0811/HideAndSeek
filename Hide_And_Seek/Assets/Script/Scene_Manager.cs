using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Manager : MonoBehaviour {
    /*
    string[] Scene_Names = new string[] { 
        "0_Wine", //0
        "1_Bath", //1
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
        "3_Garret", //19
        "S_BBQ"//일단 여기에 둠
    };*/
    Scene[] Scenes = new Scene[]{
        new Scene("0_Wine",2, new int[,]{{0,1}}),
        new Scene("1_Bath",2, new int[,]{{0,1}}),
        new Scene("1_Dining",4, new int[,]{{0,1},{0,2},{1,3},{2,3}}),
        new Scene("1_Front",1, new int[,]{}),
        new Scene("1_Hall",3, new int[,]{{0,1},{1,2}}),
        new Scene("1_Kitchen",3, new int[,]{{0,1},{0,2}}),
        new Scene("1_Laundry",2, new int[,]{{0,2}}),
        new Scene("1_Living",3, new int[,]{{0,1},{0,2},{1,2}}),
        new Scene("1_Pantry",2, new int[,]{{0,1}}),
        new Scene("1_Reception",2, new int[,]{{0,1}}),
        new Scene("2_Baby",3, new int[,]{{0,1},{1,2}}),
        new Scene("2_Bath",2, new int[,]{{0,1}}),
        new Scene("2_Bed",3, new int[,]{{0,1},{1,2}}),
        new Scene("2_Dress",2, new int[,]{{0,1}}),
        new Scene("2_Empty",1, new int[,]{}),
        new Scene("2_Hall",7, new int[,]{{0,1},{0,2},{0,5},{1,6},{2,3},{3,4},{5,6}}),
        new Scene("2_Library",3, new int[,]{{0,1},{1,2}}),
        new Scene("2_Photo",1, new int[,]{}),
        new Scene("2_Swimming",3, new int[,]{{0,1},{1,2}}),
        new Scene("3_Garret",2, new int[,]{{0,1}}),
        new Scene("S_BBQ",0, new int[,]{}) //일단 여기 두기
    };

    //아직 설정안했음!! -> 해야함
    Vector3[,] Spot_Loc = new Vector3[,] {
        {new Vector3()}, //0_Wine
        {new Vector3()}, //1_Bath
        {new Vector3()}, //1_Dining
        {new Vector3()}, //1_Front
        {new Vector3()}, //1_Hall
        {new Vector3()}, //1_Kitchen
        {new Vector3()}, //1_Laundry
        {new Vector3()}, //1_Living
        {new Vector3()}, //1_Pantry
        {new Vector3()}, //1_Reception
        {new Vector3()}, //2_Baby
        {new Vector3()}, //2_Bath
        {new Vector3()}, //2_Bed
        {new Vector3()}, //2_Dress
        {new Vector3()}, //2_Empty
        {new Vector3()}, //2_Hall
        {new Vector3()}, //2_Library
        {new Vector3()}, //2_Photo
        {new Vector3()} //2_Swimming
    };

    

	// Use this for initialization
    void Start()
    {
        //add spots on Spot List for each Scene instance
        for (int i = 0; i < Spot_Loc.Length; i++)
        {
            for (int j = 0; j < Scenes[i].get_spot_num(); j++)
            {
                Spot s = new Spot(Scenes[i].get_scene_name(), j, Spot_Loc[i, j]);
                Scenes[i].add_spot(j, s);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public class Spot
    {
        string scene_name;
        int num_in_scene;
        Vector3 loc;

        //constructor
        public Spot(string scene_name, int num_in_scene, Vector3 loc)
        {
            this.scene_name = scene_name;
            this.num_in_scene = num_in_scene;
            this.loc = loc;
        }
    }

    public class Scene{
        string scene_name;
        int spot_num;
        int[,] spot_to_spot;
        Spot[] spots_in_scene;

        public Scene(string scene_name, int spot_num, int[,] spot_to_spot){
            this.scene_name = scene_name;
            this.spot_num = spot_num;
            this.spot_to_spot = spot_to_spot;
        }

        public int get_spot_num(){
            return spot_num;
        }

        public string get_scene_name()
        {
            return scene_name;
        }

        public void add_spot(int i, Spot s){
            spots_in_scene[i] = s;
        }
    }
}

