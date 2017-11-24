using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour {

    /*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	*/

    public Vector2 a;


	private static ScriptManager instance = null;

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


    public void showScript()
    {

    }
    


}
