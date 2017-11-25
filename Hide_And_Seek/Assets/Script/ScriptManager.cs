using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WHO
{
    HYO_JUNG=0,
    JUNG_YEON,
    SU_UN,
    HA_BIN
}
struct sTalkScript
{
    int key;    //대화 순서 키값   
    string str; //대화내용
    WHO who;    //말한사람
}

public class ScriptManager : MonoBehaviour {

    /*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	*/


	private static ScriptManager instance = null;

    /// <summary>
    /// 오브젝트 스크립트 모음
    /// </summary>
    private string[] objScriptList;

    /// <summary>
    /// 대화 스크립트 모음
    /// </summary>
    private sTalkScript[] talkScriptList; 




	public static ScriptManager getInstance()
	{
		if (instance == null)
		{
			instance = new ScriptManager();
		}

		return instance;
	}

	public string findScript(int key)
	{
		string _string=null;

        


		return _string;
	}


    /// <summary>
    /// 오브젝트 클릭시 나타나는 스크립트
    /// </summary>
    public void showObjScript()
    {

    }

    /// <summary>
    /// 대화시 나타나는 스크립트
    /// </summary>
    public void showTalkScript()
    {
        
    }
    


}
