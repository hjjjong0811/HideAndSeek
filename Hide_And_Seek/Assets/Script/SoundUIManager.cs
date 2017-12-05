using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundUIManager : MonoBehaviour {

    private static SoundUIManager instance = null;

    public AudioSource BGM;
    public AudioSource Effect;
    public AudioSource FootStep;
   
    public Toggle BGM_toggle;
    public Toggle Effect_Toggle; // effect + footstep

    public Slider BGM_Slider;
    public Slider Effect_Slider; // effect + footstep

    

    public static SoundUIManager getInstance()
    {
        if (instance == null)
        {
            instance = new SoundUIManager();
        }
        return instance;
    }

    public void Start()
    {
        
        SoundManager soundmanager = SoundManager.getInstance();
       
        BGM = soundmanager.bgmSource;
        Effect = soundmanager.effectSource;
        FootStep = soundmanager.walkSource;

    

        BGM_Slider.value = soundmanager.volume_bgm;
        Effect_Slider.value = soundmanager.volume_effect;

        toggle_state("bgm_", BGM_toggle);
        toggle_state("effect_", Effect_Toggle);

        BGM_Slider.value = PlayerPrefs.GetFloat("bgm_slider_value");
        Effect_Slider.value = PlayerPrefs.GetFloat("effect_slider_value");


 
    }

    public void Update()
    {

        BGM_Slider.value = PlayerPrefs.GetFloat("bgm_slider_value");
        Effect_Slider.value = PlayerPrefs.GetFloat("effect_slider_value");

        toggle_state("bgm_", BGM_toggle);
        toggle_state("effect_", Effect_Toggle);

    }

    /// <summary>
    /// toggle 상태유지하기
    /// </summary>
    /// <param name="type">soundtype(effect_,bgm_)</param>
    /// <param name="toggle">toggle name</param>
    public void toggle_state(string type, Toggle toggle)
    {
        if (PlayerPrefs.GetInt(type) == 1)
            toggle.isOn = false;
        else
            toggle.isOn = true;
    }

    public void BGM_Volume() // BGM 볼륨조절
    {
        if (PlayerPrefs.GetInt("bgm_") == 0)
        {
            BGM.volume = BGM_Slider.value;
            SoundManager.getInstance().volume_bgm = BGM.volume;
            PlayerPrefs.SetFloat("bgm_slider_value", BGM.volume);
        }
        else
        {
            BGM.volume = BGM_Slider.value;
            PlayerPrefs.SetFloat("bgm_slider_value", BGM.volume);
        }
    }

    public void Effect_Volume() // 효과음 볼륨조절
    {
        if(PlayerPrefs.GetInt("effect_") == 0)
        {
            Effect.volume = Effect_Slider.value;
            SoundManager.getInstance().volume_effect = Effect.volume;
            PlayerPrefs.SetFloat("effect_slider_value", Effect.volume);
        }
        else
        {
            Effect.volume = Effect_Slider.value;
            PlayerPrefs.SetFloat("effect_slider_value", Effect.volume);
        }
      
    }

    public void BGM_ONOFF()// BGM ONOFF
    {
        if (BGM_toggle.isOn.Equals(true))
        {
            PlayerPrefs.SetInt("bgm_", 0);
            SoundManager.getInstance().volume_bgm = BGM_Slider.value;

        }
        else
        {
            PlayerPrefs.SetInt("bgm_", 1);
            SoundManager.getInstance().volume_bgm = 0.0f;
        }
    } 

    public void Effect_ONOFF() // 효과음 ONOFF
    {
        if (Effect_Toggle.isOn.Equals(true))
        {
            PlayerPrefs.SetInt("effect_", 0);
            SoundManager.getInstance().volume_effect = Effect_Slider.value;
        }
        else
        {
            PlayerPrefs.SetInt("effect_", 1);
            SoundManager.getInstance().volume_effect = 0.0f;
        }
    }

    public void Btn_SoundOff()// 씬종료
    {
        if (GameObject.Find("Setting"))
        {
            GameObject.Find("Setting").GetComponent<CanvasGroup>().interactable = true;
        }
        else if(SceneManager.GetActiveScene().name=="UI_Start")
        {
            GameObject.Find("Canvas_Start").GetComponent<CanvasGroup>().interactable = true;
        }
        GameObject.Destroy(GameObject.Find("Sound"));
        
    } 

}
