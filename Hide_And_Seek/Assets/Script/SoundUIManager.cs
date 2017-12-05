using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundUIManager : MonoBehaviour {
    private static SoundUIManager instance = null;
    
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

        BGM_Slider.value = soundmanager.volume_bgm;
        Effect_Slider.value = soundmanager.volume_effect;

        if (soundmanager.isMuteBgm)
            BGM_toggle.isOn = false;

        if (soundmanager.isMuteEffect)
            Effect_Toggle.isOn = false;

    }



    public void BGM_Volume() // BGM 볼륨조절
    {
        SoundManager.getInstance().setVolumeBgm(BGM_Slider.value);
    }

    public void Effect_Volume() // 효과음 볼륨조절
    {
        SoundManager.getInstance().setVolumeEffect(Effect_Slider.value);
      
    }
    
    public void BGM_ONOFF()// BGM ONOFF
    {
        if (BGM_toggle.isOn.Equals(true))
            SoundManager.getInstance().unMuteBgm();
        else
            SoundManager.getInstance().muteBgm();
        } 

    public void Effect_ONOFF() // 효과음 ONOFF
    {
        if (Effect_Toggle.isOn.Equals(true))
            SoundManager.getInstance().unMuteEffect();
        else
            SoundManager.getInstance().muteEffect();
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
