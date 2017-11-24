using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj{
    /*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    */

    int objNum;     //고유식별번호 / 키값
    int item;       //보유중인 아이템 고유번호, 없으면 -1
    bool isUsed;    //첫 실행인지
    bool isSound;   //효과음 나는 여부
    bool isChange;  //외형이 바뀌었는지
    bool isHide;    //숨을수 있나


    int getKey() { return objNum; }






}
