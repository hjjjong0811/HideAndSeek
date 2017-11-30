using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingUIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Action"))
        {
            Destroy(this.gameObject);
        }
	}

    public void stop_breath_button_click()
    {
    }
}
