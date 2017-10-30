using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_test : MonoBehaviour {
    string[] object_sorting = new string[] { "Object_back", "Object_front" };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        check_back_front();
	}

    void check_back_front()
    {
        if (this.transform.position.y > enemy_test.enemy_pos.y)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = object_sorting[0];
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = object_sorting[1];
        }
    }
}
