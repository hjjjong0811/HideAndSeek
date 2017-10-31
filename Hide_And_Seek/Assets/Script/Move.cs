using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    public float Horizontal = 0, Vertical = 0;
    public bool Run = false;
    
	// Update is called once per frame
	void Update () {
        //키보드입력
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.X)) {
            Run = true;
        } else {
            Run = false;
        }
    }
}
