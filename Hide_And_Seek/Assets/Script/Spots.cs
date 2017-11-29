using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spots : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D c)
    {
        if (c.name=="Player")
        {
            //Debug.Log(c.name + "/" + this.gameObject.name);//test
            GameObject pl = GameObject.Find("Player");
            if (pl != null)
            {
                pl.GetComponent<Player>().SpotInfo._spot = int.Parse(this.gameObject.name);
                //Debug.Log("플레이어 spot넘버 : "+pl.GetComponent<Player>().SpotInfo._spot);//test
            }
        }
    }
}
