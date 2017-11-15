using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*아이템 상수선언*/
    public const int SOJU = 12;
    public const int SALT = 18;
    public const int SALTYWATER = 22;
    public const int GROUNDKEY = 43;

    public static int MainChapter = 0; // 전체 메인 에피소드 관리

    public static int[] EndScene = new int[2]; // 씬 종료 체크 배열
    public static int[] FindCharacter = new int[4]; // 친구찾기 배열
    public static int[] DeadCharacter = new int[4]; // 죽은 친구 배열
    public static int[] MeetCharacter = new int[4]; // 만난 친구 배열
    public static int[] FindJeongyeon = new int[8]; // 정연 찾기 위해 1층 모든방 배열

    public int Num = 0;

    /*획득 아이템*/
    public static int Soju = 0;
    public static int Salt = 0;
    public static int SaltyWater = 0;
    public static int GroundKey = 0; // 지하실 열쇠

    /*상태변수*/
    public static int BreakDisplay = 0; // 장식장 깨뜨림
    public static int Wallpaper = 0; // 띠벽지 발견
    public static int HomeConstruct = 0; // 집구조도
    public static int CorrectPassword = 0; // 비밀번호 일치 여부



    public void GetItem(int ItemKey)
    {
        switch (ItemKey)
        {
            case SOJU: Soju = 1; break;
            case SALT: Salt = 1; break;
            case SALTYWATER: SaltyWater = 1; break;
            case GROUNDKEY: GroundKey = 1; break;
        }
    }




    public void CheckEvent()
    {

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

        /*M=4 시작 정연이 3번누르면 엔딩 또는 효정이 확인 */
        else if (GetMainChapter() == 4 && MeetCharacter[1] == 3) // 정연엔딩
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
        Num = 0;

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



