using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_click_test : MonoBehaviour {
    public Camera Camera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseDown()
    {
        Debug.Log(Camera.ScreenToWorldPoint(Input.mousePosition));
    }
}
