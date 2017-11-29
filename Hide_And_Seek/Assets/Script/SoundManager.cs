﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{

    // === 외부 파라미터(Inspector 표시) =====================
    public bool DebugLog = false;
    public bool DontDestroyObjectOnLoad = true;
    private string SoundFolder;

    // === 내부 파라미터 ======================================
    const string FoxSoundGroupNID = "";//"FoxSoundGroup_";

    /// <summary>
    /// BGM, Effect 의 폴더명 변경시 메서드로
    /// </summary>
    public void setFolder(string name) {
        SoundFolder = "Sounds/" + name;
    }

    // === 코드(Monobehaviour 기본 기능 구현) ================
    void Awake()
    {
        SoundFolder = "Sounds/";    //Resources/Sounds/"filename"
        if (DontDestroyObjectOnLoad)
        {
            DontDestroyOnLoad(this);
        }
    }

    // === 코드(리소스 관리 구현) =========================
    public bool CreateGroup(string name)
    {
        GameObject go = new GameObject();
        go.name = FoxSoundGroupNID + name;
        go.transform.parent = transform;

        return false;
    }

    public GameObject GetGroup(string name)
    {
        return GameObject.Find(FoxSoundGroupNID + name);
    }

    public AudioSource LoadResourcesSound(string groupName, string fileName)
    {

        GameObject goSound = transform.Find(FoxSoundGroupNID + groupName).gameObject;
        AudioSource audioSource = goSound.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        //Debug 찍어봄! 경로명확인
        string path = SoundFolder + fileName;
        Debug.Log(path);
        AudioClip audioClip = Resources.Load(path, typeof(AudioClip)) as AudioClip;
        audioSource.clip = audioClip;

        return audioSource;
    }

    public AudioSource FindAudioSource(string groupName, string soundName)
    {
        GameObject goSound = transform.Find(FoxSoundGroupNID + groupName).gameObject;
        AudioSource[] audioSourceList = goSound.GetComponents<AudioSource>();

        foreach (AudioSource audioSource in audioSourceList)
        {
            if (audioSource.clip.name == soundName)
            {
                return audioSource;
            }
        }

        return null;
    }

    public AudioSource[] FindAudioSource(string groupName)
    {
        GameObject goSound = transform.Find(FoxSoundGroupNID + groupName).gameObject;
        return goSound.GetComponents<AudioSource>();
    }

    public void Play(AudioSource audioSource, bool loop)
    {
        audioSource.loop = loop;
        audioSource.Play();
    }

    // === 코드(재생 처리 구현) =============================
    public void Play(string groupName, string soundName, bool loop)
    {
        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {
            Play(audioSource, loop);
        }
    }

    public void PlayDontOverride(AudioSource audioSource, bool loop)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.loop = loop;
            audioSource.Play();
        }
    }

    public void PlayDontOverride(string groupName, string soundName, bool loop)
    {
        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {
            PlayDontOverride(audioSource, loop);
        }
    }

    public void PlayOneShot(AudioSource audioSource)
    {
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void PlayOneShot(string groupName, string soundName)
    {
        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {
            PlayOneShot(audioSource);
        }
    }

    public void Stop(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public void Stop(string groupName, string soundName)
    {
        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {
            Stop(audioSource);
        }
    }

    public void Stop(string groupName)
    {
        AudioSource[] audioSourceList = FindAudioSource(groupName);
        foreach (AudioSource audioSource in audioSourceList)
        {
            Stop(audioSource);
        }
    }

    public void StopAllSound()
    {
        AudioSource[] audios = transform.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            audio.Stop();
        }
    }

    // === 코드(볼륨 처리 구현) ========================
    public float GetVolume(AudioSource audioSource)
    {
        return audioSource.volume;
    }

    public float GetVolume(string groupName, string soundName)
    {
        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {
            return GetVolume(audioSource);
        }
        return 0.0f;
    }

    public void SetVolume(AudioSource audioSource, float vol)
    {
        audioSource.volume = vol;
    }

    public void SetVolume(string groupName, string soundName, float vol)
    {
        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {
            SetVolume(audioSource, vol);
        }
    }

    public void SetVolume(string groupName, float vol)
    {
        GameObject go = GetGroup(groupName);
        AudioSource[] audioSourceList = go.GetComponents<AudioSource>();

        foreach (AudioSource audioSource in audioSourceList)
        {
            SetVolume(audioSource, vol);
        }
    }

    // === 코드(페이드 처리 구현) ==========================
    class Fade
    {
        public AudioSource fadeAudio;
        public float targetV;
        public float dir;
        public float time;
        public float vmin, vmax;
        public Fade(AudioSource a, float v, float d, float t)
        {
            fadeAudio = a;
            targetV = v;
            dir = d;
            time = t;
            if (dir < 0.0f)
            {
                vmin = v;
                vmax = 1.0f;
            }
            else
            {
                vmin = 0.0f;
                vmax = v;
            }
        }
    };
    List<Fade> fadeStackList = new List<Fade>();

    public void FadeInVolume(AudioSource audioSource, float v, float t, bool init)
    {
        if (audioSource.volume < 1.0f && audioSource.isPlaying)
        {
            if (fadeStackList.Count <= 0)
            {
                InvokeRepeating("SoundFade", 0.0f, 0.02f);
            }
            if (init)
            {
                audioSource.volume = 0.0f;
            }
            fadeStackList.Add(new Fade(audioSource, v, +1.0f, t));
        }
    }
    public void FadeOutVolume(AudioSource audioSource, float v, float t, bool init)
    {
        if (audioSource.volume > 0.0f && audioSource.isPlaying)
        {
            if (fadeStackList.Count <= 0)
            {
                InvokeRepeating("SoundFade", 0.0f, 0.02f);
            }
            if (init)
            {
                audioSource.volume = 1.0f;
            }
            fadeStackList.Add(new Fade(audioSource, v, -1.0f, t));
        }
    }

    public void FadeOutVolumeGroup(string groupName, AudioSource playAudioSource, float v, float t, bool init)
    {
        GameObject go = GetGroup(groupName);
        AudioSource[] audioSourceList = go.GetComponents<AudioSource>();

        foreach (AudioSource audioSource in audioSourceList)
        {
            if (playAudioSource != audioSource)
            {
                FadeOutVolume(audioSource, v, t, init);
            }
        }
    }

    public void FadeOutVolumeGroup(string groupName, string soundName, float v, float t, bool init)
    {
        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {
            FadeOutVolumeGroup(groupName, audioSource, v, t, init);
        }
    }

    public void FadeOutVolumeGroup(string groupName, float v, float t, bool init)
    {
        FadeOutVolumeGroup(groupName, (AudioSource)null, v, t, init);
    }

    void SoundFade()
    {
        foreach (Fade fade in fadeStackList)
        {
            float v = fade.fadeAudio.volume + (1.0f * (0.02f / fade.time)) * fade.dir;
            SetVolume(fade.fadeAudio, v);
        }
        for (int i = 0; i < fadeStackList.Count; i++)
        {
            if (fadeStackList[i].fadeAudio.volume <= fadeStackList[i].vmin ||
                fadeStackList[i].fadeAudio.volume >= fadeStackList[i].vmax)
            {
                if (fadeStackList[i].fadeAudio.volume <= 0.0f)
                {
                    fadeStackList[i].fadeAudio.Stop();
                }
                fadeStackList.Remove(fadeStackList[i]);
            }
        }
        if (fadeStackList.Count <= 0)
        {
            CancelInvoke("SoundFade");
        }
    }

    // === 코드(지원 함수 구현) ========================
    public static SoundManager GetInstance(string gameObjectName = "SoundManager")
    {
        GameObject go = GameObject.Find(gameObjectName);
        if (go)
        {
            return go.GetComponent<SoundManager>();
        }
        return null;
    }
}


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 키값으로 쓰면됨
/// </summary>
/// 
public enum SOUND_TYPE
{
    TYPE_BGM,
    TYPE_EFFECT,
    TYPE_WALK

};
public enum SOUND_RESOURCE
{ 
    /// <summary>
    /// 물웅덩이에 나는소리
    /// </summary>
    WATER =0,

