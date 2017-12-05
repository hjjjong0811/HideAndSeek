using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SOUND_NAME
{
    /// <summary>
    /// 브금
    /// </summary>
    STARTBGM = 0,
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

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public AudioClip[] audioClipList;

    public AudioSource bgmSource;
    public AudioSource effectSource;
    public AudioSource walkSource;

    public AudioClip audioClip;

    public float volume_bgm, volume_effect;
    public int walkVolume;//(0~100) 발소리 사운드 볼륨*퍼센트

    public bool isMute = false;//true면 음소거
    public bool isMuteBgm = false;
    public bool isMuteEffect = false;

    //접근용
    public static SoundManager getInstance()
    {

        if (instance == null)
            instance = new SoundManager();

        return instance;
    }

    //스타트보다 먼저실행
    void Awake()
    {


        if (SoundManager.instance == null) //incetance가 비어있는지 검사합니다.
        {
            SoundManager.instance = this; //자기자신을 담습니다.
        }

        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }



    // Use this for initialization
    void Start()
    {
        //환경설정불러오기
        volume_bgm = PlayerPrefs.GetFloat("Sound_vol_bgm",1.0f);
        volume_effect = PlayerPrefs.GetFloat("Sound_vol_effect",1.0f);

        bgmSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();
        walkSource = gameObject.AddComponent<AudioSource>();


        //변수초기화
        effectSource.loop = false;
        walkVolume = 0;

        //-------리소스 긁어오기-------------------------
        object[] temp;
        temp = Resources.LoadAll("Sounds");
        audioClipList = new AudioClip[temp.Length]; //리소스갯수로변경함

        //226 - 227 / 221- 224 왜 로드안댐?
        for (int i = 0; i < temp.Length; i++)
        {
            audioClipList[i] = temp[i] as AudioClip;
        }

        //--------------------------------------------

        playBgm(audioClipList[(int)SOUND_NAME.STARTBGM]);
        walkSource.clip = audioClipList[(int)SOUND_NAME.WALK];

        bgmSource.PlayOneShot(audioClip);
    }

    // Update is called once per frame
    void Update()
    {
        //walkSound 갱신
        if (!isMute)
        {
            updateWalkSoundVolume();
        }
    }

    private void OnDestroy()
    {
        //환경설정저장
        PlayerPrefs.SetFloat("Sound_vol_bgm", volume_bgm);
        PlayerPrefs.SetFloat("Sound_vol_effect", volume_effect);
        PlayerPrefs.Save();
    }

    //==========재생할거 찾기=============
    public AudioClip findAudioClip(SOUND_NAME s)
    {
        return audioClipList[(int)s];
    }



    //------------소스를 재생시키는것
    public void playBgm()
    {
        if (!isMute)
        {
            bgmSource.Play();
            bgmSource.loop = true;
            bgmSource.volume = volume_bgm;
        }
    }

    public void playBgm(AudioClip audio)
    {
        bgmSource.clip = audio;

        if (!isMute)
        {
            bgmSource.Play();
            bgmSource.loop = true;
            bgmSource.volume = volume_bgm;
        }
    }

    public void playEffect()
    {
        if (!isMuteEffect)
        {
            effectSource.Play();
            effectSource.volume = volume_effect;

        }
    }

    public void playEffect(AudioClip audio)
    {
        AudioSource au;
        if (effectSource.isPlaying)
        {
            au = new GameObject().AddComponent<AudioSource>();
        }
        else
        {
            au = effectSource;
        }
        au.clip = audio;

        if (!isMuteEffect)
        {
            au.Play();
            au.volume = volume_effect;

        }
    }

    /// <summary>
    /// 효과음중 loop로 씬에 계속 재생하고싶은 경우
    /// </summary>
    public void playEffectLoop(AudioClip audio)
    {
        GameObject go = new GameObject();
        AudioSource au = go.AddComponent<AudioSource>();
        if (!isMuteEffect)
        {
            au.volume = volume_effect;
            au.clip = audio;
            au.loop = true;
            au.Play();
        }
    }

    public void setVolumeAll(float pVol_bgm, float pVol_effect)
    {
        volume_bgm = pVol_bgm;
        volume_effect = pVol_effect;
        effectSource.volume = pVol_effect;
        bgmSource.volume = pVol_bgm;
    }
    public void setVolumeBgm(float pVol_bgm)
    {
        volume_bgm = pVol_bgm;
        bgmSource.volume = pVol_bgm;
    }
    public void setVolumeEffect(float pVol_effect)
    {
        volume_effect = pVol_effect;
        effectSource.volume = pVol_effect;
    }


    public void muteAll()
    {
        isMute = true;
        bgmSource.volume = 0;
        effectSource.volume = 0;
        walkSource.volume = 0;
    }

    public void unMute()
    {
        isMute = false;
        bgmSource.volume = volume_bgm;
        effectSource.volume = volume_bgm;
        walkSource.volume = volume_bgm;
    }

    //-------정원추가----

    public void muteBgm()
    {
        isMuteBgm = true;
        bgmSource.volume = 0;

    }

    public void unMuteBgm()
    {
        isMuteBgm = false;
        bgmSource.volume = volume_bgm;

    }


    public void muteEffect()
    {
        isMuteEffect = true;
        effectSource.volume = 0;

    }

    public void unMuteEffect()
    {
        isMuteEffect = false;
        effectSource.volume = volume_bgm;

    }

    //-----------------------------

    public void playWalkSoundStart()
    {
        walkSource.Play();
        walkSource.loop = true;
    }

    public void updateWalkSoundVolume()
    {
        walkSource.volume = walkVolume * volume_bgm;

    }




}