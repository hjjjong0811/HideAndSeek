using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUIManager : MonoBehaviour {

    //public AudioSource audioSource;
    //public Toggle SoundController;

    public void Btn_SoundOff()
    {
        GameObject.Destroy(GameObject.Find("Sound"));
    }

}