    /// <summary>
    /// 차박차박소리
    /// </summary>
    WALK1,

    /// <summary>
    /// 심장소리 천천히
    /// </summary>
    HEART_BEAT1,

    /// <summary>
    /// 심장소리 빠르게
    /// </summary>
    HEART_BEAT2,

    /// <summary>
    /// 심장소리 점점 빠르게
    /// </summary>
    HEART_BEAT3,

    /// <summary>
    /// 심장소리 점점 빠르게22
    /// </summary>
    HEART_BEAT4,

    /// <summary>
    /// 물 차박
    /// </summary>
    WATER_WALK,
    
    /// <summary>
    /// 게임오버시 피튀기는 소리
    /// </summary>
    DIE,
    
    /// <summary>
    /// 꺼림칙한 웃음소리
    /// </summary>
    LAUGH1,

    /// <summary>
    /// 낮은 중저음 웃음소리
    /// </summary>
    LAUGH2,


    /// <summary>
    /// 미친웃음소리
    /// </summary>
    LAUGH3,
    
    /// <summary>
    /// 계단 올라가는 발소리
    /// </summary>
    WALK2,
    
    /// <summary>
    /// 노크1
    /// </summary>
    KNOCK1,
    
    /// <summary>
    /// 계단올라가는 소리
    /// </summary>
    WALK3,
    
