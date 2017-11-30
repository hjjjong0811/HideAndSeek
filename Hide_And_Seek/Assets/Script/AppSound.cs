using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 사운드 목록이애오 키값으로 쓰면됨
/// </summary>
enum SOUND_RESOURCE
{
    /// <summary>
    /// 물웅덩이에 나는소리
    /// </summary>
    WATER = 0,

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
    BASIC_BGM

    /*

WATER
WALK1
HEART_BEAT1
HEART_BEAT2
HEART_BEAT3
HEART_BEAT4
WATER_WALK
DIE
LAUGH1
LAUGH2
LAUGH3
WALK2
KNOCK1
WALK3
WALK4
WALK5
WALK6
KNOCK2
DOOR
CRY
TYPING
SCREAM
BOOK
TV
SWIMMING_POOL
WATER_DROP
WRITE
KKACK1
KKACK2


 */

}

public class AppSound : MonoBehaviour
{

    // === 외부 파라미터 ======================================
    public static AppSound instance = null;
    public static int MAX_BGM=4;
    public static int MAX_EFFECT = 30;

    
    public List<AudioSource> effectList;


    
    [System.NonSerialized] public SoundManager fm;

    //한 소스에서 하나만 재생할거임
    public AudioSource bgmSource;
    public AudioSource effectSource;
    public AudioSource walkSource;


    //============BGM추가==============
    /// <summary>
    /// 시작브금
    /// </summary>
     public AudioSource BASIC_BGM;
    

    //===========EFFECT 추가-==========
    /// <summary>
    /// 물웅덩이에 나는소리
    /// </summary>
    public AudioSource WATER;

    /// <summary>
    /// 차박차박소리
    /// </summary>
     public AudioSource WALK1;

    /// <summary>
    /// 심장소리 천천히
    /// </summary>
    [System.NonSerialized] public AudioSource HEART_BEAT1;

    /// <summary>
    /// 심장소리 빠르게
    /// </summary>
    [System.NonSerialized] public AudioSource HEART_BEAT2;

    /// <summary>
    /// 심장소리 점점 빠르게
    /// </summary>
    [System.NonSerialized] public AudioSource HEART_BEAT3;

    /// <summary>
    /// 심장소리 점점 빠르게22
    /// </summary>
    [System.NonSerialized] public AudioSource HEART_BEAT4;

    /// <summary>
    /// 물 차박
    /// </summary>
    [System.NonSerialized] public AudioSource WATER_WALK;

    /// <summary>
    /// 게임오버시 피튀기는 소리
    /// </summary>
    [System.NonSerialized] public AudioSource DIE;

    /// <summary>
    /// 꺼림칙한 웃음소리
    /// </summary>
    [System.NonSerialized] public AudioSource LAUGH1;

    /// <summary>
    /// 낮은 중저음 웃음소리
    /// </summary>
    [System.NonSerialized] public AudioSource LAUGH2;


    /// <summary>
    /// 미친웃음소리
    /// </summary>
    [System.NonSerialized] public AudioSource LAUGH3;

    /// <summary>
    /// 계단 올라가는 발소리
    /// </summary>
    [System.NonSerialized] public AudioSource WALK2;

    /// <summary>
    /// 노크1
    /// </summary>
    [System.NonSerialized] public AudioSource KNOCK1;

    /// <summary>
    /// 계단올라가는 소리
    /// </summary>
    [System.NonSerialized] public AudioSource WALK3;

    /// <summary>
    /// 계단 올라가는소리, 발소리
    /// </summary>
    [System.NonSerialized] public AudioSource WALK4;

    /// <summary>
    /// 울리는 발소리
    /// </summary>
    [System.NonSerialized] public AudioSource WALK5;


    /// <summary>
    /// 조용히 발소리
    /// </summary>
    [System.NonSerialized] public AudioSource WALK6;

    /// <summary>
    /// 노크2
    /// </summary>
    [System.NonSerialized] public AudioSource KNOCK2;

    /// <summary>
    /// 문열리는 소리
    /// </summary>
    [System.NonSerialized] public AudioSource DOOR;

    /// <summary>
    ///여자 흐느끼는 소리
    /// </summary>
    [System.NonSerialized] public AudioSource CRY;

    /// <summary>
    /// 타이핑소리
    /// </summary>
    [System.NonSerialized] public AudioSource TYPING;

    /// <summary>
    /// 비명소리
    /// </summary>
    [System.NonSerialized] public AudioSource SCREAM;

    /// <summary>
    /// 일기장 페이지 넘기는 소리
    /// </summary>
    [System.NonSerialized] public AudioSource BOOK;

    /// <summary>
    /// 티비 켜져있는데 아무것도 안보일때 소리
    /// </summary>
    [System.NonSerialized] public AudioSource TV;

    /// <summary>
    /// 수영장 소리
    /// </summary>
    [System.NonSerialized] public AudioSource SWIMMING_POOL;

    /// <summary>
    /// 물 떨어지는 소리
    /// </summary>
    [System.NonSerialized] public AudioSource WATER_DROP;

    /// <summary>
    /// 연필로 글쓰는 소리
    /// </summary>
    [System.NonSerialized] public AudioSource WRITE;

    /// <summary>
    /// 까마귀 소리
    /// </summary>
    [System.NonSerialized] public AudioSource KKACK1;

    /// <summary>
    /// 까마귀 소리
    /// </summary>
    [System.NonSerialized] public AudioSource KKACK2;



    // === 내부 파라미터 ======================================
    string sceneName = "non";


    


