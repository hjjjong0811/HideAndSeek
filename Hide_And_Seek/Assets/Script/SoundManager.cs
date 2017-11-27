﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 키값으로 쓰면됨
/// </summary>
enum SOUND_RESOURCE
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

    float _volume = 0.5f;    //볼륨값
    int _nowBgm;    //지금 재생중인 브금 키값
    float _walkVolume=0.0f; //발소리 볼륨


    

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


        //브금 틀기
        BgmPlay(0); 

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
        audioList[0] = Resources.Load("002", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 심장소리 천천히
        /// </summary>
        audioList[0] = Resources.Load("003", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 심장소리 빠르게
        /// </summary>
        audioList[0] = Resources.Load("004", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 심장소리 점점 빠르게
        /// </summary>
        audioList[0] = Resources.Load("005", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 심장소리 점점 빠르게22
        /// </summary>
        audioList[0] = Resources.Load("006", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 물 차박
        /// </summary>
        audioList[0] = Resources.Load("007", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 게임오버시 피튀기는 소리
        /// </summary>
        audioList[0] = Resources.Load("008", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 꺼림칙한 웃음소리
        /// </summary>
        audioList[0] = Resources.Load("100", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 낮은 중저음 웃음소리
        /// </summary>
        audioList[0] = Resources.Load("101", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 미친웃음소리
        /// </summary>
        audioList[0] = Resources.Load("102", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 계단 올라가는 발소리
        /// </summary>
        audioList[0] = Resources.Load("103", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 노크1
        /// </summary>
        audioList[0] = Resources.Load("104", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 계단올라가는 소리
        /// </summary>
        audioList[0] = Resources.Load("105", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 계단 올라가는소리, 발소리
        /// </summary>
        audioList[0] = Resources.Load("106", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 울리는 발소리
        /// </summary>
        audioList[0] = Resources.Load("107", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 조용히 발소리
        /// </summary>
        audioList[0] = Resources.Load("108", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 노크2
        /// </summary>
        audioList[0] = Resources.Load("109", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 문열리는 소리
        /// </summary>
        audioList[0] = Resources.Load("110", typeof(AudioClip)) as AudioClip;

        /// <summary>
        ///여자 흐느끼는 소리
        /// </summary>
        audioList[0] = Resources.Load("200", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 타이핑소리
        /// </summary>
        audioList[0] = Resources.Load("201", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 비명소리
        /// </summary>
        audioList[0] = Resources.Load("202", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 일기장 페이지 넘기는 소리
        /// </summary>
        audioList[0] = Resources.Load("203", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 티비 켜져있는데 아무것도 안보일때 소리
        /// </summary>
        audioList[0] = Resources.Load("204", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 수영장 소리
        /// </summary>
        audioList[0] = Resources.Load("205", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 물 떨어지는 소리
        /// </summary>
        audioList[0] = Resources.Load("206", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// 연필로 글쓰는 소리
        /// </summary>
        audioList[0] = Resources.Load("207", typeof(AudioClip)) as AudioClip;
        /// <summary>
        /// 까마귀 소리
        /// </summary>
        audioList[0] = Resources.Load("208", typeof(AudioClip)) as AudioClip;
        /// <summary>
        /// 까마귀 소리
        /// </summary>
        audioList[0] = Resources.Load("209", typeof(AudioClip)) as AudioClip;

        /// <summary>
        /// bgm
        /// </summary>
        audioList[0] = Resources.Load("BGM", typeof(AudioClip)) as AudioClip;

    }




    /// <summary>
    ///브금 재생 시작
    /// </summary>
    public void BgmPlay(int key)
    {
        bgmSource.Pause();
        bgmSource.clip = bgmList[key];
        bgmSource.Play();
        bgmSource.loop = true;  //반복재생
    }
    
    
    /// <summary>
    ///효과음 재생
    /// </summary>
    public void playSound(int key)
    {
        effectSource.clip = audioList[key];
        effectSource.Play();
        effectSource.loop = false;

    }

    /// <summary>
    ///볼륨조절(환경설정)
    /// </summary>
    public void volumeControl(int key,int volume)
    {
        bgmSource.pitch = volume;
        effectSource.pitch = volume;

    }

    /// <summary>
    /// 적 발소리 계산해서 볼륨조절
    /// </summary>
    public void walkSound()
    {
        int i = 0;
        //Scene_Manager.find_shortest(start, end, ref i, new List<>());
    }
    
    

}