    /// <summary>
    /// 계단 올라가는소리, 발소리
    /// </summary>
    WALK4,
    
    /// <summary>
    /// 울리는 발소리
    /// </summary>
    WALK5,
    
    
    /// <summary>
    /// 조용히 발소리
    /// </summary>
    WALK6,
    
    /// <summary>
    /// 노크2
    /// </summary>
    KNOCK2,
    
    /// <summary>
    /// 문열리는 소리
    /// </summary>
    DOOR,
    
    /// <summary>
    ///여자 흐느끼는 소리
    /// </summary>
    CRY,
    
    /// <summary>
    /// 타이핑소리
    /// </summary>
    TYPING,
    
    /// <summary>
    /// 비명소리
    /// </summary>
    SCREAM,
    
    /// <summary>
    /// 일기장 페이지 넘기는 소리
    /// </summary>
    BOOK,
    
    /// <summary>
    /// 티비 켜져있는데 아무것도 안보일때 소리
    /// </summary>
    TV,
    
    /// <summary>
    /// 수영장 소리
    /// </summary>
    SWIMMING_POOL,
    
    /// <summary>
    /// 물 떨어지는 소리
    /// </summary>
    WATER_DROP,
    
    /// <summary>
    /// 연필로 글쓰는 소리
    /// </summary>
    WRITE,
    
    /// <summary>
    /// 까마귀 소리
    /// </summary>
    KKACK1,
    
    /// <summary>
    /// 까마귀 소리
    /// </summary>
    KKACK2,

    /// <summary>
    /// 브금
    /// </summary>
    BGM

}


public class SoundManager :MonoBehaviour{

    public static int BGM_MAX = 4;
    public static int EFFECT_MAX = 30;
    

    public static SoundManager Instance = null;

    public AudioClip[] audioList= new AudioClip[EFFECT_MAX];//효과음 저장
    public AudioClip[] bgmList = new AudioClip[BGM_MAX];
    public AudioSource bgmSource, effectSource , walkSource;

    public int nowClip=10;


    float _volume = 0.5f;    //볼륨값
    float _walkVolume=0.0f; //발소리 볼륨

    public static SoundManager getInstance()
    {
        if (Instance == null)
        {
            Instance = new SoundManager();
        }
        return Instance;
    }


