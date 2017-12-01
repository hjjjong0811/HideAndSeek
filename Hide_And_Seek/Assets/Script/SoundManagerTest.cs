using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SOUND_NAME
{
    /// <summary>
    /// 브금
    /// </summary>
    STARTBGM=0,
    /// <summary>
    /// 심장두근1
    /// </summary>
    HEARTBEAT1,
    /// <summary>
    /// 심장두근2
    /// </summary>
    HEARTBEAT2,
    /// <summary>
    /// 심장두근3
    /// </summary>
    HEARTBEAT3,
    /// <summary>
    /// 노크하는소리
    /// </summary>
    KNOCK,
    /// <summary>
    /// 문천천히 여는소리
    /// </summary>
    DOOR,
    /// <summary>
    /// 아저씨 폰벨소리
    /// </summary>
    BELLSOUND,
    /// <summary>
    /// 아저씨 걷는소리
    /// </summary>
    WALK,
    /// <summary>
    /// 아저씨 점점 멀어지는소리
    /// </summary>
    FAR,
    /// <summary>
    /// 아저씨웃음소리
    /// </summary>
    LAUGH_ENEMY,
    /// <summary>
    /// 수도꼭지 돌리는소리
    /// </summary>
    TRUN_PIPE,
    /// <summary>
    /// 가스레인지 스토브소리
    /// </summary>
    STOVE,
    /// <summary>
    /// 그릇놓는소리1
    /// </summary>
    DISH1,
    /// <summary>
    /// 그릇놓는소리2
    /// </summary>
    DISH2,
    /// <summary>
    /// 문닫는소리
    /// </summary>
    DOOR_CLOSE,
    /// <summary>
    /// 문여는소리
    /// </summary>
    DOOR_OPEN,
    /// <summary>
    /// 물웅덩이소리1
    /// </summary>
    WATER1,
    /// <summary>
    /// 시계 똑딱소리1
    /// </summary>
    CLOCK1,
    /// <summary>
    /// 변기내리는소리
    /// </summary>
    TOILET,
    /// <summary>
    /// 세면대 물튼소리
    /// </summary>
    TURN_WATER,
    /// <summary>
    /// 수화기 드는소리
    /// </summary>
    PHONE_UP,
    /// <summary>
    /// 식기건드는소리(냄비?)
    /// </summary>
    POT,
    /// <summary>
    /// 식기건드리는소리(잔)
    /// </summary>
    CUP1,
    /// <summary>
    /// 책 페이지 넘기는소리
    /// </summary>
    BOOK,
    /// <summary>
    /// 전화연결중소리
    /// </summary>
    CALLING,
    /// <summary>
    /// 컵건드는소리
    /// </summary>
    CUP2,
    /// <summary>
    /// 와인잔 짠
    /// </summary>
    GLASS,
    /// <summary>
    /// 키 얻었을때(짤랑)
    /// </summary>
    GET_KEY,
    /// <summary>
    /// 튜브나 공 밟는소리
    /// </summary>
    TUBE,
    /// <summary>
    /// 티비 지지직 소리
    /// </summary>
    TV,
    /// <summary>
    /// 괘종시계
    /// </summary>
    BIG_CLOCK,
    /// <summary>
    /// 유리깨지는소리
    /// </summary>
    CRASH,
    /// <summary>
    /// 냉장고여는소리
    /// </summary>
    REFRI,
    /// <summary>
    /// 여자웃음소리(마네킹)
    /// </summary>
    LAUGH,
    /// <summary>
    /// 물웅덩이소리2
    /// </summary>
    WATER2,
    /// <summary>
    /// 서랍장 문여는 소리
    /// </summary>
    DESK,
    /// <summary>
    /// 시계 똑딱소리2
    /// </summary>
    CLOCK2,
    /// <summary>
    /// 세탁기 소리
    /// </summary>
    WASHER,
    /// <summary>
    /// 청소기소리
    /// </summary>
    VACUUM,
    /// <summary>
    /// 키보드타이핑소리1
    /// </summary>
    TYPING1,
    /// <summary>
    /// 환기구소리
    /// </summary>
    VENT,
    /// <summary>
    /// 물에 담궜다 손 빼는소리 water-swirl-small-23
    /// </summary>
    WATER_HAND,
    /// <summary>
    /// 흔들의자 소리 rockingchair4
    /// </summary>
    ROCKING_CHAIR,
    /// <summary>
    ///  양동이 소리 metal-lid-thumps-01
    /// </summary>
    BUCKET,
    /// <summary>
    /// 축음기 소리 gramophone
    /// </summary>
    GRAMOPHONE,
    /// <summary>
    /// 장식장 깨는소리 glass-smash-bottle-c
    /// </summary>
    HUTCH_CRASH,
    /// <summary>
    /// 생일축하 오르골 소리 music-box-happy-birthday
    /// </summary>
    MUSICBOX,
    /// <summary>
    /// 물 넘쳐서 흐르는 소리(졸졸)
    /// </summary>
    WATER_OVERFLOW,
    /// <summary>
    /// 키보드타이핑소리2
    /// </summary>
    TYPING2,
    /// <summary>
    /// 효정이비명소리
    /// </summary>
    HYOJUNG_KKYAK,
    /// <summary>
    /// 물웅덩이3
    /// </summary>
    WATER3,
    /// <summary>
    /// 까마귀소리
    /// </summary>
    CROW,
    /// <summary>
    /// 종이에 글쓰는소리
    /// </summary>
    WRITE,
    /// <summary>
    /// 게임내 브금
    /// </summary>
    GAMEBGM

};

