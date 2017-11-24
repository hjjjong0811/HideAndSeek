using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager {
    /*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
   */

    private static ObjectManager instance = null;

    public static ObjectManager getInstance()
    {
        if (instance == null)
        {
            instance = new ObjectManager();
        }
        return instance;
    }
   
    public int key = 0;
    public List<Obj> objList;//가지고있을 오브젝트 정보
    public List<Obj> objChangeList;//상태 변한 ㅇ오브겍트

    public void inputObj(Obj obj) { objList.Add(obj); }//오브젝트 리스트에 추가


    //키값으로 오브젝트 검색
    //public Obj findObj(int key)
    //{ return obj ; } 현정 임시주석처리함, obj를찾을수없음

    //바뀐 오브젝트 리스트 반환
    public List<Obj> getObjChange()
    {
        return objChangeList;
    }

    //오브젝트 발동
    public void action()
    {
        //진행도 확인후
        //소리있으면 소리내고 변화있으면 변화
        //변화 리스트에 추가
        //어그로 있으면 적호출
        //아이템
    }
    



}
