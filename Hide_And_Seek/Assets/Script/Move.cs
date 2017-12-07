using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    public float Horizontal = 0, Vertical = 0;
    public bool Run = false;
    
	void Update () {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        if (Input.GetButton("Run")) {
            Run = true;
        } else {
            Run = false;
        }
    }
}
