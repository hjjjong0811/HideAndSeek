using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public int _key_num;
    public bool _t_thing_f_portal = false;//오브젝트 구분용 (true : 씬이동용 포탈 / false : 스크립트용 물건)
    IObject _obj;

    // Use this for initialization
    void Start()
    {
        if (_t_thing_f_portal)
        {
            _obj = new Thing(_key_num, this.gameObject);
            _obj.for_start();
        }
        else
        {
            _obj = Scene_Manager.getInstance()._get_portal(_key_num);
            _obj.for_start();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _obj.for_update();
    }

}
