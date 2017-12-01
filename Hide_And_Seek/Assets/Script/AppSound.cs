using UnityEngine;
using System.Collections;


public class AppSound : MonoBehaviour
{

    // === 외부 파라미터 ======================================
    public static AppSound instance = null;

    // 배경음
    [System.NonSerialized] public SoundManager fm;
    public AudioSource BGM;
    

    // === 내부 파라미터 ======================================
    string sceneName = "non";

    // === 코드 =============================================
    void Awake()
    {
        // 사운드
        fm = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        // 배경음
        fm.CreateGroup("BGM");
        BGM = fm.LoadResourcesSound("BGM", "001");



        instance = this;
    }

    void Start()
    {
        // 볼륨 설정
        fm.SetVolume("BGM", 1.0f);
        //fm.SetVolume("SE" ,SaveData.SoundSEVolume);

        //BGM = GetComponent<AudioSource>();

        BGM.Play();


    }
}


/*
using UnityEngine;
using System.Collections;


public class AppSound : MonoBehaviour
{

    // === 외부 파라미터 ======================================
    public static AppSound instance = null;

    // 배경음
    [System.NonSerialized] public SoundManager fm;
    public AudioSource BGM;

    // === 내부 파라미터 ======================================
    string sceneName = "non";

    // === 코드 =============================================
    void Awake()
    {
        // 사운드
        fm = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        // 배경음
        fm.CreateGroup("BGM");
        BGM = fm.LoadResourcesSound("BGM", "001");
        
        

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

            BGM.Play();

        
    }
}

*/
