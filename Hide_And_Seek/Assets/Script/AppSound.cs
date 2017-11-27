using UnityEngine;
using System.Collections;


public class AppSound : MonoBehaviour
{

    // === 외부 파라미터 ======================================
    public static AppSound instance = null;

    // 배경음
    [System.NonSerialized] public SoundManager fm;
    public AudioSource BGM;
    /*
    [System.NonSerialized] public AudioSource BGM_TITLE;
    [System.NonSerialized] public AudioSource BGM_HISCORE;
    [System.NonSerialized] public AudioSource BGM_HISCORE_RANKIN;
    [System.NonSerialized] public AudioSource BGM_STAGEA;
    [System.NonSerialized] public AudioSource BGM_STAGEB;
    [System.NonSerialized] public AudioSource BGM_STAGEB_ROOMSAKURA;
    [System.NonSerialized] public AudioSource BGM_BOSSA;
    [System.NonSerialized] public AudioSource BGM_BOSSB;
    [System.NonSerialized] public AudioSource BGM_ENDING;

    // 효과음
    [System.NonSerialized] public AudioSource SE_MENU_OK;
    [System.NonSerialized] public AudioSource SE_MENU_CANCEL;

    [System.NonSerialized] public AudioSource SE_ATK_A1;
    [System.NonSerialized] public AudioSource SE_ATK_A2;
    [System.NonSerialized] public AudioSource SE_ATK_A3;
    [System.NonSerialized] public AudioSource SE_ATK_B1;
    [System.NonSerialized] public AudioSource SE_ATK_B2;
    [System.NonSerialized] public AudioSource SE_ATK_B3;
    [System.NonSerialized] public AudioSource SE_ATK_ARIAL;
    [System.NonSerialized] public AudioSource SE_ATK_SYURIKEN;

    [System.NonSerialized] public AudioSource SE_HIT_A1;
    [System.NonSerialized] public AudioSource SE_HIT_A2;
    [System.NonSerialized] public AudioSource SE_HIT_A3;
    [System.NonSerialized] public AudioSource SE_HIT_B1;
    [System.NonSerialized] public AudioSource SE_HIT_B2;
    [System.NonSerialized] public AudioSource SE_HIT_B3;

    [System.NonSerialized] public AudioSource SE_MOV_JUMP;

    [System.NonSerialized] public AudioSource SE_ITEM_KOBAN;
    [System.NonSerialized] public AudioSource SE_ITEM_HYOUTAN;
    [System.NonSerialized] public AudioSource SE_ITEM_MAKIMONO;
    [System.NonSerialized] public AudioSource SE_ITEM_OHBAN;
    [System.NonSerialized] public AudioSource SE_ITEM_KEY;

    [System.NonSerialized] public AudioSource SE_OBJ_EXIT;
    [System.NonSerialized] public AudioSource SE_OBJ_OPENDOOR;
    [System.NonSerialized] public AudioSource SE_OBJ_SWITCH;
    [System.NonSerialized] public AudioSource SE_OBJ_BOXBROKEN;

    [System.NonSerialized] public AudioSource SE_CHECKPOINT;
    [System.NonSerialized] public AudioSource SE_EXPLOSION;
    */

    // === 내부 파라미터 ======================================
    string sceneName = "non";

    // === 코드 =============================================
    void Awake()
    {
        // 사운드
        fm = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        // 배경음
        fm.CreateGroup("BGM");
        fm.SoundFolder = "Sound/BGM/";
        BGM = fm.LoadResourcesSound("BGM", "001");
        
        

        instance = this;
    }

    void Update()
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

