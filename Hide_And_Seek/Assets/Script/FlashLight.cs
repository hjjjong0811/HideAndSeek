﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour {
    private readonly int Battery_max = 100, Battery_lack = 30;              //battery
    private readonly float Light_power_on_ch = 8.0f, Light_power_off_ch = 0.0f;   //on off 시 intensity value
    private readonly float Light_power_on_obj = 180.0f, Light_power_off_obj = 10.0f;   //on off 시 intensity value

    private GameObject Player;  //player is using this, need position update
    private float Battery;      //battery  variable

    private Light Light_p, Light_o;

    private bool isLighted = true;      //on off?
    private float timeleft = 3.0f;      //일정시간마다 깜박이기 위함
    private float nexttime = 0.0f;
    private bool isFlashed = false;     //현재 깜박임중?
    

    public void LinkUser(GameObject user) {
        Player = user;
    }
    public void Start() {
        Battery = Battery_max;
        Light_p = GetComponent<Light>();        //Light init
        Light_p.range = 5;
        Light_p.intensity = Light_power_on_ch;
        Light_o = transform.GetChild(0).GetComponent<Light>();
        Light_o.range = 4;
        Light_o.intensity = Light_power_on_obj;
    }

    // Update is called once per frame
    void Update () {
        Battery = (Battery > 0) ? Battery - (10 * Time.deltaTime) : 0; //시간경과시 배터리 방전
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -3f);

        //timeleft마다
        if (isLighted && Battery < Battery_lack && Time.time > nexttime) {
            nexttime = Time.time + timeleft;
            //30퍼확률로 꿈벅
            if (!isFlashed && Random.value < 0.3) {
                flashedLight(2f);
            }
        }

        //test on off
        if (!isFlashed && Input.GetKeyDown(KeyCode.A)) {
            Debug.Log(Battery + "");
            setLight(!isLighted);
            isLighted = !isLighted;
        }
    }

    //charge 만큼 배터리를 채우는 메소드
    public void chargeBattery(int charge) {
        Battery += charge;
        if (Battery >= Battery_max) Battery = Battery_max;
    }

    //setLight true->킨다, false->끈다
    public void setLight(bool value) {
        if (value) {
            Light_p.intensity = Light_power_on_ch;
            Light_o.intensity = Light_power_on_obj;
        } else {
            Light_p.intensity = Light_power_off_ch;
            Light_o.intensity = Light_power_off_obj;
        }
    }

    //For Flashed in some time
    public void flashedLight(float time) {
        isFlashed = true;
        StartCoroutine(coroutineFlash());
        Invoke("coroutineEnd", time);
    }

    void coroutineEnd() {
        isFlashed = false;
    }

    IEnumerator coroutineFlash() {
        bool toggle = false;
        while (isFlashed) {
            setLight(toggle);
            toggle = !toggle;
            yield return new WaitForSeconds(Random.Range(0.001f, 0.15f));
        }
        setLight(true);
        yield break;

    }
}
