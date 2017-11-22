using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager instance = null;

    /*아이템 상수선언*/
    public const int SOJU = 12;
    public const int SALT = 18;
    public const int SALTYWATER = 22;
    public const int GROUNDKEY = 43;

    public static int MainChapter; // 전체 메인 에피소드 관리

    public int[] EndScene; // 씬 종료 체크 배열
    public int[] FindCharacter; // 친구찾기 배열
    public int[] DeadCharacter; // 죽은 친구 배열
    public int[] MeetCharacter; // 만난 친구 배열
    public int[] FindJeongyeon; // 정연 찾기 위해 1층 모든방 배열

   /*획득 아이템*/
    public int Soju;
    public int Salt;
    public int SaltyWater;
    public int GroundKey; // 지하실 열쇠

    /*상태변수*/
    public int BreakDisplay; // 장식장 깨뜨림
    public int Wallpaper; // 띠벽지 발견
    public int HomeConstruct; // 집구조도
    public int CorrectPassword; // 비밀번호 일치 여부


    private GameManager()
    {
        MainChapter = 0;

        EndScene = new int[2]{ 0,0 };
        FindCharacter = new int[4] { 0, 0, 0, 0 };
        DeadCharacter = new int[4] { 0, 0, 0, 0 };
        MeetCharacter = new int[4] { 0, 0, 0, 0 };
        FindJeongyeon = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };


        Soju = 0; Salt = 0; SaltyWater = 0; GroundKey = 0;
        BreakDisplay = 0; Wallpaper = 0; HomeConstruct = 0; CorrectPassword = 0;


    }

    public static GameManager getInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }
        return instance;
    }

    public void GetItem(int ItemKey) // 스토리에 관련된 아이템 얻을때
    {
        switch (ItemKey)
        {
            case SOJU: Soju = 1; break;
            case SALT: Salt = 1; break;
            case SALTYWATER: SaltyWater = 1; break;
            case GROUNDKEY: GroundKey = 1; break;
        }
    }
    
    public void CheckMainChapter() // 챕터넘어갈 이벤트 만족했는지 확인
    {
        if(GetMainChapter() == 0 && EndScene[0] != 1) {
            PlayScene.getInstance().playScene(PlayScene.numScene.tutorial);
        }
        /*M=0 튜토리얼*/
        if (GetMainChapter() == 0 && EndScene[0] == 1)
            SetMainChapter(1);

        /*M=1 1차숨바꼭질(친구찾기)*/
        else if (GetMainChapter() == 1 && CheckArray(FindCharacter, 4)) // 숨바꼭질로 친구들 다 찾았을때
            SetMainChapter(2);

        /*M=2 소금물없어서 소주찾으러*/
        else if (GetMainChapter() == 2 && Soju == 1) // 소주 획득시
            SetMainChapter(3);

        /*M=3 준비물 모두 모아서 장롱씬*/
        else if (GetMainChapter() == 3 && EndScene[1] == 1) // 2차 장롱씬 종료시
            SetMainChapter(4);

        /*M=4 시작 정연이 5번누르면 엔딩 또는 효정이 확인 */
        else if (GetMainChapter() == 4 && MeetCharacter[1] == 5) // 정연엔딩
            SetMainChapter(-2);

        else if (GetMainChapter() == 4 && DeadCharacter[0] == 1)
            SetMainChapter(5);

        /*M=5 소금을 얻어야 겠다*/
        else if (GetMainChapter() == 5 && MeetCharacter[2] == 1)
            SetMainChapter(6);

        else if (GetMainChapter() == 6 && Salt == 1)
            SetMainChapter(7);

        else if (GetMainChapter() == 7 && CheckArray(FindJeongyeon, 8))
            SetMainChapter(8);

        else if (GetMainChapter() >= 8 && SaltyWater == 1)
            SetMainChapter(MainChapter + 1);

        else if (GetMainChapter() >= 8 && BreakDisplay == 1)
            SetMainChapter(MainChapter + 1);

        else if (GetMainChapter() >= 8 && DeadCharacter[2] == 1)
            SetMainChapter(MainChapter + 1);

        /*M= 11 서운 죽음*/
        else if (GetMainChapter() == 11 && DeadCharacter[3] == 1)
            SetMainChapter(12);

        else if (GetMainChapter() == 12 && Wallpaper == 1)
            SetMainChapter(13);

        else if (GetMainChapter() == 13 && HomeConstruct == 1)
            SetMainChapter(14);

        else if (GetMainChapter() == 14 && GroundKey == 1)
            SetMainChapter(15);

        else if (GetMainChapter() == 15 && CorrectPassword == 1)
            SetMainChapter(16);

    }

    public bool CheckArray(int[] TestArray, int ArrayNum)
    {
        int Num = 0;

        for (int i = 0; i < ArrayNum; i++)
        {
            if (TestArray[i] == 1)
                Num += 1;

        }

        if (ArrayNum == Num)
            return true;

        else
            return false;
    }

    public int GetMainChapter() // 현재 챕터 반환
    {
        return MainChapter;
    }

    public void SetMainChapter(int Chapter) // 챕터 수정하기
    {
        MainChapter = Chapter;
    }


}