    // === 코드 =============================================
    void Awake()
    {
        // 사운드
        fm = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        AudioSource[] bgmList = new AudioSource[MAX_BGM];


        // 배경음
        fm.CreateGroup("BGM");
        BASIC_BGM = fm.LoadResourcesSound("BGM", "BGM");
        WATER = fm.LoadResourcesSound("BGM", "001");
        WALK1 = fm.LoadResourcesSound("BGM", "001");
        HEART_BEAT1 = fm.LoadResourcesSound("BGM", "001");
        HEART_BEAT2 = fm.LoadResourcesSound("BGM", "001");
        HEART_BEAT3 = fm.LoadResourcesSound("BGM", "001");
        HEART_BEAT4 = fm.LoadResourcesSound("BGM", "001");
        WATER_WALK = fm.LoadResourcesSound("BGM", "001");
        DIE = fm.LoadResourcesSound("BGM", "001");
        LAUGH1 = fm.LoadResourcesSound("BGM", "001");
        LAUGH2 = fm.LoadResourcesSound("BGM", "001");
        LAUGH3 = fm.LoadResourcesSound("BGM", "001");
        WALK2 = fm.LoadResourcesSound("BGM", "001");
        KNOCK1 = fm.LoadResourcesSound("BGM", "001");
        WALK3 = fm.LoadResourcesSound("BGM", "001");
        WALK4 = fm.LoadResourcesSound("BGM", "001");
        WALK5 = fm.LoadResourcesSound("BGM", "001");
        WALK6 = fm.LoadResourcesSound("BGM", "001");
        KNOCK2 = fm.LoadResourcesSound("BGM", "001");
        DOOR = fm.LoadResourcesSound("BGM", "001");
        CRY = fm.LoadResourcesSound("BGM", "001");
        TYPING = fm.LoadResourcesSound("BGM", "001");
        SCREAM = fm.LoadResourcesSound("BGM", "001");
        BOOK = fm.LoadResourcesSound("BGM", "001");
        TV = fm.LoadResourcesSound("BGM", "001");
        SWIMMING_POOL = fm.LoadResourcesSound("BGM", "001");
        WATER_DROP = fm.LoadResourcesSound("BGM", "001");
        WRITE = fm.LoadResourcesSound("BGM", "001");
        KKACK1 = fm.LoadResourcesSound("BGM", "001");
        KKACK2 = fm.LoadResourcesSound("BGM", "001");

        bgmList[(int)SOUND_RESOURCE.BASIC_BGM] = BASIC_BGM;
        bgmList[(int)SOUND_RESOURCE.WATER] = WATER;
        bgmList[(int)SOUND_RESOURCE.WALK1] = WALK1;
        bgmList[(int)SOUND_RESOURCE.HEART_BEAT1] = HEART_BEAT1;
        bgmList[(int)SOUND_RESOURCE.HEART_BEAT2] = HEART_BEAT2;
        bgmList[(int)SOUND_RESOURCE.HEART_BEAT3] = HEART_BEAT3;
        bgmList[(int)SOUND_RESOURCE.HEART_BEAT4] = HEART_BEAT4;
        bgmList[(int)SOUND_RESOURCE.WATER_WALK] = WATER_WALK;
        bgmList[(int)SOUND_RESOURCE.DIE] = DIE;
        bgmList[(int)SOUND_RESOURCE.LAUGH1] = LAUGH1;
        bgmList[(int)SOUND_RESOURCE.LAUGH2] = LAUGH2;
        bgmList[(int)SOUND_RESOURCE.LAUGH3] = LAUGH3;
        bgmList[(int)SOUND_RESOURCE.WALK2] = WALK2;
        bgmList[(int)SOUND_RESOURCE.KNOCK1] = KNOCK1;
        bgmList[(int)SOUND_RESOURCE.WALK3] = WALK3;
        bgmList[(int)SOUND_RESOURCE.WALK4] = WALK4;
        bgmList[(int)SOUND_RESOURCE.WALK5] = WALK5;
        bgmList[(int)SOUND_RESOURCE.WALK6] = WALK6;
        bgmList[(int)SOUND_RESOURCE.KNOCK2] = KNOCK2;
        bgmList[(int)SOUND_RESOURCE.DOOR] = DOOR;
        bgmList[(int)SOUND_RESOURCE.CRY] = CRY;
        bgmList[(int)SOUND_RESOURCE.TYPING] = TYPING;
        bgmList[(int)SOUND_RESOURCE.SCREAM] = SCREAM;
        bgmList[(int)SOUND_RESOURCE.BOOK] = BOOK;
        bgmList[(int)SOUND_RESOURCE.TV] = TV;
        bgmList[(int)SOUND_RESOURCE.SWIMMING_POOL] = SWIMMING_POOL;
        bgmList[(int)SOUND_RESOURCE.WATER_DROP] = WATER_DROP;
        bgmList[(int)SOUND_RESOURCE.WRITE] = WRITE;
        bgmList[(int)SOUND_RESOURCE.KKACK1] = KKACK1;
        bgmList[(int)SOUND_RESOURCE.KKACK2] = KKACK2;


        //BASIC_BGM.Play();
        bgmList[(int)SOUND_RESOURCE.KKACK1].Play();

        instance = this;
    }

    void Start()
    {
        // 씬이 바뀌었는지 검사
        if (sceneName != Application.loadedLevelName)
        {
            sceneName = Application.loadedLevelName;

        }    
            // 볼륨 설정
            fm.SetVolume("BGM", 1.0f);
            //fm.SetVolume("SE" ,SaveData.SoundSEVolume);

        
    }

    private void addResource()
    {
       
    }
}