    private void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }
    

    private void Start()
    {

        addSource();
        
        bgmSource = GetComponents<AudioSource>()[0];
        effectSource = GetComponents<AudioSource>()[1];
        walkSource = GetComponents<AudioSource>()[2];

        //브금 틀기
        BgmPlay(SOUND_RESOURCE.BGM); 
        

    }

    /// <summary>
    /// 리소스 추가
    /// </summary>
    private void addSource()
    {
        /// <summary>
        /// 물웅덩이에 나는소리
        /// </summary>
        audioList[0] = Resources.Load("001", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 차박차박소리
        /// </summary>
        audioList[1] = Resources.Load("002", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 심장소리 천천히
        /// </summary>
        audioList[2] = Resources.Load("003", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 심장소리 빠르게
        /// </summary>
        audioList[3] = Resources.Load("004", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 심장소리 점점 빠르게
        /// </summary>
        audioList[4] = Resources.Load("005", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 심장소리 점점 빠르게22
        /// </summary>
        audioList[5] = Resources.Load("006", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 물 차박
        /// </summary>
        audioList[6] = Resources.Load("007", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 게임오버시 피튀기는 소리
        /// </summary>
        audioList[7] = Resources.Load("008", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 꺼림칙한 웃음소리
        /// </summary>
        audioList[8] = Resources.Load("100", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 낮은 중저음 웃음소리
        /// </summary>
        audioList[9] = Resources.Load("101", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 미친웃음소리
        /// </summary>
        audioList[10] = Resources.Load("102", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 계단 올라가는 발소리
        /// </summary>
        audioList[11] = Resources.Load("103", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 노크1
        /// </summary>
        audioList[12] = Resources.Load("104", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 계단올라가는 소리
        /// </summary>
        audioList[13] = Resources.Load("105", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 계단 올라가는소리, 발소리
        /// </summary>
        audioList[14] = Resources.Load("106", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 울리는 발소리
        /// </summary>
        audioList[15] = Resources.Load("107", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 조용히 발소리
        /// </summary>
        audioList[16] = Resources.Load("108", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 노크2
        /// </summary>
        audioList[17] = Resources.Load("109", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 문열리는 소리
        /// </summary>
        audioList[18] = Resources.Load("110", typeof(AudioClip)) as AudioClip;

        /// <summary>
        ///여자 흐느끼는 소리
        /// </summary>
        audioList[19] = Resources.Load("200", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 타이핑소리
        /// </summary>
        audioList[20] = Resources.Load("201", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 비명소리
        /// </summary>
        audioList[21] = Resources.Load("202", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 일기장 페이지 넘기는 소리
        /// </summary>
        audioList[22] = Resources.Load("203", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 티비 켜져있는데 아무것도 안보일때 소리
        /// </summary>
        audioList[23] = Resources.Load("204", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 수영장 소리
        /// </summary>
        audioList[24] = Resources.Load("205", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 물 떨어지는 소리
        /// </summary>
        audioList[25] = Resources.Load("206", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 연필로 글쓰는 소리
        /// </summary>
        audioList[26] = Resources.Load("207", typeof(AudioClip)) as AudioClip;
        /// <summary>
        /// 까마귀 소리
        /// </summary>
        audioList[27] = Resources.Load("208", typeof(AudioClip)) as AudioClip;
        /// <summary>
        /// 까마귀 소리
        /// </summary>
        audioList[28] = Resources.Load("209", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// bgm
        /// </summary>
        bgmList[(int)SOUND_RESOURCE.BGM] = Resources.Load("BGM", typeof(AudioClip)) as AudioClip;
        bgmSource.

    }




    /// <summary>
    ///브금 재생 시작
    /// </summary>
    public void BgmPlay(SOUND_RESOURCE key)
    {
        Debug.Log(audioList[(int)key]);
        bgmSource.Stop();
        bgmSource.clip = audioList[(int)key];
        bgmSource.volume = 1.0f;
        bgmSource.Play();
        bgmSource.loop = true;  //반복재생
        nowClip = (int)key;

        Debug.Log(audioList[(int)key]);
    }
    
    
    /// <summary>
    ///효과음 재생
    /// </summary>
    public void playSound(SOUND_RESOURCE key)
    {
        effectSource.clip = audioList[(int)key];
        effectSource.Play();
        effectSource.loop = false;

    }

  

    /// <summary>
    ///볼륨조절(환경설정)
    /// </summary>
    public void volumeControl(SOUND_TYPE type, int volume)
    {
        bgmSource.volume = volume;
        effectSource.volume = volume;

    }

    /// <summary>
    /// 적 발소리 계산해서 볼륨조절
    /// </summary>
    public void walkSound()
    {
        int i = 0;

        //플레이어 최단경로
       // Scene_Manager.find_shortest(Enemy.get_enemy_spot(), Player.,i, new List<>());
    }
    
    

}

*/
