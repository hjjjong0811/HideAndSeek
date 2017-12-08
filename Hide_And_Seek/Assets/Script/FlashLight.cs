using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour {
    private const int Battery_max = 100, Battery_lack = 30;
    private readonly float Light_position_z = -2f, Light_range_ch = 3f, Light_range_obj = 2.6f;
    private readonly float Light_power_on_ch = 8.0f, Light_power_off_ch = 0.0f;
    private readonly float Light_power_on_obj = 180.0f, Light_power_off_obj = 10.0f;

    private GameObject Player;
    private float Battery;

    private Light Light_p, Light_o;

    private bool isLighted = true;
    private bool isFlashed = false;
    private bool isFade = false;
    private bool isMoved = false;
    private float batteryleft = 0.01f;

    private AudioClip Audio_BatteryLack;
    
    public static void Init(float pBattery, bool pIsLighted) {
        PlayerPrefs.SetFloat("Flash_Battery", pBattery);
        if (pIsLighted) PlayerPrefs.SetInt("Flash_IsLighted", 1);
        else PlayerPrefs.SetInt("Flash_IsLighted", 0);
        PlayerPrefs.Save();

        GameObject fl = GameObject.Find("Flash");
        if (fl != null) {
            fl.GetComponent<FlashLight>().setBattery(pBattery);
            fl.GetComponent<FlashLight>().setLight(pIsLighted);
        }
    }
    
    public static float getFlashData() {
        GameObject fl = GameObject.Find("Flash");
        if(fl != null) {
            return fl.GetComponent<FlashLight>().getBattery();
        } else {
            return PlayerPrefs.GetFloat("Flash_Battery", Battery_max);
        }
    }

    public void LinkUser(GameObject user) {
        Player = user;
    }
    private void Awake() {
        Battery = PlayerPrefs.GetFloat("Flash_Battery", Battery_max);
        if (PlayerPrefs.GetInt("Flash_IsLighted", 1) == 1) isLighted = true;
        else isLighted = false;
        Audio_BatteryLack = Resources.Load("Sounds/247") as AudioClip;
    }
    public void Start() {
        Light_p = GetComponent<Light>();        //Light init
        Light_p.range = Light_range_ch;
        Light_p.intensity = Light_power_on_ch;
        Light_o = transform.GetChild(0).GetComponent<Light>();
        Light_o.range = Light_range_obj;
        Light_o.intensity = Light_power_on_obj;
        
        setLight(isLighted);
        if (isLighted) fadeIn(1.0f);
        StartCoroutine(UpdateFlash());
    }

    private void OnDestroy() {
        PlayerPrefs.SetFloat("Flash_Battery", Battery);
        if (isLighted) PlayerPrefs.SetInt("Flash_IsLighted", 1);
        else PlayerPrefs.SetInt("Flash_IsLighted", 0);
        PlayerPrefs.Save();
    }
    
    private IEnumerator UpdateFlash() {
        while (true) {
            if(isLighted)
                Battery = Battery - (batteryleft);
            
            if (isLighted && Battery < Battery_lack) {
                if (!isFlashed && Random.value < 0.1) {
                    SoundManager.getInstance().playEffect(Audio_BatteryLack);
                    flashedLight(2f);
                }
            }
        
            if(Battery <= 0) {
                PlayScene.getInstance().playScene(PlayScene.numScene.batteryLack);
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void LateUpdate() {
        if (Player != null) {
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 0.3f, Light_position_z);
        }
    }

    public void setPosition(Vector2 v) {
        if (Player != null) return;
        transform.position = new Vector3(v.x, v.y, Light_position_z);
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

    public static void chargeBattery(float charge) {
        GameObject go = GameObject.Find("Flash");
        if(go != null) {
            FlashLight flash = go.GetComponent<FlashLight>();
            flash.Battery = flash.Battery + charge;
            if (flash.Battery >= Battery_max) flash.Battery = Battery_max;
        } else {
            float newBattery = PlayerPrefs.GetFloat("Flash_Battery", Battery_max);
            newBattery = newBattery + charge;
            PlayerPrefs.SetFloat("Flash_Battery", newBattery);
        }
        
    }

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
    

    public void flashedLight(float time) {
        isFlashed = true;
        StartCoroutine(coroutineFlash());
        Invoke("coroutineEnd", time);
    }
    public void fadeIn(float time) {
        if (isFade) return;
        isFade = true;
        StartCoroutine(coroutineFadein(time));
        Invoke("coroutineEnd", time);
    }
    public void move(Vector2 position) {
        if (Player != null) return;
        StartCoroutine(moveSlow(position));
    }

    private IEnumerator moveSlow(Vector2 position) {
        isMoved = true;
        float x_p = (position.x - this.transform.position.x) * 0.1f;
        float y_p = (position.y - this.transform.position.y) * 0.1f;
        for (int i = 0; i < 10; i++) {
            this.transform.position = new Vector3(transform.position.x + x_p, transform.position.y + y_p, Light_position_z);
            yield return new WaitForSeconds(0.05f);
        }
        isMoved = false;
        yield break;
    }
    private void coroutineEnd() {
        isFlashed = false;
        isFade = false;
    }
    private IEnumerator coroutineFlash() {
        bool toggle = false;
        while (isFlashed && isLighted) {
            if (toggle) {
                Light_p.intensity = Light_power_on_ch;
                Light_o.intensity = Light_power_on_obj;
            } else {
                Light_p.intensity = Light_power_off_ch;
                Light_o.intensity = Light_power_off_obj;
            }
            toggle = !toggle;
            yield return new WaitForSeconds(Random.Range(0.001f, 0.15f));
        }
        setLight(isLighted);
        isFlashed = false;
        isFade = false;
        CancelInvoke();
        yield break;

    }
    private IEnumerator coroutineFadein(float time) {
        Light_p.intensity = Light_power_off_ch;
        Light_o.intensity = Light_power_off_obj;
        while (isFade) {
            Light_p.intensity += (Light_power_on_ch - Light_power_off_ch) / (time * 10f);
            Light_o.intensity += (Light_power_on_obj - Light_power_off_obj) / (time * 10f);
            yield return new WaitForSeconds(0.1f);
        }
        setLight(true);
        yield break;

    }
}
