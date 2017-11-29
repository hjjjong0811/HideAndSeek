using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUIManager : MonoBehaviour {

    public AudioSource BGM;
    public AudioSource Effect;
    public AudioSource FootStep;
   
    public Toggle BGMtoggle;
    public Toggle EffectToggle;


    public void Awake()
    {
        BGM = GetComponent<AudioSource>();
        Effect = GetComponent<AudioSource>();
        FootStep = GetComponent<AudioSource>();
    }

    public void Start()
    {
        audioSource.Play();
    
    }

    public void SaveData()
    {
        SceneManager.LoadScene("save");

    }


    public void Btn_Setting()
    {
        SettingPanel.SetActive(true);

    }

    public void Btn_SettingDelete()
    {
        SettingPanel.SetActive(false);
    }

    public void Btn_Developer()
    {
        DeveloperPanel.SetActive(true);
    }

    public void Btn_DeveloperDelete()
    {
        DeveloperPanel.SetActive(false);
    }

    public void SoundOnOff()
    {
        if (SoundControll.isOn.Equals(true))
        {

            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }





public void Btn_SoundOff()
    {
        GameObject.Destroy(GameObject.Find("Sound"));
    }

}
