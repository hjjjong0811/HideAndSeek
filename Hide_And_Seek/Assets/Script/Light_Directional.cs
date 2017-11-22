using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Directional : MonoBehaviour {
    private readonly float Light_power_on_ch = 0.7f, Light_power_off_ch = 0.0f;   //on off 시 intensity value
    private readonly float Light_power_on_obj = 8.5f, Light_power_off_obj = 0.0f;   //on off 시 intensity value
    
    private Light Light_p, Light_o;
    public bool isFade;

    // Use this for initialization
    void Start () {
        Light_p = GetComponent<Light>();        //Light init
        Light_p.intensity = Light_power_on_ch;
        Light_o = transform.GetChild(0).GetComponent<Light>();
        Light_o.intensity = Light_power_on_obj;

    }
    public void setLight(bool value) {
        if (value) {
            Light_p.intensity = Light_power_on_ch;
            Light_o.intensity = Light_power_on_obj;
        } else {
            Light_p.intensity = Light_power_off_ch;
            Light_o.intensity = Light_power_off_obj;
        }
    }
    public void fadeOut(float time) {
        isFade = true;
        StartCoroutine(coroutineFade(time));
        Invoke("coroutineEnd", time);
    }

    private void coroutineEnd() {
        isFade = false;
    }

    private IEnumerator coroutineFade(float time) {
        while (isFade) {
            Light_p.intensity -= Light_power_on_ch / (time / 0.1f);
            Light_o.intensity -= Light_power_on_obj / (time / 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        setLight(false);
        yield break;

    }
}