public class SoundManagerTest : MonoBehaviour
{
    public static SoundManagerTest instance = null;

    public string[] audioName;
    public AudioClip[] audioClipList;

    public AudioSource bgmSource;
    public AudioSource effectSource;
    public AudioSource walkSource;

    public AudioClip audioClip;

    float soundVolume;
    int walkVolume;//(0~100) 발소리 사운드 볼륨*퍼센트

    bool isMute = false;//true면 음소거

    //접근용
    public static SoundManagerTest getInstance()
    {

        if (instance == null)
            instance = new SoundManagerTest();

        return instance;
    }

    //스타트보다 먼저실행
    void Awake()
    {


        if (SoundManagerTest.instance == null) //incetance가 비어있는지 검사합니다.
        {
            SoundManagerTest.instance = this; //자기자신을 담습니다.
        }
    }

    

    // Use this for initialization
    void Start()
    {
        soundVolume = 1.0f;


        bgmSource = new AudioSource();
        effectSource = new AudioSource();
        walkSource = new AudioSource();

        audioClipList = new AudioClip[50];

        bgmSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();
        walkSource = gameObject.AddComponent<AudioSource>();
        
        

        //-------리소스 긁어오기-------------------------
        object[] temp = Resources.LoadAll("Sounds");


        //001-003 / 221- 224 왜 로드안댐?
        for (int i = 0; i < temp.Length; i++)
        {
            audioClipList[i] = temp[i] as AudioClip;
        }


        //--------------------------------------------

        playBgm(audioClipList[0]);
        playEffect(audioClipList[1]);

        bgmSource.PlayOneShot(audioClip);
    }

    // Update is called once per frame
    void Update()
    {
        //walkSound 갱신
    }


    //==========재생할거 찾기=============
    public AudioClip findAudioClip(SOUND_NAME s)
    {
        return audioClipList[(int)s];
    }

    public AudioClip findAudioClip(string s)
    {
        for (int i = 0; i < audioClipList.Length; i++)
        {
            if (s == audioName[i])
            {
                return audioClipList[i];
            }
        }

        return null;
    }
    

    public void setWalkVolume(float volume)
    {
        soundVolume = volume;
    }

    //------------소스를 재생시키는것
    public void playBgm(AudioClip audio)
    {
        bgmSource.clip = audio;

        if (!isMute)
        {
            bgmSource.Play();
            bgmSource.loop = true;
            bgmSource.volume = soundVolume;
        }
    }

    public void playEffect(AudioClip audio)
    {

        effectSource.clip = audio;

        if (!isMute)
        {
            effectSource.Play();
            effectSource.loop = false;
            effectSource.volume = soundVolume;

        }
    }

    public void muteAll()
    {
        isMute = true;
        bgmSource.Stop();
        effectSource.Stop();
        walkSource.Stop();
    }

    public void unMute()
    {
        isMute = false;
        bgmSource.Play();
        walkSource.Play();
    }

    public void playWalkSound()
    {
        
    }
    




}

