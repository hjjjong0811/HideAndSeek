using System.Collections;
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
    private float batteryleft = 0.5f;   //배터리 닳는 속도(값이 커지면 빨라짐)


    public void Init(GameObject user) {
        Player = user;
    }
    private void Awake() {
        Battery = PlayerPrefs.GetFloat("Flash_Battery", Battery_max);
        if (PlayerPrefs.GetInt("Flash_IsLighted", 1) == 1) isLighted = true;
        else isLighted = false;
    }
    public void Start() {
        Light_p = GetComponent<Light>();        //Light init
        Light_p.range = 5;
        Light_p.intensity = Light_power_on_ch;
        Light_o = transform.GetChild(0).GetComponent<Light>();
        Light_o.range = 4;
        Light_o.intensity = Light_power_on_obj;
        
        setLight(isLighted);
    }

    private void OnDestroy() {
        PlayerPrefs.SetFloat("Flash_Battery", Battery);
        if (isLighted) PlayerPrefs.SetInt("Flash_IsLighted", 1);
        else PlayerPrefs.SetInt("Flash_IsLighted", 0);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update () {
        Battery = (Battery > 0) ? Battery - (batteryleft * Time.deltaTime) : 0; //시간경과시 배터리 방전
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -3f);

        //timeleft마다
        if (isLighted && Battery < Battery_lack && Time.time > nexttime) {
            nexttime = Time.time + timeleft;
            //30퍼확률로 꿈벅
            if (!isFlashed && Random.value < 0.3) {
                flashedLight(2f);
            }
        }
        
    }

    public float getBattery() {
        return Battery;
    }
    public void setBattery(float battery) {
        Battery = battery;
    }
    public bool getIsLighted() {
        return isLighted;
    }
    /// <summary>
    /// 배터리 충전 메서드
    /// </summary>
    /// <param name="charge">충전 배터리 양</param>
    public void chargeBattery(int charge) {
        Battery += charge;
        if (Battery >= Battery_max) Battery = Battery_max;
    }

    /// <summary>
    /// 손전등 키고 끄기
    /// </summary>
    /// <param name="value">true->on false->off</param>
    public void setLight(bool value) {
        if (value) {
            Light_p.intensity = Light_power_on_ch;
            Light_o.intensity = Light_power_on_obj;
            isLighted = true;
        } else {
            Light_p.intensity = Light_power_off_ch;
            Light_o.intensity = Light_power_off_obj;
            isLighted = false;
        }
    }

    /// <summary>
    /// 깜박깜박지지직해줌
    /// </summary>
    /// <param name="time">지속시간 time second</param>
    public void flashedLight(float time) {
        isFlashed = true;
        StartCoroutine(coroutineFlash());
        Invoke("coroutineEnd", time);
    }

    private void coroutineEnd() {
        isFlashed = false;
    }

    private IEnumerator coroutineFlash() {
        bool toggle = false;
        while (isFlashed) {
            setLight(toggle);
            toggle = !toggle;
            yield return new WaitForSeconds(Random.Range(0.001f, 0.15f));
        }
        setLight(isLighted);
        yield break;

    }
}
