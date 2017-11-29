using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUIManager : MonoBehaviour {

    public AudioSource BGM;
    public AudioSource Effect;
    public AudioSource FootStep;
   
    public Toggle BGM_toggle;
    public Toggle Effect_Toggle;

    public Slider BGM_Slider;
    public Slider Effect_Slider;

    public void Awake()
    {
        BGM = GetComponent<AudioSource>();
        Effect = GetComponent<AudioSource>();
        FootStep = GetComponent<AudioSource>();
    }

    public void Start()
    {
        BGM.Play();
    }

    public void BGM_Volume() // BGM 볼륨조절
    {
        BGM.volume = BGM_Slider.value;
    }

    public void Effect_Volume() // 효과음 볼륨조절
    {
        Effect.volume = Effect_Slider.value;
    }

    public void BGM_ONOFF()
    {
        if (BGM_toggle.isOn.Equals(true))
            BGM.Play();
        else
            BGM.Stop();
    } // BGM ONOFF

    public void Effect_ONOFF()
    {
        if (Effect_Toggle.isOn.Equals(true))
        {
            Effect.Play();
            FootStep.Play();
        }
        else
        {
            Effect.Stop();
            FootStep.Stop();
        }
    } // 효과음 ONOFF

 
public void Btn_SoundOff()
    {
        GameObject.Destroy(GameObject.Find("Sound"));
    } // 씬종료

}
