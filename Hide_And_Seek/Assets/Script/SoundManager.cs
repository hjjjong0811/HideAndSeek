using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class soundPack
{
    public AudioClip audioClip;    //오디오
    string name;            //이름
    public int key;
};

public class SoundManager :MonoBehaviour{

    /* 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    */

    public static SoundManager Instance = null;

    public soundPack[] audioList;//효과음 저장
    public AudioSource bgmSource, effectSource;

    float _volume = 0.5f;    //볼륨값
    int _nowBgm;    //지금 재생중인 브금 키값
    float _walkVolume=0.0f; //발소리 볼륨

    void Awake()
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
    

    //리스트 추가로 바꿀것
    //----------효과음------------

    /// <summary>
    /// 물웅덩이에 나는소리
    /// </summary>
    public AudioClip sWater;

    /// <summary>
    /// 차박차박소리
    /// </summary>
    public AudioClip sWalk;

    /// <summary>
    /// 심장소리 천천히
    /// </summary>
    public AudioClip sHeartBeat1;

    /// <summary>
    /// 심장소리 빠르게
    /// </summary>
    public AudioClip sHeartBeat2;

    /// <summary>
    /// 심장소리 점점 빠르게
    /// </summary>
    public AudioClip sHeartBeat3;

    /// <summary>
    /// 심장소리 점점 빠르게22
    /// </summary>
    public AudioClip sHeartBeat4;

    /// <summary>
    /// 물 차박
    /// </summary>
    public AudioClip sWaterWalk;

    /// <summary>
    /// 게임오버시 피튀기는 소리
    /// </summary>
    public AudioClip sDie;

    /// <summary>
    /// 꺼림칙한 웃음소리
    /// </summary>
    public AudioClip sLaugh1;

    /// <summary>
    /// 낮은 중저음 웃음소리
    /// </summary>
    public AudioClip sLaugh2;


    /// <summary>
    /// 미친웃음소리
    /// </summary>
    public AudioClip sLaugh3;

    /// <summary>
    /// 계단 올라가는 발소리
    /// </summary>
    public AudioClip sWalk2;

    /// <summary>
    /// 노크1
    /// </summary>
    public AudioClip sKnock1;

    /// <summary>
    /// 계단올라가는 소리
    /// </summary>
    public AudioClip sWalk3;

    /// <summary>
    /// 계단 올라가는소리, 발소리
    /// </summary>
    public AudioClip sWalk4;

    /// <summary>
    /// 울리는 발소리
    /// </summary>
    public AudioClip sWalk5;


    /// <summary>
    /// 조용히 발소리
    /// </summary>
    public AudioClip sWalk6;

    /// <summary>
    /// 노크2
    /// </summary>
    public AudioClip sKnock2;

    /// <summary>
    /// 문열리는 소리
    /// </summary>
    public AudioClip sDoor;

    /// <summary>
    ///여자 흐느끼는 소리
    /// </summary>
    public AudioClip sCry;

    /// <summary>
    /// 타이핑소리
    /// </summary>
    public AudioClip sTyping;

    /// <summary>
    /// 비명소리
    /// </summary>
    public AudioClip sScream;

    /// <summary>
    /// 일기장 페이지 넘기는 소리
    /// </summary>
    public AudioClip sBook;

    /// <summary>
    /// 티비 켜져있는데 아무것도 안보일때 소리
    /// </summary>
    public AudioClip sTV;

    /// <summary>
    /// 수영장 소리
    /// </summary>
    public AudioClip sSwimmingPool;

    /// <summary>
    /// 물 떨어지는 소리
    /// </summary>
    public AudioClip sWaterDrop;

    /// <summary>
    /// 연필로 글쓰는 소리
    /// </summary>
    public AudioClip sWrite;

    /// <summary>
    /// 까마귀 소리
    /// </summary>
    public AudioClip sKkac1;

    /// <summary>
    /// 까마귀 소리
    /// </summary>
    public AudioClip sKkac2;


    //-----------브금---------------


    /// <summary>
    ///시작화면 BGM
    /// </summary>
    public AudioSource startBGM;

    /// <summary>
    ///게임화면 BGM
    /// </summary>
    public AudioSource basicBGM;

    /// <summary>
    /// 장소가 다른 곳으로 갔을 때 처리할 AudioSource
    /// </summary>
    public AudioSource BGM;

    /// <summary>
    /// 효과음 처리 할 AudioSource
    /// </summary>
    public AudioSource EFXSource;

  





    /// <summary>
    ///브금 재생 시작
    /// </summary>
    public void BgmStart(int key)
    {
        
    }
    z
    /// <summary>
    ///브금 끄기
    /// </summary>
    public void stopBgm()
    {


    }

    /// <summary>
    ///효과음 재생
    /// </summary>
    public void playSound(int key)
    {


        
    }

    /// <summary>
    ///볼륨조절
    /// </summary>
    public void volumeControl(int key,int volume)
    {



    }

    /// <summary>
    /// 적 발소리 계산해서 볼륨조절
    /// </summary>
    public void walkSound()
    {

    }

    /// <summary>
    ///키값으로 효과음찾기
    /// </summary>
    public AudioClip findSound(int key)
    {
        //초기화
        AudioClip audio = null;

        for (int i = 0; i < audioList.Length; i++)
        {
            if (audioList[i].key == key)
            {
                audio = audioList[i].audioClip;
            }
        }

        
 

        return audio;
    }
    

}
